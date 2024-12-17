using System.Net;

namespace DiscoveryService.Services;

public class DiscoveryService
{
    private Dictionary<string, List<string>> endpoints;
    private Dictionary<string, List<string>> routes;    
    private List<string> serviceNames;

    public DiscoveryService()
    {
        // Initilize in memory data store
        endpoints = new Dictionary<string, List<string>>();
        routes = new Dictionary<string, List<string>>();
        serviceNames = new List<string>();

        // Get data after initialization
        Refresh().GetAwaiter().GetResult();
    }
    
    // Gets endpoints for a service
    public List<string>? GetEndpoints(string serviceName)
    {
        endpoints.TryGetValue(serviceName, out var serviceEndpoints);
        return serviceEndpoints;
    }

    // Gets routes for a service
    public List<string>? GetRoutes(string serviceName)
    {
        routes.TryGetValue(serviceName, out var serviceRoutes);
        return serviceRoutes;
    }
    
    // Refresh discovery data
    public async Task Refresh()
    {
        // Clear in-memory data
        endpoints = new Dictionary<string, List<string>>();
        routes = new Dictionary<string, List<string>>();
        serviceNames = new List<string>();
        
        // Get service names to discover
        string envDS = Environment.GetEnvironmentVariable("DISCOVER_SERVICES") ?? string.Empty;
        // If no DISCOVER_SERVICES environment variable is set: throw exception
        if (envDS is "")
            throw new ArgumentException("DISCOVER_SERVICES environment variable is empty.");
        // Add service names to list of serviceNames
        foreach (string s in envDS.Split(", ")) {
            serviceNames.Add(s);            
        }
        
        // Discovery of endpoints and routes
        DiscoverEndpoints();
        foreach (var service in serviceNames)
            await DiscoverRoutesForServiceAsync(service);
    }
    
    private void DiscoverEndpoints()
    {
        // Grab service addresses from container network internal DNS server
        foreach (string service in serviceNames) {
            endpoints.Add(service, Dns.GetHostAddresses(service).ToList().ConvertAll<string>(addr => addr.ToString()));
        }
    }
    
    private async Task DiscoverRoutesForServiceAsync(string serviceName)
    {
        // Get routes from an instance of the service
        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri("http://" + endpoints[serviceName].First() + ":8080");
        var routesStr = await client.GetAsync("/discovery/routes").Result.Content.ReadAsStringAsync();
        
        // Collect into list of string
        List<string> routesForService = new List<string>();
        foreach (var route in routesStr.Split(", ")) {
            routesForService.Add(route);
            Console.WriteLine(serviceName + " - " + route);
        }
        
        // Add to in-memory store
        routes.Add(serviceName, routesForService);
    }
}