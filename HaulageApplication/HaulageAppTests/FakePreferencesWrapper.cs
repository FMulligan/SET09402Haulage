using HaulageApp.Common;

namespace HaulageAppTests;

public class FakePreferencesWrapper: IPreferencesWrapper
{
    private readonly Dictionary < string, string > _preferences = new();

    public string Get<T>(string key, string defaultValue)
    {
        return _preferences.GetValueOrDefault(key, defaultValue);
    }

    public void Set<T>(string key, string value)
    {
        _preferences[key] = value;
    }

    public bool ContainsKey(string key)
    {
        return _preferences.ContainsKey(key);
    }

    public void Clear()
    {
        _preferences.Clear();
    }
}