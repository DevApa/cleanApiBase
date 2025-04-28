namespace bg.crm.integration.application.interfaces.services
{
    public interface ITokenService
    {
        Task<string> GetTokenAsync(Dictionary<string, string>? tokenParams);
    }
}