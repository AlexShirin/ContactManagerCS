namespace ContactManagerCS.Common.ApiKeyAuthentication;

public interface IApiKeyValidation
{
    bool IsValidApiKey(string userApiKey);
}
