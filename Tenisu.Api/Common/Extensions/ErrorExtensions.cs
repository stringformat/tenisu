using Tenisu.Common;

namespace Tenisu.Api.Common.Extensions;

public static class ErrorExtensions
{
    public static IResult ToHttpResponse(this Error error) => Results.Json(new ErrorResponse(error.Message, error.Code), statusCode: (int)error.StatusCode);
    
    private sealed record ErrorResponse(string Message, Enum Code);
}