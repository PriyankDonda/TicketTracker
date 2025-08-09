using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace TicketTracker.Helpers;

public static class ApiResponseHelper
{
    public static IActionResult Success<T>(T data, string message = "Request Successful")
    {
        return new OkObjectResult(new ApiResponse<T>
        {
            IsSuccess = true,
            Data = data,
            Message = message
        });
    }

    public static IActionResult Error<T>(T data, string message = "Request Failed",
        HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        return new ObjectResult(new ApiResponse<T>
        {
            IsSuccess = false,
            Message = message,
            Data = data
        })
        {
            StatusCode = (int)statusCode
        };
    }
}