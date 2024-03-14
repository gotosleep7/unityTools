using System;
using TMPro;
using UnityEngine;
namespace FDebugTools
{
    enum LogMode
    {
        ServerToClient, ClientToServer, Http
    }
    public class LogController : MonoBehaviour
    {
        [SerializeField] LogMode logMode;
        public static LogController Instance;
        public TMP_Text id;
        ILogHandler sourceLogHandler;
        WsLogLogic wsLogLogic;
        string ip = "150.158.136.177";
        // string ip = "127.0.0.1:8888";
        string logUri = "dt/log";
        public string User;
        private float testTimer;
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
            if (id != null)
                id.text = $"{User}-{Application.version}";
        }
        private void Start()
        {
            // FDTMessage fDTMessage = new FDTMessage();
            SetHandle(true);
            LocalOrNetLogHandler.Instance.Log2Local = true;
            LocalOrNetLogHandler.Instance.Log2Net = true;
            // await wsLogLogic.OpenWebsocket("ws://" + ip, User);
        }
        private void Update()
        {
            testTimer += Time.deltaTime;
            if (testTimer > 2f)
            {
                testTimer = 0f;
                Debug.Log("Client debug message Client debug message Client debug message Client debug message Client debug message Client debug message Client debug message Client debug message Client debug message ");
            }
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
            switch (logMode)
            {
                case LogMode.ServerToClient:
                    SendLogForServerToClient(message);
                    break;
                case LogMode.ClientToServer:
                    SendLogForClientToServer(message);
                    break;
                case LogMode.Http:
                    SendLogForHttp(message);
                    break;
            }

        }
        public void SendLogForClientToServer(object message)
        {
            SocketClient.Instance?.SendMessageToServer(message.ToString());
        }

        public void SendLogForServerToClient(object message)
        {
            SocketManager.Instance?.PushMessageToClient(message.ToString());
        }
        public void SendLogForHttp(object message)
        {
            HttpLogLogic.Instance?.Post($"http://{ip}/{logUri}", $"{message}");
        }


    }
}
