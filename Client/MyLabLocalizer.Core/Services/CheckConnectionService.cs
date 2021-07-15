using System.Runtime.InteropServices;

namespace MyLabLocalizer.Core.Services
{
    public class CheckConnectionService : ICheckConnectionService
    {
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int description, int reservedValue);

        public bool IsConnectionAvailable()
        {
            int description;
            return InternetGetConnectedState(out description, 0);
        }
    }
}
