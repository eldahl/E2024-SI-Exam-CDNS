using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace DiscoveryService;

public class DiscoveryFilePersistence : IDiscoveryPersistence
{
    private readonly string _filePath;
    
    public DiscoveryFilePersistence(string filePath)
    {
        _filePath = filePath;
        
        // Create file
        File.Create(_filePath);
    }

    public void SaveOne(KeyValuePair<string, string> endpoint)
    {
        var json = JsonSerializer.Serialize(endpoint);
        File.AppendAllText(_filePath, json);
    }

    public void SaveMany(Dictionary<string, string> endpoints)
    {
        var json = JsonSerializer.Serialize(endpoints);
        File.AppendAllText(_filePath, json);
    }

    public void OverwriteAll(Dictionary<string, string> endpoints)
    {
        var json = JsonSerializer.Serialize(endpoints);
        File.WriteAllText(_filePath, json);
    }

    public async Task SaveOneAsync(KeyValuePair<string, string> endpoint)
    {
        var json = JsonSerializer.Serialize(endpoint);
        await File.AppendAllTextAsync(_filePath, json);
    }

    public async Task SaveManyAsync(Dictionary<string, string> endpoints)
    {
        var json = JsonSerializer.Serialize(endpoints);
        await File.AppendAllTextAsync(_filePath, json);
    }

    public async Task OverwriteAllAsync(Dictionary<string, string> endpoints)
    {
        var json = JsonSerializer.Serialize(endpoints);
        await File.WriteAllTextAsync(_filePath, json);
    }

    public KeyValuePair<string, string>? GetOne(string endpointIdentifier)
    {
        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(_filePath));
        if (dict is null)
            return null;
        
        dict.TryGetValue(endpointIdentifier, out var endpoint);
        if (endpoint is null)
            return null;
        
        return new KeyValuePair<string, string>(endpointIdentifier, endpoint);
    }

    public Dictionary<string, string>? GetMany(List<string> endpointIdentifiers)
    {
        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(_filePath));
        if (dict is null)
            return null;

        var returnDict = new Dictionary<string, string>();
        foreach (var endpointIdent in endpointIdentifiers)
        {
            dict.TryGetValue(endpointIdent, out var endpoint);
            if (endpoint is null)
                continue;
            
            returnDict.Add(endpointIdent, endpoint);
        }

        return returnDict.Count > 0 ? returnDict : null;
    }

    public Dictionary<string, string>? GetAll()
    {
        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(_filePath));
        if (dict is null)
            return null;
        return dict;
    }

    public async Task<KeyValuePair<string, string>?> GetOneAsync(string endpointIdentifier)
    {
        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(await File.ReadAllTextAsync(_filePath));
        if (dict is null)
            return null;
        
        dict.TryGetValue(endpointIdentifier, out var endpoint);
        if (endpoint is null)
            return null;
        
        return new KeyValuePair<string, string>(endpointIdentifier, endpoint);
    }

    public async Task<Dictionary<string, string>?> GetManyAsync(List<string> endpointIdentifiers)
    {
        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(await File.ReadAllTextAsync(_filePath));
        if (dict is null)
            return null;

        var returnDict = new Dictionary<string, string>();
        foreach (var endpointIdent in endpointIdentifiers)
        {
            dict.TryGetValue(endpointIdent, out var endpoint);
            if (endpoint is null)
                continue;
            
            returnDict.Add(endpointIdent, endpoint);
        }

        return returnDict.Count > 0 ? returnDict : null;
    }

    public async Task<Dictionary<string, string>?> GetAllAsync()
    {
        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(await File.ReadAllTextAsync(_filePath));
        if (dict is null)
            return null;
        return dict;
    }

    public void DeleteOne(string endpointIdentifier)
    {
        var dict = GetAll();
        if (dict is null)
            return;
        dict.Remove(endpointIdentifier);
        OverwriteAll(dict);
    }

    public void DeleteMany(List<string> endpointIdentifiers)
    {
        var dict = GetAll();
        if (dict is null)
            return;
        foreach (var endpointIdent in endpointIdentifiers)
            dict.Remove(endpointIdent);
        OverwriteAll(dict);
    }

    public void DeleteAll()
    {
        File.WriteAllText(_filePath, string.Empty);
    }

    public async Task DeleteOneAsync(string endpointIdentifier)
    {
        var dict = await GetAllAsync();
        if (dict is null)
            return;
        dict.Remove(endpointIdentifier);
        await OverwriteAllAsync(dict);
    }

    public async Task DeleteManyAsync(List<string> endpointIdentifiers)
    {
        var dict = await GetAllAsync();
        if (dict is null)
            return;
        foreach (var endpointIdent in endpointIdentifiers)
            dict.Remove(endpointIdent);
        await OverwriteAllAsync(dict);
    }

    public async Task DeleteAllAsync()
    {
        await File.WriteAllTextAsync(_filePath, string.Empty);
    }
}