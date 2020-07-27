using System.Threading.Tasks;

namespace Globe.Identity.Security
{
    public interface IJwtTokenEncoder<T>
    {
        Task<string> EncodeAsync(T input);
    }
}
