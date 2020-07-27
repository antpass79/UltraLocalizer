namespace Globe.BusinessLogic.Repositories
{
    public interface ISaveableWriteRepository<T> : IWriteRepository<T>, ISaveable
        where T : class
    {
    }

    public interface IAsyncSaveableWriteRepository<T> : IAsyncWriteRepository<T>, IAsyncSaveable
        where T : class
    {
    }
}
