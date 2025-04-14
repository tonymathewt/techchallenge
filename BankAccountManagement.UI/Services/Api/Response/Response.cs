using System.Net;

namespace BankAccountManagement.UI.Services.Api.Response;

internal class Response<T> : IResponse<T>
{
    internal Response(HttpStatusCode statusCode, Uri? requestUri = null)
          : this((int)statusCode, requestUri)
    {
    }

    internal Response(HttpStatusCode statusCode, T value, Uri? requestUri = null)
        : this((int)statusCode, value, requestUri)
    {
    }

    private Response(int statusCode, T value, Uri? requestUri)
    {
        StatusCode = statusCode;
        Value = value;
        RequestUri = requestUri;
    }

    private Response(int statusCode, Uri? requestUri)
    {
        StatusCode = statusCode;
        RequestUri = requestUri;
    }

    public int StatusCode { get; }
    public T Value { get; } = default!;
    public Uri? RequestUri { get; }
}
