using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using BankAccountManagement.UI.Constants;
using BankAccountManagement.UI.Enums;
using BankAccountManagement.UI.Services.Api.Configuration;
using BankAccountManagement.UI.Services.Api.Response;

namespace BankAccountManagement.UI.Services.Api;

public abstract class BaseApiService
{
    protected const string HeaderMediaType = "application/json";

    protected BaseApiService(IConfiguration configuration)
    {
        HttpClient = new HttpClient(new HttpClientHandler());

        InitializeConfigurations(configuration);
    }

    protected BaseApiService(HttpClient httpClient, IConfiguration configuration)
    {
        httpClient.DefaultRequestHeaders.Add("Accept", HeaderMediaType);
        HttpClient = httpClient;

        InitializeConfigurations(configuration);
    }

    private void InitializeConfigurations(IConfiguration configuration)
    {
        var sectionApiService = configuration.GetSection("ApiService");
        var configCmsAutomationApiService = sectionApiService.Get<ApiServiceConfiguration>();
        ApiServiceConfig = configCmsAutomationApiService ?? new ApiServiceConfiguration();
    }

    protected HttpClient HttpClient { get; }

    protected internal ApiServiceConfiguration ApiServiceConfig { get; private set; } = null!;

    protected string ControllerName { get; init; } = null!;

    protected async Task<IResponse<string>> GetResponseAsString(string requestUri)
    {
        var responseMessage = await HttpClient.GetAsync(requestUri);
        var responseContent = await responseMessage.Content.ReadAsStringAsync();

        return responseMessage.IsSuccessStatusCode
            ? new Response<string>(responseMessage.StatusCode, responseContent, responseMessage.RequestMessage?.RequestUri)
            : CreateErrorResponse<string>(
                statusCode: responseMessage.StatusCode,
                error: responseMessage.ReasonPhrase ?? string.Empty,
                originalData: responseContent,
                requestUri: responseMessage.RequestMessage?.RequestUri);
    }

    protected async Task<IResponse<T>> Get<T>(string requestUri)
    {
        return await PerformRequest<T>(HttpRequestType.Get, requestUri);
    }

    protected async Task<IResponse<T>> Post<T>(string requestUri, JsonContent? content = null)
    {
        return await PerformRequest<T>(HttpRequestType.Post, requestUri, content);
    }

    private async Task<IResponse<T>> PerformRequest<T>(HttpRequestType requestType, string requestUri, HttpContent? content = null)
    {
        HttpResponseMessage? responseMessage = null;
        var responseContent = string.Empty;

        try
        {
            switch (requestType)
            {
                case HttpRequestType.Get:
                    responseMessage = await HttpClient.GetAsync($"{ControllerName}{requestUri}");
                    break;

                case HttpRequestType.Post:
                    responseMessage = await HttpClient.PostAsync($"{ControllerName}{requestUri}", content);
                    break;

                case HttpRequestType.Put:
                    responseMessage = await HttpClient.PutAsync($"{ControllerName}{requestUri}", content);
                    break;

                default:
                    return CreateErrorResponse<T>(HttpStatusCode.MethodNotAllowed, CommonConstant.ErrorMessage.DoesNotSupportRequestMethod);
            }

            responseContent = await responseMessage.Content.ReadAsStringAsync();

            return responseMessage.IsSuccessStatusCode
                ? CreateResponse<T>(
                    responseMessage.StatusCode,
                    responseContent,
                    responseMessage.RequestMessage?.RequestUri)
                : CreateErrorResponse<T>(
                    statusCode: responseMessage.StatusCode,
                    error: responseMessage.ReasonPhrase ?? string.Empty,
                    originalData: responseContent,
                    requestUri: responseMessage.RequestMessage?.RequestUri);
        }
        catch (Exception? ex)
        {
            return CreateErrorResponse<T>(
                responseMessage?.StatusCode ?? HttpStatusCode.InternalServerError,
                ex.Message,
                ex,
                responseContent,
                responseMessage?.RequestMessage?.RequestUri);
        }
    }

    private static IResponse<T> CreateResponse<T>(HttpStatusCode statusCode, string json, Uri? requestUri)
    {
        if (statusCode == HttpStatusCode.NoContent)
        {
            return new Response<T>(statusCode, requestUri);
        }

        try
        {
            var data = JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
            return new Response<T>(statusCode, data ?? throw new InvalidOperationException(typeof(T).ToString()), requestUri);
        }
        catch (Exception ex)
        {
            return new ErrorResponse<T>(statusCode, CommonConstant.ErrorMessage.CouldNotDeserialize, requestUri)
            {
                OriginalData = json,
                Exception = ex
            };
        }
    }

    private static IResponse<T> CreateErrorResponse<T>(
        HttpStatusCode statusCode,
        string error,
        Exception? exception = null,
        string? originalData = null,
        Uri? requestUri = null)
    {
        return new ErrorResponse<T>(statusCode, error, requestUri)
        {
            Exception = exception,
            OriginalData = originalData
        };
    }
}
