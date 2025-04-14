using BankAccountManagement.Models;
using BankAccountManagement.Models.Payload;
using BankAccountManagement.UI.Services.Api.Response;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Json;

namespace BankAccountManagement.UI.Services.Api
{
    public class LoanController: BaseApiService
    {
        public LoanController(HttpClient httpClient,
        IConfiguration configuration) : base(httpClient, configuration)
        {
            HttpClient.BaseAddress = new Uri(ApiServiceConfig.BaseAddress);
            ControllerName = ApiServiceConfig.LoanController.Name;
        }

        public async Task<IResponse<List<string>>> ValidateLoanEligibility(string userName)
        {
            var requestUri = $"{ApiServiceConfig.LoanController.Methods.ValidateLoanEligibility}{userName}";
            return await Get<List<string>>(requestUri);
        }

        public async Task<IResponse<List<string>>> ValidateLoanRequest(Loan loanRequest)
        {
            var requestUri = ApiServiceConfig.LoanController.Methods.ValidateLoanRequest;
            var content = JsonContent.Create(loanRequest);
            return await Post<List<string>>(requestUri, content);
        }

        public async Task<IResponse<bool>> CreateLoan(LoanRequest loanRequest)
        {
            var requestUri = ApiServiceConfig.LoanController.Methods.CreateLoan;
            var content = JsonContent.Create(loanRequest);
            return await Post<bool>(requestUri, content);
        }
    }
}
