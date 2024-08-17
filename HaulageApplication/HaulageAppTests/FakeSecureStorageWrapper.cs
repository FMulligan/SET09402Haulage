using HaulageApp.Common;

namespace HaulageAppTests;

public class FakeSecureStorageWrapper: ISecureStorageWrapper
{
    private readonly Dictionary < string, string > _secureStorage = new();
    
    public async Task<string?> GetAsync(string key)
    {
        return _secureStorage.GetValueOrDefault(key, null);
    }

    public async Task SetAsync(string key, string value)
    {
        _secureStorage[key] = value;
    }
}