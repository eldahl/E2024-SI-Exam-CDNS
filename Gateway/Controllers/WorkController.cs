using Gateway.Services;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers;

[ApiController]
[Route("[controller]")]
public class WorkController(HttpClient client, DiscoveryFetcherService dfs) : ControllerBase
{
    [HttpGet("GetHashes")]
    public async Task<ActionResult> GetHashes()
    {
        // Get address from discovery service
        var address = await dfs.GetRandomAddressForServiceAsync("dummy-worker1");
        address = "http://" + address + ":8080";
        client.BaseAddress = new Uri(address);
        
        // Perform request on endpoint
        var res = await client.GetAsync("RequestWork").Result.Content.ReadAsStringAsync();
        
        // Return work result
        return Ok(res);
    }
}