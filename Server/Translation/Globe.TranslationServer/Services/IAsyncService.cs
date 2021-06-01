namespace Globe.TranslationServer.Services
{
    public interface IAsyncService<T, TKey> : IAsyncReadService<T, TKey>, IAsyncWriteService<T>
    {
    }

    public interface IAsyncService<T> : IAsyncReadService<T, int>, IAsyncWriteService<T>
    {
    }

    public interface IAsyncService<TRead, TWrite, TKey> : IAsyncReadService<TRead, TKey>, IAsyncWriteService<TWrite>
    {
    }
}
