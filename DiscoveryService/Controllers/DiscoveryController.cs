using Microsoft.AspNetCore.Mvc;

namespace DiscoveryService.Controllers;

[ApiController]
[Route("[controller]")]
public class DiscoveryController(Services.DiscoveryService discover) : ControllerBase
{
    [HttpGet("RefreshDiscoveryData")]
    public async Task<IActionResult> RefreshDiscoveryData()
    {
        await discover.Refresh();
        return Ok();
    }

    [HttpGet("GetDiscoveredServices")]
    public IActionResult GetDiscoveredServices()
    {
        var services = discover.GetServiceNames();
        return Ok(services);
    }

    [HttpGet("GetRoutesForService")]
    public IActionResult GetRoutesForService([FromQuery] string serviceName)
    {
        var routes = discover.GetRoutes(serviceName);
        return routes is not null ? Ok(routes) : NotFound();
    }
    
    [HttpGet("GetAddressesForService")]
    public IActionResult GetAddressesForService([FromQuery] string serviceName)
    {
        var endpoints = discover.GetEndpoints(serviceName);
        return endpoints is not null ? Ok(endpoints) : NotFound();
    }

    [HttpGet("GetRandomAddressForService")]
    public IActionResult GetRandomAddressForService([FromQuery] string serviceName)
    {
        // Get endpoints
        var endpoints = discover.GetEndpoints(serviceName);
        if (endpoints is null)
            return NotFound();
        
        // Get random endpoint
        Random random = new();
        var endpoint = endpoints[random.Next(endpoints.Count)];
        return Ok(endpoint);
    }
}