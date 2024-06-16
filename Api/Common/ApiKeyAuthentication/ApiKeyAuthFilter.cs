using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ContactManagerCS.Common.ApiKeyAuthentication;

public class ApiKeyAuthFilter : IAuthorizationFilter
{
    private readonly IApiKeyValidation _apiKeyValidation;
    public ApiKeyAuthFilter(IApiKeyValidation apiKeyValidation)
    {
        _apiKeyValidation = apiKeyValidation;
    }
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        //string userApiKey = context.HttpContext.Request.Headers[Constants.ApiKeyHeaderName].ToString();
        string userApiKey = context.HttpContext.Request.Query[Constants.ApiKeyName].ToString();

        if (string.IsNullOrWhiteSpace(userApiKey))
        {
            context.Result = new BadRequestResult();
            return;
        }

        if (!_apiKeyValidation.IsValidApiKey(userApiKey))
            context.Result = new UnauthorizedResult();
    }
}
