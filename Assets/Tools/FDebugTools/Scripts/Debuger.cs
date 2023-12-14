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
        private string _tag;
        private string Tag
        {
            get
            {
                if (_tag == null)
                {
                    if (LogController.Instance.User == null) return null;

                    _tag = $"{LogController.Instance.User}-{SystemInfo.deviceName}-{Application.version}";
                }
                return _tag;
            }
        }
        private bool _log2Local;
        private bool _log2Net;
        public bool Log2Local
        {
            get
            {
                return _log2Local;
            }
            set
            {
                _log2Local = value;
                Debuger.localLogEnable = value;
            }
        }
        public bool Log2Net
        {
            get
            {
                return _log2Net;
            }
            set
            {
                _log2Net = value;
                Debuger.netLogEnable = value;
            }
        }
        private ILogHandler m_DefaultLogHandler = Debug.unityLogger.logHandler;

        public void LogException(System.Exception exception, Object context)
        {
            if (Log2Local) m_DefaultLogHandler.LogException(exception, context);
        }

        public void LogFormat(LogType logType, Object context, string format, params object[] args)
        {
            string v = System.String.Format(format, args);
            Debuger.NetLog(Tag, v, null);
            if (Log2Local) m_DefaultLogHandler.LogFormat(logType, context, format, args);
        }
    }

    public static class Debuger
    {
        public static bool localLogEnable;
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
            if (localLogEnable) Debug.unityLogger.Log(LogType.Log, tag, message, context);
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
            // if (netLogEnable) LogController.Instance.SendLog($"[{tag}]{message}\r\n{context}");
            if (netLogEnable) LogController.Instance.SendLog($"[{tag}]{message}\r\n");
        }
    }
}