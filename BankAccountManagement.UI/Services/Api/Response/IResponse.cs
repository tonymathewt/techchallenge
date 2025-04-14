namespace BankAccountManagement.UI.Services.Api.Response;

public interface IResponse<out T>
{
    int StatusCode { get; }
    T Value { get; }
    Uri? RequestUri { get; }
}
