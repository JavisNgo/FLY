using AutoMapper;
using FLY.Business.Models.Feedback;
using FLY.DataAccess.Entities;
using FLY.DataAccess.Repositories;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace FLY.Business.Services.Implements
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FeedbackService(IUnitOfWork unitOfWork, IMapper mapper,
                                IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;

        }

        public async Task<FeedbackResponse> CreateFeedback(FeedbackRequest request)
        {
            var accountExist = _unitOfWork.AccountRepository.FindAsync(x => x.AccountId == request.AccountId);
            var shopExist = _unitOfWork.ShopRepository.FindAsync(x => x.ShopId == request.ShopId);

            if(accountExist == null || shopExist == null)
            {
                return null;
            }

            var map = _mapper.Map<Feedback>(request);
            map.Status = 2;
            await _unitOfWork.FeedbackRepository.InsertAsync(map);
            await Task.Delay(500);
            await _unitOfWork.SaveAsync();

            var response = _mapper.Map<FeedbackResponse>(map);

            return response;
        }

        public async Task<bool> DeleteFeedback(int feedbackId, int accountId)
        {
            var verifyAccountId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("accountId")?.Value);

            if (verifyAccountId != accountId)
            {
                return false;
            }

            var fbExist = await _unitOfWork.FeedbackRepository.GetAsync(filter: x => x.FeedbackId == feedbackId 
                                                                            && x.AccountId == verifyAccountId);
            var final = fbExist.FirstOrDefault();

            if (final == null)
            {
                return false;
            }

            await _unitOfWork.FeedbackRepository.DeleteAsync(final.FeedbackId);
            await Task.Delay(500);
            await _unitOfWork.SaveAsync();

            return true;
        }

        public async Task<IList<FeedbackResponse>> GetAllFeedbackOfShop(int shopId)
        {
            var getAllFbOShop = await _unitOfWork.FeedbackRepository
                                            .GetAsync(filter: x => x.ShopId == shopId
                                                                && x.Status != 2, 
                                                                includeProperties: "Shop,Account");

            var response = _mapper.Map<IList<FeedbackResponse>>(getAllFbOShop);

            return response;
        }

        public async Task<FeedbackResponse> GetOneFb(int shopId, int accountId)
        {
            var getAll = await _unitOfWork.FeedbackRepository
                                            .GetAsync(filter: x => x.ShopId == shopId
                                                                && x.Status != 2
                                                                && x.AccountId == accountId,
                                                                includeProperties: "Shop,Account");
            var final = getAll.FirstOrDefault();
            var response = _mapper.Map<FeedbackResponse>(final);
            return response;
        }

        public async Task<FeedbackResponse> UpdateFeedback(int feedbackId, FeedbackRequest request)
        {
            var verifyAccountId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("accountId")?.Value);

            if (verifyAccountId != request.AccountId)
            {
                return null;
            }

            var getAll = await _unitOfWork.FeedbackRepository
                                            .GetAsync(filter: x => x.ShopId == request.ShopId
                                                                && x.AccountId == verifyAccountId
                                                                && x.FeedbackId == feedbackId,
                                                                includeProperties: "Shop,Account");
            var final = getAll.FirstOrDefault();

            if(final == null)
            {
                return null;
            }

            var map = _mapper.Map(request, final);
            map.Status = 2;

            await _unitOfWork.FeedbackRepository.UpdateAsync(map);
            await Task.Delay(500);
            await _unitOfWork.SaveAsync();

            var response = _mapper.Map<FeedbackResponse>(map);

            return response;

        }

        public async Task<bool> UpdateStsAdmin(int feedbackId, UpdateFeedbackRequest request)
        {
            var getAll = await _unitOfWork.FeedbackRepository
                                            .GetAsync(filter: x => x.FeedbackId == feedbackId);

            var final = getAll.FirstOrDefault();

            if(final == null)
            {
                return false;
            }

            var map = _mapper.Map(request,final);
            await _unitOfWork.FeedbackRepository.UpdateAsync(map);
            await Task.Delay(500);
            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}
