using Microsoft.AspNetCore.Mvc;

namespace PLMS.API.ApiHelper
{
    public static class ApiResponseHelper
    {
        public static ActionResult CreateOkResponse<T>(string message, int statusCode = 200, T data = default)
        {
            var response = new
            {
                IsSuccess = true,
                Message = message,
                Data = data
            };

            return new JsonResult(response)
            {
                StatusCode = statusCode
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
    }
}
