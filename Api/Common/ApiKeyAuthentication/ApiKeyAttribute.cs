using Microsoft.AspNetCore.Mvc;

namespace ContactManagerCS.Common.ApiKeyAuthentication;

public class ApiKeyAttribute : ServiceFilterAttribute
{
    public ApiKeyAttribute()
        : base(typeof(ApiKeyAuthFilter))
    {
    }
}
