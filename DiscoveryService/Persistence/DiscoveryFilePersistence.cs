using System.Text.Json;

namespace DiscoveryService.Persistence;

public class DiscoveryFilePersistence : IDiscoveryDictionaryPersistence<string, List<string>>
{
    private readonly string _filePath;
    
    public DiscoveryFilePersistence(string filePath)
    {
        _filePath = filePath;
        
        // Create file
        File.Create(_filePath).Close();
    }

    public void SaveOne(KeyValuePair<string, List<string>> kvp)
    {
        var json = JsonSerializer.Serialize(kvp);
        File.AppendAllText(_filePath, json);
    }

    public void SaveMany(Dictionary<string, List<string>> dict)
    {
        var json = JsonSerializer.Serialize(dict);
        File.AppendAllText(_filePath, json);
    }

    public void OverwriteAll(Dictionary<string, List<string>> dict)
    {
        var json = JsonSerializer.Serialize(dict);
        File.WriteAllText(_filePath, json);
    }

    public async Task SaveOneAsync(KeyValuePair<string, List<string>> kvp)
    {
        var json = JsonSerializer.Serialize(kvp);
        await File.AppendAllTextAsync(_filePath, json);
    }

    public async Task SaveManyAsync(Dictionary<string, List<string>> dict)
    {
        var json = JsonSerializer.Serialize(dict);
        await File.AppendAllTextAsync(_filePath, json);
    }

    public async Task OverwriteAllAsync(Dictionary<string, List<string>> dict)
    {
        var json = JsonSerializer.Serialize(dict);
        await File.WriteAllTextAsync(_filePath, json);
    }

    public KeyValuePair<string, List<string>>? GetOne(string key)
    {
        var dict = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(File.ReadAllText(_filePath));
        if (dict is null)
            return null;
        
        dict.TryGetValue(key, out var values);
        if (values is null)
            return null;
        
        return new KeyValuePair<string, List<string>>(key, values);
    }

    public Dictionary<string, List<string>>? GetMany(List<string> keys)
    {
        var dict = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(File.ReadAllText(_filePath));
        if (dict is null)
            return null;

        var returnDict = new Dictionary<string, List<string>>();
        foreach (var key in keys)
        {
            dict.TryGetValue(key, out var values);
            if (values is null)
                continue;
            
            returnDict.Add(key, values);
        }

        return returnDict.Count > 0 ? returnDict : null;
    }

    public Dictionary<string, List<string>>? GetAll()
    {
        var dict = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(File.ReadAllText(_filePath));
        if (dict is null)
            return null;
        return dict;
    }

    public async Task<KeyValuePair<string, List<string>>?> GetOneAsync(string key)
    {
        var dict = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(await File.ReadAllTextAsync(_filePath));
        if (dict is null)
            return null;
        
        dict.TryGetValue(key, out var values);
        if (values is null)
            return null;
        
        return new KeyValuePair<string, List<string>>(key, values);
    }

    public async Task<Dictionary<string, List<string>>?> GetManyAsync(List<string> keys)
    {
        var dict = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(await File.ReadAllTextAsync(_filePath));
        if (dict is null)
            return null;

        var returnDict = new Dictionary<string, List<string>>();
        foreach (var key in keys)
        {
            dict.TryGetValue(key, out var values);
            if (values is null)
                continue;
            
            returnDict.Add(key, values);
        }

        return returnDict.Count > 0 ? returnDict : null;
    }

    public async Task<Dictionary<string, List<string>>?> GetAllAsync()
    {
        var dict = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(await File.ReadAllTextAsync(_filePath));
        if (dict is null)
            return null;
        return dict;
    }

    public void DeleteOne(string key)
    {
        var dict = GetAll();
        if (dict is null)
            return;
        dict.Remove(key);
        OverwriteAll(dict);
    }

    public void DeleteMany(List<string> keys)
    {
        var dict = GetAll();
        if (dict is null)
            return;
        foreach (var key in keys)
            dict.Remove(key);
        OverwriteAll(dict);
    }

    public void DeleteAll()
    {
        File.WriteAllText(_filePath, string.Empty);
    }

    public async Task DeleteOneAsync(string key)
    {
        var dict = await GetAllAsync();
        if (dict is null)
            return;
        dict.Remove(key);
        await OverwriteAllAsync(dict);
    }

    public async Task DeleteManyAsync(List<string> keys)
    {
        var dict = await GetAllAsync();
        if (dict is null)
            return;
        foreach (var key in keys)
            dict.Remove(key);
        await OverwriteAllAsync(dict);
    }

    public async Task DeleteAllAsync()
    {
        await File.WriteAllTextAsync(_filePath, string.Empty);
    }
}