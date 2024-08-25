namespace HaulageApp.Common;

public interface IPreferencesWrapper
{
    string Get<T>(string key, string defaultValue);
    
    void Set<T>(string key, string value);
    
    bool ContainsKey(string key);
    
    void Clear();
}