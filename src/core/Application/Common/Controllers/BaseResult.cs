using System.Net;

namespace Application.Common.Controllers;

public class BaseResult
{
    protected BaseResult()
    {
    }

    public BaseResult(HttpStatusCode status)
    {
        Status = status;
    }

    public BaseResult(HttpStatusCode status, string title, string? stackTrace, List<string>? errorMessages)
    {
        Status = status;
        Title = title;
        StackTrace = stackTrace;
        ErrorMessages = errorMessages;
    }

    public HttpStatusCode Status { get; set; }
    public string? Title { get; set; }
    public ICollection<string>? ErrorMessages { get; set; }
    public string? StackTrace { get; set; }
}


public class BaseResult<T> : BaseResult
{
    public BaseResult()
    {
        
    }
    
    public BaseResult(HttpStatusCode status, T? data) : base(status)
    {
        Data = data;
    }

    public T? Data { get; set; }
}