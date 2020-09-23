using Globe.TranslationServer.DTOs;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public interface IAsyncJobListService
    {
        Task SaveAsync(SavableJobListDTO savableJobList);
    }
}
