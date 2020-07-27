namespace Globe.BusinessLogic.Repositories
{
    public interface IRepository<T, in K> : IReadRepository<T, K>, IWriteRepository<T>
        where T : class
    {
    }

    public interface IRepository<T> : IReadRepository<T>, IWriteRepository<T>
        where T : class
    {
    }

    public interface IAsyncRepository<T, in K> : IAsyncReadRepository<T, K>, IAsyncWriteRepository<T>
        where T : class
    {
    }

    public interface IAsyncRepository<T> : IAsyncReadRepository<T>, IAsyncWriteRepository<T>
        where T : class
    {
    }
}
