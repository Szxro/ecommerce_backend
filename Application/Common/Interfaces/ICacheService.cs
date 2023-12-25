namespace Application.Common.Interfaces;

public interface ICacheService
{
    Task<T?> GetOrCreateAsync<T>(
        string key,
        Func<CancellationToken, Task<T>> request, 
        TimeSpan? expirationTime = null,    
        CancellationToken cancellationToken = default);

    void Remove(string key);
}
