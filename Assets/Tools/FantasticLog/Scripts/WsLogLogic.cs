
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
namespace FantasticLog
{
    public class WsLogLogic : MonoBehaviour
    {
        public static WsLogLogic Instance;

        bool connectFlag;
        ClientWebSocket wsClient;
        LogInfoPanelController logInfoController;
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
                        Debuger.LogWarning("WebSocket连接异常：" + ex.Message);
                    }
                    if (!connectFlag) break;
                    // 等待一段时间后进行重连
                    await Task.Delay(5000);
                }
            }
        }

        public void HandleMessage(string message)
        {
            try
            {
                Debuger.Log($"message={message}");
                string[] strings = message.Split("|");
                if (strings.Length != 7) return;
                //备用
                string msgType = strings[1];
                switch (msgType)
                {
                    case "1":
                        HandleMaterial(strings);
                        break;
                    case "2":
                        break;
                }
            }
            catch (System.Exception e)
            {
                Debuger.LogWarning(e.Message);
            }
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