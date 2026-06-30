using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ProgressService.Shared;

namespace ProgressService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiBaseController : ControllerBase
    {
        //Handle Result Without Value
        //If Result Is Success Return NoContent 204 
        //If Result IS Failure Return Problem With Status Code and Error Details 

        protected IActionResult HandleResult(Result result)
        {
            if (result.IsSuccess)
                return NoContent(); //204
            else
            {
                return HandleProblem(result.Errors);
            }
        }

        //Handle With Value
        //If Result Is Success Return Ok 200 With Value
        //If Result IS Failure Return Problem With Status Code and Error Details

        protected ActionResult<TValue> HandleResult<TValue>(Result<TValue> result)
        {
            if (result.IsSuccess)
                return Ok(result.Value); //200
            else
            {
                return HandleProblem(result.Errors);
            }
        }

        private ActionResult HandleProblem(IReadOnlyList<Error> errors)
        {
            //If No Errors Are Provided , Return 500 Error
            if (errors.Count == 0)
                return Problem(statusCode: StatusCodes.Status500InternalServerError);

            //If All Errors are Validation Errors , Handle them as Validation problem
            if (errors.All(e => e.Type == ErrorType.Validation))
                return HandleValidationProblem(errors);

            //If There Is Only One Error Handle it as a single error problem
            return HandleSingleErrorProblem(errors[0]);
        }

        private ActionResult HandleSingleErrorProblem(Error error)
        {
            return Problem(
                title: error.Code,
                detail: error.Description,
                type: error.Type.ToString(),
                statusCode: MapErrorTypeToStatusCode(error.Type));

        }
        private int MapErrorTypeToStatusCode(ErrorType errorType) => errorType switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ErrorType.InvalidCredentials => StatusCodes.Status401Unauthorized,
            ErrorType.Failure => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };

        private ActionResult HandleValidationProblem(IReadOnlyList<Error> errors)
        {
            var modelState = new ModelStateDictionary();
            foreach (var item in errors)
            {
                modelState.AddModelError(item.Code, item.Description);
            }

            return ValidationProblem(modelState);
        }
    }
}
