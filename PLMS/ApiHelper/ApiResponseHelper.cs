using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PLMS.API.ApiHelper
{
    public static class ApiResponseHelper
    {
        public static ActionResult CreateOkResponseWithMessage<T>(string message, T? data = default)
        {
            var response = new
            {
                IsSuccess = true,
                Message = message,
                Data = data
            };

            return new JsonResult(response)
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        public static ActionResult CreateOkResponseWithoutMessage<T>(T? data = default)
        {
            var response = new
            {
                Data = data
            };

            return new JsonResult(response)
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        public static ActionResult CreateErrorResponse(string error, int statusCode)
        {

            var response = new
            {
                IsSuccess = false,
                Errors = error
            };

            return new JsonResult(response)
            {
                StatusCode = statusCode
            };
        }

        public static ActionResult CreateValidationErrorResponse(ModelStateDictionary ModelState)
        {
            var errorMessage = string.Join(", ", ModelState.Values.First().Errors.First().ErrorMessage);
            var response = new
            {
                IsSuccess = false,
                Errors = errorMessage
            };

            return new JsonResult(response)
            {
                StatusCode = StatusCodes.Status400BadRequest
            };
        }
    }
}
