using System.Net;
using DiscoveryService.Persistence;

namespace DiscoveryService.Services;

public class DiscoveryService
{
    private readonly DiscoveryFilePersistence _persistence;
    
    private Dictionary<string, List<string>> _discoveryStore; 
    private List<string> _serviceNames;

    public DiscoveryService(DiscoveryFilePersistence persistence)
    {
        // Persistence instance for saving to file
        _persistence = persistence;
        
        // Initialize in memory data store
        _discoveryStore = new Dictionary<string, List<string>>();
        _serviceNames = new List<string>();

        // Fetch data after initialization
        Refresh().GetAwaiter().GetResult();
    }
    
    private string FormatServiceAddresses(string serviceName) => serviceName + "-addresses";
    private string FormatServiceRoutes(string serviceName) => serviceName + "-routes";

    // Gets service names provided as environment variable
    public List<string> GetServiceNames()
    {
        return _serviceNames;
    }

    // Gets endpoints for a service
    public List<string>? GetEndpoints(string serviceName)
    {
        _discoveryStore.TryGetValue(
            FormatServiceAddresses(serviceName),
            out var serviceEndpoints);
        return serviceEndpoints;
    }

    // Gets routes for a service
    public List<string>? GetRoutes(string serviceName)
    {
        _discoveryStore.TryGetValue(
            FormatServiceRoutes(serviceName), 
            out var serviceRoutes);
        return serviceRoutes;
    }
    
    // Refresh discovery data
    public async Task Refresh()
    {
        // Clear in-memory data
        _discoveryStore = new Dictionary<string, List<string>>();
        _serviceNames = new List<string>();
        
        // Get service names to discover
        var envDs = Environment.GetEnvironmentVariable("DISCOVER_SERVICES") ?? string.Empty;
        // If no DISCOVER_SERVICES environment variable is set: throw exception
        if (envDs is "")
            throw new ArgumentException("DISCOVER_SERVICES environment variable is empty.");
        // Add service names to list of serviceNames
        foreach (var s in envDs.Split(", ")) {
            _serviceNames.Add(s);            
        }
        
        // Discovery of endpoints and routes
        DiscoverEndpoints();
        foreach (var service in _serviceNames)
            await DiscoverRoutesForServiceAsync(service);

        // Write to file when information has been gathered
        await _persistence.OverwriteAllAsync(_discoveryStore);
    }
    
    private void DiscoverEndpoints()
    {
        // Grab service addresses from container network internal DNS server
        foreach (string service in _serviceNames) {
            _discoveryStore.Add(
                FormatServiceAddresses(service), 
                Dns.GetHostAddresses(service).ToList().ConvertAll<string>(addr => addr.ToString()));
        }
    }
    
    private async Task DiscoverRoutesForServiceAsync(string serviceName)
    {
        // Get routes from an instance of the service
        var client = new HttpClient();
        client.BaseAddress = new Uri("http://" + _discoveryStore[FormatServiceAddresses(serviceName)].First() + ":8080");
        var routesStr = await client.GetAsync("/discovery/routes").Result.Content.ReadAsStringAsync();
        
        // Collect into list of string
        List<string> routesForService = new List<string>();
        foreach (var route in routesStr.Split(", ")) {
            routesForService.Add(route);
            
            // Debug
            Console.WriteLine(serviceName + " - " + route);
        }
        
        // Add to in-memory store
        _discoveryStore.Add(FormatServiceRoutes(serviceName), routesForService);
    }

    
}