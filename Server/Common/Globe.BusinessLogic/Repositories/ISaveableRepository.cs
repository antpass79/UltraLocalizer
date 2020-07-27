namespace Globe.BusinessLogic.Repositories
{
    public interface ISaveableRepository<T> : IRepository<T>, ISaveable
        where T : class
    {
    }

    public interface IAsyncSaveableRepository<T> : IAsyncRepository<T>, IAsyncSaveable
        where T : class
    {
    }
}
