using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountManagement.UI.Services.Api.Response;

internal interface IErrorResponse<out T> : IResponse<T>
{
    string Error { get; }
    string? OriginalData { get; }
    Exception? Exception { get; }
    bool HasException { get; }
}
