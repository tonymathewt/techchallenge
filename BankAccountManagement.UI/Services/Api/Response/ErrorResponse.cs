
using System.Net;

namespace BankAccountManagement.UI.Services.Api.Response;

internal class ErrorResponse<T> : IErrorResponse<T>
{
    internal ErrorResponse(HttpStatusCode statusCode, string error, Uri? requestUri)
        : this((int)statusCode, error, requestUri)
    {
    }

    private ErrorResponse(int statusCode, string error, Uri? requestUri)
    {
        StatusCode = statusCode;
        Error = error;
        RequestUri = requestUri;
    }

    public int StatusCode { get; }
    public string Error { get; }
    public string? OriginalData { get; internal init; }
    public Exception? Exception { get; internal init; }
    public bool HasException => Exception != null;
    T IResponse<T>.Value => default;
    public Uri? RequestUri { get; }
}
