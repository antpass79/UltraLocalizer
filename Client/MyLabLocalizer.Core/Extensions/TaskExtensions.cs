using System.Threading.Tasks;

namespace MyLabLocalizer.Core.Extensions
{
    public static class TaskExtensions
    {
        public static T RunSync<T>(this Task<T> task)
        {
            task.RunSynchronously();
            return task.Result;
        }
    }
}
