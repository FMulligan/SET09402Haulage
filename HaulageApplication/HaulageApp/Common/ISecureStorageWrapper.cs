namespace HaulageApp.Common;

public interface ISecureStorageWrapper
{
    Task<string?> GetAsync(string key);

    Task SetAsync(string key, string value);
}