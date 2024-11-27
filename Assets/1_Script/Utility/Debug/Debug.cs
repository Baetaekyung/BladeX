using System;
using UnityEngine;

/// <summary>
/// At build remove symbol
/// </summary>
namespace Utility
{
    public static class Debug
    {
        [System.Diagnostics.Conditional("ENABLE_LOG")]
        public static void Log(object message)
        {
            UnityEngine.Debug.Log(message);
        }
    
        /// <param name="context">if log happen, you click log context object in hierarchy is focus</param>
        [System.Diagnostics.Conditional("ENABLE_LOG")]
        public static void Log(object message, UnityEngine.Object context)
        {
            UnityEngine.Debug.Log(message, context);
        }
            
        [System.Diagnostics.Conditional("ENABLE_LOG")]
        public static void LogWarning(object message)
        {
            UnityEngine.Debug.LogWarning(message);
        }
    }
}
