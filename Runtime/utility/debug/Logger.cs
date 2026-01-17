using System;

namespace TwoSides.Utility.Debug.Logging
{
    /// <summary>
    /// Lightweight logger that can be enabled or disabled globally.
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// Gets or sets whether logging is enabled.
        /// </summary>
        public static bool Enabled { get; private set; } = true;

        /// <summary>
        /// Enables or disables logging.
        /// </summary>
        /// <param name="enabled">Whether logging should be enabled.</param>
        public static void SetEnabled(bool enabled)
        {
            Enabled = enabled;
        }

        /// <summary>
        /// Writes a standard log message.
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="context">Optional Unity context object.</param>
        public static void Log(object message, UnityEngine.Object context = null)
        {
            if (!Enabled) return;

            if (context == null)
                UnityEngine.Debug.Log(message);
            else
                UnityEngine.Debug.Log(message, context);
        }

        /// <summary>
        /// Writes a warning log message.
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="context">Optional Unity context object.</param>
        public static void LogWarning(object message, UnityEngine.Object context = null)
        {
            if (!Enabled) return;

            if (context == null)
                UnityEngine.Debug.LogWarning(message);
            else
                UnityEngine.Debug.LogWarning(message, context);
        }

        /// <summary>
        /// Writes an error log message.
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="context">Optional Unity context object.</param>
        public static void LogError(object message, UnityEngine.Object context = null)
        {
            if (!Enabled) return;

            if (context == null)
                UnityEngine.Debug.LogError(message);
            else
                UnityEngine.Debug.LogError(message, context);
        }

        /// <summary>
        /// Writes an exception log message.
        /// </summary>
        /// <param name="exception">Exception to log.</param>
        /// <param name="context">Optional Unity context object.</param>
        public static void LogException(Exception exception, UnityEngine.Object context = null)
        {
            if (!Enabled) return;

            if (context == null)
                UnityEngine.Debug.LogException(exception);
            else
                UnityEngine.Debug.LogException(exception, context);
        }
    }
}