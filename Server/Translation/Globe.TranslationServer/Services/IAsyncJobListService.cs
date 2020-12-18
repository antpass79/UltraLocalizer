using Globe.Shared.DTOs;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public interface IAsyncJobListService
    {
        Task SaveAsync(NewJobList newJobList);
    }
}
