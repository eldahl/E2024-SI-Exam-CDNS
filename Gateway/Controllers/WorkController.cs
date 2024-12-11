using System.Text.Json;
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
        // TODO: Implement
        // Get address from kv store
        client.BaseAddress = new Uri(dfs.GetRandomAddressForService("dummy-worker"));
        
        // Perform request on endpoint
        var res = await client.GetAsync("RequestWork").Result.Content.ReadAsStringAsync();
        
        // Return work result
        return Ok(res);
    }
}