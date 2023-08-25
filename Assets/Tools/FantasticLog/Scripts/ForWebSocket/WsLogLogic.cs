
using System;
using System.Net.WebSockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace FantasticLog
{
    public class WsLogLogic : MonoBehaviour
    {
        public static WsLogLogic Instance;

        bool connectFlag;
        ClientWebSocket wsClient;
        LogInfoPanelController logInfoController;
        public UnityEvent<string> OnRecieve;
        private void Awake()
        {
            Instance = this;
            logInfoController = GetComponentInChildren<LogInfoPanelController>(true);
        }
        private void Start()
        {
        }
        public async Task OpenWebsocket()
        {
            if (connectFlag) return;
            while (true)
            {
                using (wsClient = new ClientWebSocket())
                {
                    try
                    {
                        // 连接到WebSocket服务器
                        await wsClient.ConnectAsync(new Uri($"{logInfoController.WsUrl}/ping/{logInfoController.User}"), CancellationToken.None);
                        // 接收消息的循环
                        while (wsClient.State == WebSocketState.Open)
                        {
                            connectFlag = true;
                            byte[] buffer = new byte[1024];
                            WebSocketReceiveResult result = await wsClient.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                            if (result.MessageType == WebSocketMessageType.Text)
                            {
                                string message = System.Text.Encoding.UTF8.GetString(buffer, 0, result.Count);

                                HandleMessage(message);
                            }
                        }
                    }
                    catch (WebSocketException ex)
                    {
                        Debug.LogWarning("WebSocket连接异常：" + ex.Message);
                    }
                    if (!connectFlag) break;
                    // 等待一段时间后进行重连
                    await Task.Delay(5000);
                }
            }
        }
        [ContextMenu("HandleMessage")]
        public void HandleMessage(string message)
        {
            try
            {
                OnRecieve?.Invoke(message);
                Debug.Log($"message={message}");
                string[] strings = message.Split("|");
                if (strings.Length < 2) return;
                //备用
                string msgType = strings[1];
                switch (msgType)
                {
                    case "1":
                        HandleMaterial(strings);
                        break;
                    case "2":
                        HandleComponent(strings);
                        break;
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e.Message);
            }
        }

        private void HandleComponent(string[] dataMsg)
        {
            //  string a = "userid|1|xxx,xx,xx|class.fullname|methodname|parpm1,parpm1,parpm1,parpm1";
            string[] path = dataMsg[2].Split(",");
            string classFunllName = dataMsg[3];
            string methodName = dataMsg[4];
            object[] parames = dataMsg[5]?.Split(",");
            Type classType = Type.GetType(classFunllName);
            System.Reflection.MethodInfo methodInfo = classType.GetMethod(methodName);
            GameObject target = GetTarget(path);
            Component component = target.GetComponent(classType);
            methodInfo.Invoke(component, parames);

        }

        public GameObject GetTarget(string[] path)
        {
            Transform root = GameObject.Find(path[0]).transform;
            for (int i = 1; i < path.Length; i++)
            {
                root = root.transform.Find(path[i]);
            }
            return root.gameObject;
        }


        public void HandleMaterial(string[] dataMsg)
        {
            //备用
            string msgType = dataMsg[1];
            string[] path = dataMsg[2].Split(",");
            int valueType = int.Parse(dataMsg[3]);
            string fieldName = dataMsg[4];
            string value = dataMsg[5];
            string[] rgbaColor = dataMsg[6].Split(",");

            GameObject target;
            Transform root = GameObject.Find(path[0]).transform;
            for (int i = 1; i < path.Length; i++)
            {
                root = root.transform.Find(path[i]);
            }
            target = root.gameObject;
            MeshRenderer mr = target.GetComponent<MeshRenderer>();
            switch (valueType)
            {
                case 0:
                    mr.material.SetFloat(fieldName, float.Parse(value));
                    break;
                case 1:
                    mr.sharedMaterial.SetColor(fieldName, new Color(float.Parse(rgbaColor[0]), float.Parse(rgbaColor[1]), float.Parse(rgbaColor[2]), float.Parse(rgbaColor[3])));
                    break;
            }
        }

        public async void SendWsMessage(string message)
        {
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(message);

                // 创建发送的数据缓冲区
                var buffer = new ArraySegment<byte>(bytes);
                await wsClient?.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);

            }
            catch (System.Exception e)
            {
                Debug.LogError(e);

            }
        }

        public async void CloseWebsocket()
        {
            try
            {

                connectFlag = false;
                await wsClient?.CloseAsync(WebSocketCloseStatus.NormalClosure, "关闭连接", CancellationToken.None);
            }
            catch (System.Exception e)
            {
                Debuger.LogWarning(e);
            }
        }
        private void OnDestroy()
        {
            CloseWebsocket();
        }
    }
}