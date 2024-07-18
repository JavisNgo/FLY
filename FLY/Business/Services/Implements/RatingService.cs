using AutoMapper;
using FLY.Business.Models.Rating;
using FLY.DataAccess.Repositories;

namespace FLY.Business.Services.Implements
{
    public class RatingService : IRatingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RatingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<RatingResponse> CreateRate(RatingRequest request)
        {
            var checkRateOfShop = await _unitOfWork.RatingRepository
                                                .GetAsync(filter: x => x.ShopId == request.ShopId
                                                        && x.AccountId == request.AccountId,
                                                        includeProperties: "Account,Shop");
            var firstOrDefault = checkRateOfShop.FirstOrDefault();

            if (firstOrDefault != null)
            {
                return null;
            }

            var mapper = _mapper.Map(request, firstOrDefault);
            mapper.Status = 1;

            await _unitOfWork.RatingRepository.InsertAsync(mapper);
            await Task.Delay(3000);
            await _unitOfWork.SaveAsync();

            var response = _mapper.Map<RatingResponse>(mapper);

            return response;
        }

        public async Task<RatingResponse> UpdateRate(RatingRequest request)
        {
            var checkRateOfShop = await _unitOfWork.RatingRepository
                                                .GetAsync(filter: x => x.ShopId == request.ShopId
                                                        && x.AccountId == request.AccountId,
                                                        includeProperties: "Account,Shop");
            var firstOrDefault = checkRateOfShop.FirstOrDefault();

            if (firstOrDefault == null)
            {
                return null;
            }

            var mapper = _mapper.Map(request, firstOrDefault);
            mapper.Status = 1;

            await _unitOfWork.RatingRepository.UpdateAsync(mapper);
            await Task.Delay(3000);
            await _unitOfWork.SaveAsync();

            var response = _mapper.Map<RatingResponse>(mapper);

            return response;
        }

        public async Task<RatingResponse> GetRatingShop(int shopId, int accountId)
        {
            var totalRateOfShop = await _unitOfWork.RatingRepository
                                                .GetAsync(filter: x => x.ShopId == shopId
                                                                    && x.Status == 1);

            var getRateUrSelf = await _unitOfWork.RatingRepository
                                                .GetAsync(filter: x => x.ShopId == shopId
                                                                    && x.AccountId == accountId
                                                                    && x.Status == 1,
                                                          includeProperties: "Account,Shop");

            var getFirst = getRateUrSelf.FirstOrDefault();

            var averageNum = totalRateOfShop.Any() ? totalRateOfShop.Average(x => x.RateNumber) : 0;

            var reponse = _mapper.Map<RatingResponse>(getFirst);
            reponse.RateNumber = getFirst != null ? getFirst.RateNumber : 0;
            reponse.AverageNumber = averageNum;

            return reponse;
        }

    }
}
