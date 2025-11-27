using System;
using System.Collections.Generic;
using System.Text;

namespace ItemStorageManager
{
    internal class Logger
    {
        #region Logging Methods

        public static void WriteLine(LogLevel level, string title, string message) { }

        public static void WriteLine(string title, string message) { }

        public static void WriteLine(string level, string title, string message) { }

        public static void WriteRaw(string title, string message) { }

        #endregion
    }

    /// <summary>
    /// Log level
    /// - None: No logging
    /// - Debug: Debug information (cyan)
    /// - Info: General information (green)
    /// - Attention: Important information that requires attention (yellow)
    /// - Warning: Warning information (magenta)
    /// - Error: Error information (red)
    /// </summary>
    public enum LogLevel
    {
        None = 0,
        Debug = 1,
        Info = 2,
        Attention = 3,
        Warning = 4,
        Error = 5,
    }
}
