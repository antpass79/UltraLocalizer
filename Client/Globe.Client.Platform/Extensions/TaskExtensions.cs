using System.Threading.Tasks;

namespace Globe.Client.Platform.Extensions
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
