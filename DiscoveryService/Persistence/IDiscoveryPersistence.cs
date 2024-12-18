namespace DiscoveryService.Persistence;

public interface IDiscoveryDictionaryPersistence<TKey, TValue> where TKey : notnull
{
    void SaveOne(KeyValuePair<TKey, TValue> kvp);
    void SaveMany(Dictionary<TKey, TValue> dict);
    void OverwriteAll(Dictionary<TKey, TValue> dict);
    Task SaveOneAsync(KeyValuePair<TKey, TValue> kvp);
    Task SaveManyAsync(Dictionary<TKey, TValue> dict);
    Task OverwriteAllAsync(Dictionary<TKey, TValue> dict);
    
    KeyValuePair<TKey, TValue>? GetOne(TKey key);
    Dictionary<TKey, TValue>? GetMany(List<TKey> keys);
    Dictionary<TKey, TValue>? GetAll();
    Task<KeyValuePair<TKey, TValue>?> GetOneAsync(TKey keys);
    Task<Dictionary<TKey, TValue>?> GetManyAsync(List<TKey> keys);
    Task<Dictionary<TKey, TValue>?> GetAllAsync();
    
    void DeleteOne(TKey key);
    void DeleteMany(List<TKey> keys);
    void DeleteAll();
    Task DeleteOneAsync(TKey key);
    Task DeleteManyAsync(List<TKey> keys);
    Task DeleteAllAsync();
}