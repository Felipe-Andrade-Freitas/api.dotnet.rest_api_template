using System.Net;
using Application.Common.Controllers;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Exceptions;

namespace ProductService.Controllers;

[ApiController]
public class BaseApiController : ControllerBase
{
    private ISender Mediator => HttpContext.RequestServices.GetRequiredService<ISender>();
    private ILogger<BaseApiController> Logger => HttpContext.RequestServices.GetRequiredService<ILogger<BaseApiController>>();

    protected async Task<ActionResult<BaseResult<TResult>>> Result<TRequest, TResult>(TRequest request, Func<TResult, ActionResult<BaseResult<TResult>>>? returnFunction = null)
    {
        try
        {
            if (request is null) throw new CustomException(ErrorsMessages.Validate.BadRequest);

            var result = await Mediator.Send(request);
            return result is null ? Result<TResult>(HttpStatusCode.OK, data: default) :
                returnFunction != null
                ? returnFunction((TResult)result)
                : Result(HttpStatusCode.OK, (TResult)result);
        }
        catch (ValidationException e)
        {
            Logger.LogError(e, $"Erro no resultado da controller");
            return new ObjectResult(Result(HttpStatusCode.BadRequest, e))
            { StatusCode = (int)HttpStatusCode.BadRequest };
        }
        catch (CustomException e)
        {
            Logger.LogError(e, $"Erro no resultado da controller");
            return new ObjectResult(Result(e.StatusCode, e)) { StatusCode = (int)e.StatusCode };
        }
        catch (Exception e)
        {
            Logger.LogError(e, $"Erro no resultado da controller");
            return new ObjectResult(Result(HttpStatusCode.BadRequest, e)) { StatusCode = (int)HttpStatusCode.BadRequest };
        }
    }

    private BaseResult Result(HttpStatusCode status, Exception exception) => new(status, exception.Message, exception.StackTrace ?? "", new List<string> { exception.InnerException?.Message ?? "No Inner Exception" });
    private BaseResult Result(HttpStatusCode status, CustomException exception) => new(status, exception.Message, exception.StackTrace ?? "", exception.ErrorMessages);
    private BaseResult Result(HttpStatusCode status, ValidationException exception) => new(status, ErrorsMessages.Validate.BadRequest, exception.StackTrace ?? "", exception.Errors.Select(validationFailure => validationFailure.ErrorMessage).ToList());
    protected BaseResult<T> Result<T>(HttpStatusCode status, T? data) => new(status, data);
}