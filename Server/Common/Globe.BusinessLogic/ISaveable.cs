using System.Threading.Tasks;

namespace Globe.BusinessLogic
{
    public interface ISaveable
    {
        void Save();
    }

    public interface IAsyncSaveable
    {
        Task SaveAsync();
    }
}
