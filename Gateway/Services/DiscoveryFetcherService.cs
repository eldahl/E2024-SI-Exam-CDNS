using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;

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

    public async Task<List<string>?> GetDiscoveredServices()
    {
        // Get service names from Discovery service
        var res = await _client.GetAsync("/Discovery/GetDiscoveredServices").Result.Content.ReadAsStringAsync();
        //Console.WriteLine(res);
        
        // Deserialize to list of strings
        var serviceNames = JsonSerializer.Deserialize<List<string>>(res) ?? null;
        return serviceNames;
    }
    
    public async Task<List<string>?> GetRoutesForServiceAsync(string serviceName)
    {
        // Get Routes for service
        var res = await _client.GetAsync("/Discovery/GetRoutesForService?serviceName=" + serviceName).Result.Content.ReadAsStringAsync();
        //Console.WriteLine(res);
        // Convert to list of strings from json
        var routes = JsonSerializer.Deserialize<List<string>>(res) ?? null;
        
        if (routes is null)
            return null;
    
        List<String> routeStrings = new();
        foreach (var route in routes) {
            // Example of split:
            // HTTP: GET /discovery/routes
            //   0  | 1 |       2
            routeStrings.Add(route.Split(" ")[2]);
        }
        return routeStrings;
    }

    public async Task<List<string>?> GetAddressesForServiceAsync(string serviceName)
    {
        // Get Routes for service
        var res = await _client.GetAsync("/Discovery/GetAddressesForService?serviceName=" + serviceName).Result.Content.ReadAsStringAsync();
        // Convert to list of strings from json
        var addresses = JsonSerializer.Deserialize<List<string>>(res) ?? null;
        return addresses ?? null;
    }

    public async Task<string> GetRandomAddressForServiceAsync(string serviceName)
    {
        // Get Routes for service
        var res = await _client.GetAsync("/Discovery/GetRandomAddressForService?serviceName=" + serviceName).Result.Content.ReadAsStringAsync();
        if (string.IsNullOrEmpty(res))
            throw new ApplicationException("No address found for service: " + serviceName);
        return res;
    }
}