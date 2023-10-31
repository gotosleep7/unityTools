using System;
using UnityEngine;
using UnityEngine.UI;
namespace FDebugTools
{
    public class LogInfoPanelController : MonoBehaviour
    {

        TMPro.TMP_InputField userText;
        TMPro.TMP_InputField addressText;
        TMPro.TMP_InputField portText;
        TMPro.TMP_InputField serverPortText;
        [SerializeField] RectTransform BtnParent;
        [SerializeField] RectTransform Parent;
        GameObject MiniPad;
        Button SetBtn;
        Toggle LocalLogBtn;
        Button CaptureBtn;
        public Toggle WsConnectBtn { get; set; }
        public Toggle NetLogBtn { get; set; }
        public Toggle SetHandlerBtn { get; set; }
        Toggle MiniPadBtn;
        Toggle SockerServerBtn;
        public string user;
        public string address;
        public int port;
        public int serverPort;
        public static bool logEnable;
        public static bool isWsEnable;

        public string Url => $"http://{address}:{port}";
        public string WsUrl => $"ws://{address}:{port}";
        public string User => user;
        // Start is called before the first frame update
        void Start()
        {
            Init();
        }
        private void Init()
        {
            userText = Parent.Find("user").GetComponent<TMPro.TMP_InputField>();
            addressText = Parent.Find("address").GetComponent<TMPro.TMP_InputField>();
            portText = Parent.Find("port").GetComponent<TMPro.TMP_InputField>();
            serverPortText = Parent.Find("serverPort").GetComponent<TMPro.TMP_InputField>();
            MiniPad = Parent.Find("MiniPad").gameObject;

            SetBtn = BtnParent.Find("SetBtn").GetComponent<Button>();
            CaptureBtn = BtnParent.Find("Capture").GetComponent<Button>();
            LocalLogBtn = BtnParent.Find("LocalLog").GetComponent<Toggle>();
            WsConnectBtn = BtnParent.Find("WsConnect").GetComponent<Toggle>();
            NetLogBtn = BtnParent.Find("NetLog").GetComponent<Toggle>();
            SetHandlerBtn = BtnParent.Find("CustomLogHandler").GetComponent<Toggle>();
            MiniPadBtn = BtnParent.Find("MiniPadBtn").GetComponent<Toggle>();
            SockerServerBtn = BtnParent.Find("SockerServerBtn").GetComponent<Toggle>();


            user = PlayerPrefs.GetString("userName");
            address = PlayerPrefs.GetString("address", "150.158.136.177");
            port = PlayerPrefs.GetInt("port", 8888);
            serverPort = PlayerPrefs.GetInt("serverPort", 9999);

            userText.text = user;
            addressText.text = address;
            portText.text = port.ToString();
            serverPortText.text = serverPort.ToString();

            SetBtn.onClick.AddListener(SetIpAndPort);
            CaptureBtn.onClick.AddListener(OnCapture);
            WsConnectBtn.onValueChanged.AddListener(OnWsConnect);
            LocalLogBtn.onValueChanged.AddListener(OnLocalLog);
            NetLogBtn.onValueChanged.AddListener(OnNetLog);
            SetHandlerBtn.onValueChanged.AddListener(OnSetHandleBtn);
            MiniPadBtn.onValueChanged.AddListener(OnMiniPadBtn);
            SockerServerBtn.onValueChanged.AddListener(OnSockerServerBtn);
        }

        private void OnSockerServerBtn(bool isOn)
        {
            if (isOn)
                SocketManager.Instance.OpenSocketServer(address, serverPort);
            else
                SocketManager.Instance.CloseSocketServer();
        }

        private void OnMiniPadBtn(bool isOn)
        {
            MiniPad.SetActive(isOn);
        }

        private void OnSetHandleBtn(bool isOn)
        {
            LogController.Instance.SetHandle(isOn);
        }

        private async void OnWsConnect(bool isOn)
        {
            if (isOn)
            {
                await WsLogLogic.Instance.OpenWebsocket(WsUrl, user);
            }
            else
            {
                WsLogLogic.Instance.CloseWebsocket();
            }
        }

        private void OnCapture()
        {
            HttpLogLogic.Instance.CaptureScreen();
        }
        public void Hide()
        {
            this.gameObject.SetActive(false);
        }

        private void SetIpAndPort()
        {
            if (!addressText.text.Equals("") && !portText.text.Equals("") && !userText.text.Equals(""))
            {
                user = userText.text;
                address = addressText.text;
                port = int.Parse(portText.text);
                PlayerPrefs.SetString("userName", user);
                PlayerPrefs.SetString("address", address);
                PlayerPrefs.SetInt("port", port);
            }
            else
            {
                Debuger.LogError("user,ip,port 都不能为空");
            }
        }

        private void OnLocalLog(bool isOn)
        {
            LocalOrNetLogHandler.Instance.Log2Local = isOn;
        }
        private void OnNetLog(bool isOn)
        {
            LocalOrNetLogHandler.Instance.Log2Net = isOn;
        }

        // private void OnDestroy()
        // {
        //     WsLogLogic.Instance.CloseWebsocket();
        // }

    }
}
