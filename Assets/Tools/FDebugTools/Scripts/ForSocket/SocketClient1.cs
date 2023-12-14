
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
namespace FDebugTools
{
    public class SocketClient1 : MonoBehaviour
    {
        public static SocketClient1 Instance;
        public static bool ConnectFlag { get; set; }
        static TcpClient tcpClient;
        public UnityEvent<string> OnRecieve;
        Dictionary<string, Type> cachedTypes = new Dictionary<string, Type>();
        Dictionary<string, System.Reflection.MethodInfo> cachedMethods = new();
        Queue<string> messageQueue = new Queue<string>();
        private CancellationTokenSource cts;
        [SerializeField] private float heartDance = 30;
        [SerializeField] private float timer;

        public string host = "127.0.0.1";
        public int port = 8989;
        private void Awake()
        {
            Instance = this;

        }
        private void Start()
        {
            OpenWebsocket();
        }
        public void OpenWebsocket()
        {
            DoOpenWebsocket();
        }

        public async Task DoOpenWebsocket()
        {
            GetDeviceInfo();
            cts = new CancellationTokenSource();
            if (ConnectFlag) return;
            ConnectFlag = true;
            while (!cts.IsCancellationRequested)
            {

                using (tcpClient = new TcpClient())
                {
                    try
                    {
                        Debug.Log("open socket");
                        // 连接到WebSocket服务器

                        await tcpClient.ConnectAsync(this.host, this.port);
                        Debug.Log($"tcpClient.Connected=={tcpClient.Connected}");
                        // 接收消息的循环
                        while (tcpClient.Connected)
                        {
                            byte[] buffer = new byte[4096];

                            int resultCount = await tcpClient.GetStream().ReadAsync(new ArraySegment<byte>(buffer), cts.Token);
                            Debug.Log($"resultCount={resultCount}");
                            if (resultCount == 0) continue;
                            // string message = System.Text.Encoding.UTF8.GetString(buffer, 0, result.Count);
                            string message = System.Text.Encoding.UTF8.GetString(buffer, 0, resultCount);
                            HandleMessage(message);
                            // WsState.Instance.ConnectFlag = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogWarning("socket连接异常：" + ex.Message);
                    }
                    if (!ConnectFlag) break;
                    // 等待一段时间后进行重连
                    await Task.Delay(5000);
                }
            }
        }
        private void Update()
        {
            if (ConnectFlag)
            {
                timer -= Time.deltaTime;
                if (timer < 0)
                {
                    DoSendWsMessage("ping");
                    timer = heartDance;

                }
            }
        }

        private void GetDeviceInfo()
        {
            var deviceInfo = new StringBuilder()
                            .Append("\r\n").Append("设备名称").Append(SystemInfo.deviceName)
                            .Append("\r\n").Append("操作系统名称和版本号").Append(SystemInfo.operatingSystem)
                            .Append("\r\n").Append("CPU类型").Append(SystemInfo.processorType)
                            .Append("\r\n").Append("CPU核心数").Append(SystemInfo.processorCount)
                            .Append("\r\n").Append("系统内存大小").Append(SystemInfo.systemMemorySize)
                            .Append("\r\n").Append("GPU名称").Append(SystemInfo.graphicsDeviceName)
                            .Append("\r\n").Append("GPU内存大小").Append(SystemInfo.graphicsMemorySize)
                            .Append("\r\n").Append("GPU类型").Append(SystemInfo.graphicsDeviceType)
                            .Append("\r\n").Append("GPU供应商").Append(SystemInfo.graphicsDeviceVendor)
                            .Append("\r\n").Append("GPU驱动版本号").Append(SystemInfo.graphicsDeviceVersion)
                            .Append("\r\n").Append("支持的最大纹理尺寸").Append(SystemInfo.maxTextureSize)
                            .Append("\r\n").Append("是否支持GPU实例化").Append(SystemInfo.supportsInstancing);
            Debug.Log(deviceInfo);
        }
        private void LateUpdate()
        {
            // Debug.Log($"messageQueue.Count={messageQueue.Count}");
            int count = messageQueue.Count;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    if (tcpClient != null) DoSendWsMessage(messageQueue.Dequeue());
                }
            }

        }

        private async void DoSendWsMessage(string message)
        {
            if ("".Equals(message)) return;
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            var buffer = new ArraySegment<byte>(bytes);
            try
            {
                // 创建发送的数据缓冲区
                if (tcpClient != null && tcpClient.Connected)
                {
                    await tcpClient?.GetStream().WriteAsync(bytes, 0, bytes.Length, cts.Token);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"message={message},count={message.Length}");
                Debug.LogError($"bytes={bytes},count={bytes.Length}");
                throw e;

            }
        }
        [ContextMenu("HandleMessage")]
        public void HandleMessage(string data)
        {
            string[] messages = data.Split("\r\n");
            Debug.Log($"socketMsg={data}");
            foreach (var message in messages)
            {
                if ("".Equals(message)) continue;
                if ("pong".Equals(message)) continue;
                try
                {
                    SyncDataModel syncDataModel = new SyncDataModel(message);
                    switch (syncDataModel.msgType)
                    {
                        case 1:
                            if (SyncDataModelForMaterial.TryConvert(syncDataModel.contentStr, out SyncDataModelForMaterial syncDataModelForMaterial))
                                HandleMaterial(syncDataModelForMaterial);
                            break;
                        case 2:
                            if (SyncDataModelForRpc.TryConvert(syncDataModel.contentStr, out SyncDataModelForRpc content))
                                HandleComponent(content);
                            break;
                        default:
                            break;
                    }
                }
                catch (SyncDataConvertException e)
                {
                    Debug.LogWarning(e);
                    Debug.Log(message);
                    // 消息是未知类型，直接转发出去
                    OnRecieve?.Invoke(message);
                    continue;
                }

            }

        }


        private void HandleComponent(SyncDataModelForRpc syncDataModel)
        {
            foreach (var content in syncDataModel.dataContents)
            {
                if (!cachedTypes.TryGetValue(content.classFullName, out var classType))
                {
                    classType = Type.GetType(content.classFullName);
                    cachedTypes[content.classFullName] = classType;
                }
                string key = $"{content.methodName}-{content.parmaTypes[0]}-{content.parmaTypes[content.parmaTypes.Length - 1]}";
                if (!cachedMethods.TryGetValue(key, out var methodInfo))
                {
                    methodInfo = classType.GetMethod(content.methodName, content.parmaTypes);
                    if (methodInfo != null) cachedMethods[key] = methodInfo;
                }
                if (!cachedGameObjects.TryGetValue(content.sourcePath, out GameObject target))
                {
                    target = GetTarget(content.path);
                    cachedGameObjects[content.sourcePath] = target;
                }
                Component targetComponent = target.GetComponent(classType);
                methodInfo.Invoke(targetComponent, content.parmas);
            }
        }

        Dictionary<string, Material> cachedMaterials = new Dictionary<string, Material>();
        Dictionary<string, GameObject> cachedGameObjects = new Dictionary<string, GameObject>();
        public void HandleMaterial(SyncDataModelForMaterial syncDataModel)
        {
            foreach (var content in syncDataModel.dataContents)
            {
                if (!cachedGameObjects.TryGetValue(content.sourcePath, out GameObject target))
                {
                    target = GetTarget(content.path);
                    if (!target.activeInHierarchy) return;
                    cachedGameObjects[content.sourcePath] = target;
                }
                if (!cachedMaterials.TryGetValue(content.sourcePath, out Material material))
                {
                    if (target.TryGetComponent(out MeshRenderer mr))
                    {
                        Material m = new Material(mr.material);
                        cachedMaterials[content.sourcePath] = m;
                        mr.material = m;
                        material = m;
                    }

                }
                foreach (var param in content.paramArr)
                {

                    switch (param.valueType)
                    {
                        case MaterialContentParamType.F:

                            material?.SetFloat(param.fieldName, float.Parse(param.value));
                            break;
                        case MaterialContentParamType.C:
                            string[] rgbaColor = param.value.Split(',');
                            material?.SetColor(param.fieldName, new Color(float.Parse(rgbaColor[0]), float.Parse(rgbaColor[1]), float.Parse(rgbaColor[2]), float.Parse(rgbaColor[3])));
                            break;
                    }
                }

            }
        }

        private GameObject GetTarget(string[] path)
        {
            Transform root = GameObject.Find(path[0]).transform;
            for (int i = 1; i < path.Length; i++)
            {
                root = root.transform.Find(path[i]);
            }
            return root.gameObject;
        }


        public void SendWsMessage(string message)
        {
            messageQueue.Enqueue(message);

        }

        public void CloseWebsocket()
        {
            try
            {
                LogController.Instance.SendLog("close ws");
                ConnectFlag = false;
                tcpClient?.Close();
            }
            catch (System.Exception e)
            {
                Debuger.LogWarning(e);
            }
        }
        private void OnDestroy()
        {
            CloseWebsocket();
            cts.Cancel();
        }
    }
}

