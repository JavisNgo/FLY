using AutoMapper;
using FLY.Business.Exceptions;
using FLY.Business.Models.Account;
using FLY.Business.Models.GoogleCloud;
using FLY.DataAccess.Entities;
using FLY.DataAccess.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FLY.Business.Services.Implements
{
    public class AuthService : IAuthService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration, IMemoryCache memoryCache, IEmailService emailService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _memoryCache = memoryCache;
            _emailService = emailService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
        }
        public async Task<AuthResponse> AuthenticateAccount(AuthRequest authRequest)
        {
            try
            {
                string hashedPassword = await HashedPassword(authRequest.Password);
                var account = _unitOfWork.AccountRepository.
                    FindAsync(a => a.Email == authRequest.Email && a.Password == hashedPassword).Result.
                    FirstOrDefault();
                if (account != null) return _mapper.Map<AuthResponse>(account);
                else return null;
            }
            catch (Exception ex)
            {
                throw new ApiException(System.Net.HttpStatusCode.InternalServerError, $"Internal Error: {ex.Message}");
            }
        }

        private async Task<string> HashedPassword(string password)
        {
            try
            {
                using (SHA512 sha512 = SHA512.Create())
                {
                    byte[] hashBytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(password));

                    StringBuilder stringBuilder = new StringBuilder();
                    for (int i = 0; i < hashBytes.Length; i++)
                    {
                        stringBuilder.Append(hashBytes[i].ToString("x2"));
                    }
                    return await Task.FromResult<string?>(stringBuilder.ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> AuthenticateAccountAdvanced(AuthResponse authResponse)
        {
            try
            {
                string emailReturn = authResponse.Email;
                var account = _unitOfWork.AccountRepository.FindAsync(a => a.Email == emailReturn).Result.FirstOrDefault();
                if (account != null)
                {
                    await SendVerificationEmail(emailReturn);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task SendVerificationEmail(string email)
        {
            try
            {
                string verificationCode = await GenerateVerificationCode();
                _memoryCache.Set(email, verificationCode, TimeSpan.FromMinutes(60));
                await _emailService.SendVerificationEmailAsync(email, verificationCode);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<VerifyForgetPasswordRequest> VerifyRecoverAccount(VerifyForgetPasswordRequest request)
        {
            try
            {
                if (_memoryCache.TryGetValue($"Recover_{request.Email}", out string? cachedCode) && cachedCode == request.Code)
                {
                    var account = _unitOfWork.AccountRepository.FindAsync(a => a.Email == request.Email).Result.FirstOrDefault();
                    if (account != null)
                    {
                        _memoryCache.Remove($"Recover_{request.Email}");
                        var verificationCode = await GenerateVerificationCode();
                        _memoryCache.Set($"NewPassword_{request.Email}", verificationCode, TimeSpan.FromMinutes(60));
                        return new VerifyForgetPasswordRequest { Email = request.Email, Code = verificationCode };
                    }
                    else
                    {
                        throw new ApiException(HttpStatusCode.NotFound, "Account not found");
                    }
                }
                else
                {
                    throw new ApiException(HttpStatusCode.NotFound, "CacheCode not found ");
                }
            }
            catch (ApiException ex)
            {
                throw new ApiException(ex.statusCode, ex.Message);
            }
        }

        public async Task SendVerificationEmailForRegister(string email)
        {
            try
            {
                // Send verification email
                string verificationCode;
                verificationCode = await GenerateVerificationCode();
                _memoryCache.Set($"Register_{email}", verificationCode, TimeSpan.FromMinutes(60));
                await _emailService.SendVerificationEmailAsync(email, verificationCode);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<string> GenerateVerificationCode()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        public async Task<(string accessToken, string refreshToken)> GenerateTokens(string email, string verificationCode)
        {
            try
            {
                var account = _unitOfWork.AccountRepository.FindAsync(a => a.Email == email).Result.FirstOrDefault();
                if (account != null)
                {
                    if (account.Status == 1)
                    {
                        if (_memoryCache.TryGetValue(email, out string? cachedCode) && cachedCode == verificationCode)
                        {
                            try
                            {
                                var accessTokenHandler = new JwtSecurityTokenHandler();
                                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
                                string test = account.RoleId.ToString();
                                var accessTokenDescriptor = new SecurityTokenDescriptor
                                {
                                    Subject = new ClaimsIdentity(new[]
                                    {
                                        new Claim(ClaimTypes.Role, account.RoleId.ToString()),
                                        new Claim(ClaimTypes.Email, account.Email),
                                        new Claim("accountId", account.AccountId.ToString())
                                    }),
                                    Expires = DateTime.UtcNow.AddHours(1),
                                    Issuer = _configuration["Jwt:Issuer"],
                                    Audience = _configuration["Jwt:Audience"],
                                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                                };
                                var accessTokenInfo = accessTokenHandler.CreateToken(accessTokenDescriptor);
                                var accessToken = accessTokenHandler.WriteToken(accessTokenInfo);

                                var refreshTokenHandler = new JwtSecurityTokenHandler();
                                var refreshTokenDescriptor = new SecurityTokenDescriptor
                                {
                                    Subject = new ClaimsIdentity(new[]
                                    {
                                        new Claim(ClaimTypes.Email, account.Email)
                                    }),
                                    Expires = DateTime.UtcNow.AddDays(7),
                                    Issuer = _configuration["Jwt:Issuer"],
                                    Audience = _configuration["Jwt:Audience"],
                                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                                };
                                var refreshTokenInfo = refreshTokenHandler.CreateToken(refreshTokenDescriptor);
                                var refreshToken = refreshTokenHandler.WriteToken(refreshTokenInfo);

                                // Store refresh token in the database
                                var token = new RefreshToken
                                {
                                    AccountId = account.AccountId,
                                    Token = refreshToken,
                                    ExpiredDate = DateTime.UtcNow.AddDays(7),
                                    Status = 1,
                                    DeviceName = "Unknown"
                                };

                                await _unitOfWork.RefreshTokenRepository.InsertAsync(token);
                                await _unitOfWork.SaveAsync();

                                _memoryCache.Remove(email);

                                return (accessToken, refreshToken);

                            }
                            catch (Exception ex)
                            {
                                throw new Exception(ex.Message);
                            }
                        }
                        else
                        {
                            throw new Exception("Invalid verification code.");
                        }
                    }
                    else
                    {
                        if (_memoryCache.TryGetValue(email, out string? cachedCode) && cachedCode == verificationCode)
                        {
                            _memoryCache.Remove(email);
                        }
                        throw new Exception("You cannot access your account now, please contact administrator");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return (null, null);
        }

        public async Task<(string accessToken, string refreshToken)> GenerateTokens(string email)
        {
            try
            {
                var cacheKey = $"Account_{email}";
                if(_memoryCache.TryGetValue(cacheKey, out string? item))
                {
                    _memoryCache.Remove(cacheKey);
                }
                var sessionId = Guid.NewGuid().ToString();
                _memoryCache.Set($"Account_{email}", sessionId, TimeSpan.FromDays(7));
                var accounts = await _unitOfWork.AccountRepository.FindAsync(a => a.Email == email);
                if (accounts.Any())
                {
                    var account = accounts.FirstOrDefault();

                    var accessTokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
                    var accessTokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new[]
                        {
                            new Claim(ClaimTypes.Role, account.RoleId.ToString()),
                            new Claim(ClaimTypes.Email, account.Email),
                            new Claim("accountId", account.AccountId.ToString()),
                            new Claim("sessionId", sessionId)
                        }),
                        Expires = DateTime.UtcNow.AddHours(1),
                        Issuer = _configuration["Jwt:Issuer"],
                        Audience = _configuration["Jwt:Audience"],
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var accessTokenInfo = accessTokenHandler.CreateToken(accessTokenDescriptor);
                    var accessToken = accessTokenHandler.WriteToken(accessTokenInfo);
                    //Create RefreshToken
                    var refreshTokenHandler = new JwtSecurityTokenHandler();
                    var refreshTokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new[]
                        {
                            new Claim(ClaimTypes.Email, account.Email)
                        }),
                        Expires = DateTime.UtcNow.AddDays(7),
                        Issuer = _configuration["Jwt:Issuer"],
                        Audience = _configuration["Jwt:Audience"],
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var refreshTokenInfo = refreshTokenHandler.CreateToken(refreshTokenDescriptor);
                    var refreshToken = refreshTokenHandler.WriteToken(refreshTokenInfo);

                    // Store refresh token in the database
                    var token = new RefreshToken
                    {
                        AccountId = account.AccountId,
                        Token = refreshToken,
                        ExpiredDate = DateTime.UtcNow.AddDays(7),
                        Status = 1,
                        DeviceName = "Unknown"
                    };

                    await _unitOfWork.RefreshTokenRepository.InsertAsync(token);
                    await _unitOfWork.SaveAsync();

                    return (accessToken, refreshToken);
                }
                return (null, null);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> VerifyAccount(string email, string verificationCode)
        {
            if (_memoryCache.TryGetValue(email, out string? cachedCode) && cachedCode == verificationCode)
            {
                var account = _unitOfWork.AccountRepository.FindAsync(a => a.Email == email).Result.FirstOrDefault();
                if (account != null)
                {
                    using (var transaction = _unitOfWork.BeginTransaction())
                    {
                        try
                        {
                            if (account.Status == 2)
                            {
                                account.Status = 1;
                                await _unitOfWork.AccountRepository.UpdateAsync(account);
                                await _unitOfWork.SaveAsync();
                                await transaction.CommitAsync();
                                return true;
                            }
                            else
                            {
                                throw new Exception("Cannot verify your account");
                            }
                        }
                        catch
                        {
                            await transaction.RollbackAsync();
                            _memoryCache.Remove(email);
                            return false;
                        }
                    }
                }
                else { throw new ApiException(System.Net.HttpStatusCode.NotFound, "Account not found"); }
            }
            else { return false; }
        }

        public async Task<bool> RegisterCustomer(RegisterRequest registerCustomerRequest)
        {
            var existAccount = await _unitOfWork.AccountRepository.FindAsync(a => a.Email == registerCustomerRequest.Email);
            if (existAccount.Any()) throw new ApiException(HttpStatusCode.BadRequest, "Email already registered");
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var account = _mapper.Map<Account>(registerCustomerRequest);
                    account.Status = 2;
                    account.RoleId = 3;
                    account.Password = await HashedPassword(account.Password);
                    await _unitOfWork.AccountRepository.InsertAsync(account);
                    await _unitOfWork.SaveAsync();
                    await transaction.CommitAsync();
                    return true;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }
        }
        public async Task<bool> RegisterSeller(RegisterSellerRequest registerSellerRequest)
        {
            var accounts = await _unitOfWork.AccountRepository.FindAsync(a => a.Email == registerSellerRequest.Email);
            var existAccount = accounts.FirstOrDefault();
            if (existAccount != null)
            {
                var properties = existAccount.GetType().GetProperties();
                var incompleteProperties = properties
                    .Where(property => property.GetValue(existAccount) == null ||
                        (property == typeof(string) && string.IsNullOrEmpty(property.GetValue(existAccount) as string)))
                    .Select(property => property.Name);
                if(incompleteProperties.Any())
                {
                    var incompleteFieldNames = string.Join(", ", incompleteProperties);
                    throw new ApiException(HttpStatusCode.BadRequest, $"You need to complete the following information before registering as a seller: {incompleteFieldNames}");
                }
                else
                {
                    using (var transaction = _unitOfWork.BeginTransaction())
                    {
                        try
                        {
                            existAccount.Status = 2;
                            existAccount.RoleId = 2;
                            await _unitOfWork.AccountRepository.UpdateAsync(existAccount);
                            await _unitOfWork.SaveAsync();
                            await transaction.CommitAsync();
                            return true;
                        }
                        catch
                        {
                            await transaction.RollbackAsync();
                            return false;
                        }
                    }
                }
            } else
            {
                using (var transaction = _unitOfWork.BeginTransaction())
                {
                    try
                    {
                        var account = _mapper.Map<Account>(registerSellerRequest);
                        account.Status = 2;
                        account.RoleId = 2;
                        account.Password = await HashedPassword(account.Password);
                        await _unitOfWork.AccountRepository.InsertAsync(account);
                        await _unitOfWork.SaveAsync();
                        await transaction.CommitAsync();
                        return true;
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                        return false;
                    }
                }
            }
            
        }

        public async Task<(string accessToken, string refreshToken)> RefreshingAccessToken(string oldRefreshToken)
        {
            var existedRefreshToken = _unitOfWork.RefreshTokenRepository.FindAsync(r => r.Token == oldRefreshToken).
                Result.
                FirstOrDefault();
            if (existedRefreshToken != null && existedRefreshToken.ExpiredDate >= DateTime.UtcNow)
            {
                using (var transaction = _unitOfWork.BeginTransaction())
                {
                    try
                    {
                        var account = _unitOfWork.AccountRepository.FindAsync(a => a.AccountId == existedRefreshToken.AccountId)
                        .Result
                        .FirstOrDefault();
                        if (account != null)
                        {
                            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                            var accessClaims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Role, account.RoleId.ToString()),
                                new Claim(ClaimTypes.Email, account.Email),
                                new Claim("accountId", account.UserName)
                            };
                            var accessExpiration = DateTime.UtcNow.AddHours(1);
                            var accessJwt = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], accessClaims, expires: accessExpiration, signingCredentials: credentials);
                            var newAccessToken = new JwtSecurityTokenHandler().WriteToken(accessJwt);

                            var refreshClaims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Email, account.Email)
                            };
                            var refreshExpiration = DateTime.UtcNow.AddDays(7);
                            var refreshJwt = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], refreshClaims, expires: accessExpiration, signingCredentials: credentials);
                            var newRefreshToken = new JwtSecurityTokenHandler().WriteToken(accessJwt);

                            existedRefreshToken.Token = newRefreshToken;
                            existedRefreshToken.ExpiredDate = refreshExpiration;

                            await _unitOfWork.RefreshTokenRepository.UpdateAsync(existedRefreshToken);
                            await _unitOfWork.SaveAsync();

                            return (newAccessToken, newRefreshToken);
                        }
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                    }
                }
            }
            return (null, null);
        }

        public async Task ChangeNewPassword(ChangeNewPasswordRequest request)
        {
            try
            {
                if (_memoryCache.TryGetValue($"NewPassword_{request.Email}", out string? cachedCode) 
                    && cachedCode == request.Code)
                {
                    var accounts = await _unitOfWork.AccountRepository.FindAsync(a => a.Email == request.Email);
                    if (accounts.Any())
                    {
                        var account = accounts == null ? throw new Exception("Cannot find your account") : accounts.FirstOrDefault();
                        account.Password = await HashedPassword(request.Password);
                        await _unitOfWork.AccountRepository.UpdateAsync(account);
                        await _unitOfWork.SaveAsync();
                        _memoryCache.Remove($"NewPassword_{request.Email}");
                    }
                }
                else
                {
                    throw new Exception("Invalid code");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> VerifyRegisterAccount(EmailVerificationView request)
        {
            if (_memoryCache.TryGetValue($"Register_{request.Email}", out string? cachedCode) && cachedCode == request.Code)
            {
                using (var transaction = _unitOfWork.BeginTransaction())
                {
                    string hashedPassword = await HashedPassword(request.Password);
                    try
                    {
                        var account = _unitOfWork.AccountRepository
                            .FindAsync(a => a.Email == request.Email && a.Password == hashedPassword)
                            .Result
                            .FirstOrDefault();
                        if (account != null)
                        {
                            if (account.Status == 2)
                            {
                                account.Status = 1;
                                await _unitOfWork.AccountRepository.UpdateAsync(account);
                                await _unitOfWork.SaveAsync();
                                await transaction.CommitAsync();
                            }
                            else
                            {
                                throw new Exception("Cannot verify your account");
                            }
                        }
                        else
                        {
                            throw new Exception("Cannot verify your account");
                        }
                        _memoryCache.Remove($"Register_{request.Email}");
                        return true;
                    }
                    catch
                    {
                        _memoryCache.Remove($"Register_{request.Email}");
                        await transaction.RollbackAsync();
                        return false;
                    }
                }
            }
            else return false;
        }

        public async Task SendRecoveringVerificationEmail(string email)
        {
            try
            {
                var account = _unitOfWork.AccountRepository.FindAsync(a => a.Email == email).Result.FirstOrDefault();
                if(account != null)
                {
                    string verificationCode = await GenerateVerificationCode();
                    _memoryCache.Set($"Recover_{email}", verificationCode, TimeSpan.FromMinutes(60));
                    await _emailService.SendVerificationEmailAsync(email, verificationCode);
                }
                else
                {
                    throw new Exception("Your email doesn't match any acount in system");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
