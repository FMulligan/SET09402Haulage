namespace HaulageApp.Services;

public class PreferencesWrapper: IPreferencesWrapper
{
    public string Get<T>(string key, string defaultValue)
    {
        return Preferences.Default.Get(key, defaultValue);
    }

    public void Set<T>(string key, string value)
    {
        Preferences.Default.Set(key, value);
    }

    public bool ContainsKey(string key)
    {
        return Preferences.Default.ContainsKey(key);
    }

    public void Clear()
    {
        Preferences.Default.Clear();
    }
}