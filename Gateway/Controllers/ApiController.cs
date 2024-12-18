using Gateway.Services;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers;

[ApiController]
[Route("[controller]")]
public class ApiController(HttpClient client, DiscoveryFetcherService dfs) : ControllerBase
{
    [HttpGet("GetHashes")]
    public async Task<ActionResult> GetHashes()
    {
        // Get services from Discovery service
        var services = await dfs.GetDiscoveredServices();
        if (services is null) return NotFound();
        
        // Select random service
        var random = new Random();
        var service = services[random.Next(services.Count)];
        
        // Get available routes from discovery service for the service:
        var routes = await dfs.GetRoutesForServiceAsync(service);
        if (routes is null) return NotFound();
        
        // Get address from discovery service
        var address = await dfs.GetRandomAddressForServiceAsync(service);
        address = "http://" + address + ":8080";
        client.BaseAddress = new Uri(address);
        
        // Perform request on the index 1 route
        var res = await client.GetAsync(routes[1]).Result.Content.ReadAsStringAsync();
        
        // Return work result
        return Ok(res);
    }
}