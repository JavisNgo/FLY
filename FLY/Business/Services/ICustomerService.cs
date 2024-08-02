using FLY.Business.Models.Account;
using FLY.Business.Models.Blog;
using FLY.Business.Models.Customer;

namespace FLY.Business.Services
{
    public interface ICustomerService
    {
        Task<bool> UpdateCustomerInformation(UpdateInfoRequest request);
        Task<AccountResponse> GetByAccountIdAsync(int accountId);
        Task<bool> UpdateCustomer(CustomerResponse response);


    }
}