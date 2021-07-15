using System;
using System.Threading.Tasks;

namespace MyLabLocalizer.Core.Services
{
    public interface IStyleService
    {
        Task<string> Get(string stylePath);
    }
}
