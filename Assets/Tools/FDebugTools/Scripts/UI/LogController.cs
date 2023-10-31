using System;
using System.Buffers.Text;
using System.Text;
using FDebugTools;
using TMPro;
using Unity.Core;
using Unity.Entities.UniversalDelegates;
using UnityEngine;
using UnityEngine.UI;
namespace FDebugTools
{
    public class LogController : MonoBehaviour
    {
        public static LogController Instance;
        public TMP_Text id;
        ILogHandler sourceLogHandler;
        WsLogLogic wsLogLogic;
        string ip = "150.158.136.177";
        // string ip = "127.0.0.1:8888";
        string logUri = "dt/log";
        public string User;
        private void Awake()
        {
            Instance = this;
            sourceLogHandler = Debug.unityLogger.logHandler;
            wsLogLogic = GetComponentInChildren<WsLogLogic>();
            User = PlayerPrefs.GetString("userName");
            if ("".Equals(User))
            {
                User = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
                PlayerPrefs.SetString("userName", User);
            }
            id.text = $"{User}-{Application.version}";
        }
        private async void Start()
        {
            // FDTMessage fDTMessage = new FDTMessage();
            SetHandle(true);
            LocalOrNetLogHandler.Instance.Log2Local = true;
            LocalOrNetLogHandler.Instance.Log2Net = true;
            await wsLogLogic.OpenWebsocket("ws://" + ip, User);
        }

        public void SetHandle(bool isOn)
        {
            if (isOn)
            {

                Debug.unityLogger.logHandler = LocalOrNetLogHandler.Instance;
                Debug.unityLogger.logEnabled = true;
            }
            else
            {
                Debug.unityLogger.logHandler = sourceLogHandler;
            }
        }

        public void SendLog(object message)
        {
            HttpLogLogic.Instance.Post($"http://{ip}/{logUri}", $"{message}");
        }
    }
}
