using Microsoft.Extensions.Configuration;
using System.Net.Http;
using BankAccountManagement.Models;
using BankAccountManagement.UI.Services.Api.Response;

namespace BankAccountManagement.UI.Services.Api;

public class UserController: BaseApiService
{
    public UserController(HttpClient httpClient,
        IConfiguration configuration): base(httpClient, configuration)
    {
        HttpClient.BaseAddress = new Uri(ApiServiceConfig.BaseAddress);
        ControllerName = ApiServiceConfig.UserController.Name;
    }

    public async Task<IResponse<List<User>>> GetAll()
    {
        var requestUri = ApiServiceConfig.UserController.Methods.GetAll;
        return await Get<List<User>>(requestUri);
    }

    public async Task<IResponse<User>> GetUser(string userName)
    {
        var requestUri = $"{ApiServiceConfig.UserController.Methods.GetUser}{userName}";
        return await Get<User>(requestUri);
    }
}
