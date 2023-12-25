namespace Application.Common.Interfaces;

public interface ICachedQuery<TResponse> : IQuery<TResponse>, ICachedQuery { }

public interface ICachedQuery
{
    public string Key { get; } // represent the key that is going to be save in the cached

    public TimeSpan? ExpirationTime { get;}
}
