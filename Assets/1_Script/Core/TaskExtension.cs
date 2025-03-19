using System.Threading.Tasks;
using UnityEngine;

namespace Swift_Blade
{
    public static class TaskExtension
    {
        public static void Forget(this Awaitable @this)
        {
        }
        public static void Forget(this Task @this)
        {
        }
        public static void Forget(this ValueTask @this)
        {
        }
    }
}
