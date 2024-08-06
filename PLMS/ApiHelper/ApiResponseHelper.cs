using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;

namespace PLMS.API.ApiHelper
{
    public static class ApiResponseHelper
    {
        public static ActionResult CreateResponse(bool isSuccess, string message, int statusCode = 200, string? token = null)
        {
            var response = new
            {
                IsSuccess = isSuccess,
                Message = message,
                Token = token
            };

            return new JsonResult(response)
            {
                StatusCode = statusCode
            };
        }

        public static ActionResult CreateErrorResponse(ModelStateDictionary modelState, int statusCode = 400)
        {
            var errorMessage = modelState.Values.First().Errors.First().ErrorMessage;
            var response = new
            {
                IsSuccess = false,
                Message = "Validation errors occurred.",
                Errors = string.Join(", ", errorMessage)
            };

            return new JsonResult(response)
            {
                StatusCode = statusCode
            };
        }
    }
}
