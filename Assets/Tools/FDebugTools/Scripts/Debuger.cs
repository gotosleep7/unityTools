using UnityEngine;
namespace FDebugTools
{
    public class LocalOrNetLogHandler : ILogHandler
    {
        private static LocalOrNetLogHandler _instance;
        public static LocalOrNetLogHandler Instance
        {
            get
            {
                if (_instance == null) _instance = new LocalOrNetLogHandler();
                return _instance;
            }
        }
        public bool Log2Local { get; set; }
        private ILogHandler m_DefaultLogHandler = Debug.unityLogger.logHandler;

        public void LogException(System.Exception exception, Object context)
        {
            if (Log2Local) m_DefaultLogHandler.LogException(exception, context);
            HttpLogLogic.Instance.SendLog($"{exception.Message}\r\n{exception.StackTrace}");
        }

        public void LogFormat(LogType logType, Object context, string format, params object[] args)
        {
            string v = System.String.Format(format, args);
            HttpLogLogic.Instance.SendLog(v);
            if (Log2Local) m_DefaultLogHandler.LogFormat(logType, context, format, args);
        }
    }

    public static class Debuger
    {
        public static bool logEnable;
        public static bool netLogEnable;
        public static void Log(object message)
        {
            Log(null, message, null);
        }
        public static void LogError(object message)
        {
            LogError(null, message, null);
        }

        public static void LogWarning(object message)
        {
            LogWarning(null, message, null);
        }

        public static void Log(string tag, object message, Object context)
        {
            if (logEnable) Debug.unityLogger.Log(LogType.Log, tag, message, context);
        }

        public static void LogWarning(string tag, object message, Object context)
        {
            Debug.unityLogger.Log(LogType.Warning, tag, message, context);
        }

        public static void LogError(string tag, object message, Object context)
        {
            Debug.unityLogger.Log(LogType.Error, tag, message, context);
        }

        public static void NetLog(string tag, object message, Object context)
        {
            HttpLogLogic.Instance.SendLog(message.ToString());
        }
    }
}