using System;
using System.Runtime.InteropServices;
using System.Security;

namespace MyLabLocalizer.Core.Extensions
{
    public static class StringExtensions
    {
        public static string ToPlainString(this SecureString secureString)
        {
            if (secureString == null)
                return string.Empty;

            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }
    }
}
