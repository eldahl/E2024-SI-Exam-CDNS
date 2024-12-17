namespace DiscoveryService;

public interface IDiscoveryPersistence
{
    void SaveOne(KeyValuePair<string, string> endpoint);
    void SaveMany(Dictionary<string, string> endpoints);
    void OverwriteAll(Dictionary<string, string> endpoints);
    Task SaveOneAsync(KeyValuePair<string, string> endpoint);
    Task SaveManyAsync(Dictionary<string, string> endpoints);
    Task OverwriteAllAsync(Dictionary<string, string> endpoints);
    
    KeyValuePair<string, string>? GetOne(string endpointIdentifier);
    Dictionary<string, string>? GetMany(List<string> endpointIdentifiers);
    Dictionary<string, string>? GetAll();
    Task<KeyValuePair<string, string>?> GetOneAsync(string endpointIdentifier);
    Task<Dictionary<string, string>?> GetManyAsync(List<string> endpointIdentifiers);
    Task<Dictionary<string, string>?> GetAllAsync();
    
    void DeleteOne(string endpointIdentifier);
    void DeleteMany(List<string> endpointIdentifiers);
    void DeleteAll();
    Task DeleteOneAsync(string endpointIdentifier);
    Task DeleteManyAsync(List<string> endpointIdentifiers);
    Task DeleteAllAsync();
}