using Microsoft.AspNetCore.Http;
using XeGo.Shared.Lib.Constants;

namespace XeGo.Shared.Lib.Helpers
{
    public class RequestIdHelpers
    {
        public static string? GetRequestId(HttpContext httpContext)
        {
            string? result = null;
            if (httpContext.Request.Headers.TryGetValue(HeaderConstants.RequestIdHeaderName, out var requestId))
            {
                result = requestId.ToString();
            }

            return result;
        }

        public static void MapReqIdToRespHeaders(HttpContext httpContext, string requestId)
        {
            httpContext.Response.Headers.Add(HeaderConstants.RequestIdHeaderName, requestId);
        }
    }
}
