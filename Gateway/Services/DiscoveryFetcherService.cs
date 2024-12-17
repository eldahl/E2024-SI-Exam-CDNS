namespace Gateway.Services;

public class DiscoveryFetcherService
{
    private readonly HttpClient _client;
    public DiscoveryFetcherService(HttpClient client) {
        _client = client;
        // Get DISCOVERY_URL environment variable
        var envDiscoveryUrl = Environment.GetEnvironmentVariable("DISCOVERY_URL");
        if (envDiscoveryUrl is null)
            throw new ApplicationException("Environment variable 'DISCOVERY_URL' is missing");
        
        // Set base address in http client
        _client.BaseAddress = new Uri(envDiscoveryUrl);
    }
    
    public List<string> GetRoutesForService(string serviceName)
    {
        // TODO: Implement
        return null!;
    }

    public List<string> GetAddressesForService(string serviceName)
    {
        // TODO: Implement
        return null!;
    }

    public string GetRandomAddressForService(string serviceName)
    {
        // TODO: Implement
        return serviceName;
    }
}