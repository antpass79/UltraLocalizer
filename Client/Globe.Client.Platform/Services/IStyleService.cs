using System;
using System.Threading.Tasks;

namespace Globe.Client.Platform.Services
{

    public interface IStyleService
    {
        Task<string> Get(string stylePath);
    }
}
