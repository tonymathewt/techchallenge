using BankAccountManagement.Models;
using BankAccountManagement.Models.Payload;
using BankAccountManagement.UI.Services.Api.Response;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountManagement.UI.Services.Api
{
    public class AccountController : BaseApiService
    {
        public AccountController(HttpClient httpClient,
        IConfiguration configuration) : base(httpClient, configuration)
        {
            HttpClient.BaseAddress = new Uri(ApiServiceConfig.BaseAddress);
            ControllerName = ApiServiceConfig.AccountController.Name;
        }

        public async Task<IResponse<List<Account>>> GetAll()
        {
            var requestUri = ApiServiceConfig.AccountController.Methods.GetAll;
            return await Get<List<Account>>(requestUri);
        }

        public async Task<IResponse<List<Account>>> GetUserAccounts(int userId)
        {
            var requestUri = $"{ApiServiceConfig.AccountController.Methods.GetUserAccounts}{userId}";
            return await Get<List<Account>>(requestUri);
        }

        public async Task<IResponse<bool>> TransferMoney(TransferMoney transferRequest)
        {
            var requestUri = ApiServiceConfig.AccountController.Methods.TransferMoney;
            var content = JsonContent.Create(transferRequest);
            return await Post<bool>(requestUri, content);
        }
    }
}
