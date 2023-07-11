using System;
using UnityEngine;
using UnityEngine.UI;
namespace FantasticLog
{
    public class LogInfoPanelController : MonoBehaviour
    {

        TMPro.TMP_InputField userText;
        TMPro.TMP_InputField addressText;
        TMPro.TMP_InputField portText;
        [SerializeField] RectTransform BtnParent;
        [SerializeField] RectTransform Parent;
        Button SetBtn;
        Toggle LocalLogBtn;
        Button CaptureBtn;
        Toggle WsConnectBtn;
        Toggle NetLogBtn;
        Toggle SetHandlerBtn;
        public string user;
        public string address;
        public int port;
        public bool logEnable;
        public bool isNetLogEnable;

        public string Url => $"http://{address}:{port}";
        public string WsUrl => $"ws://{address}:{port}";
        public string User => user;

        ILogHandler sourceLogHandler;
        // Start is called before the first frame update
        void Start()
        {
            userText = Parent.Find("name").GetComponent<TMPro.TMP_InputField>();
            addressText = Parent.Find("address").GetComponent<TMPro.TMP_InputField>();
            portText = Parent.Find("port").GetComponent<TMPro.TMP_InputField>();

            SetBtn = BtnParent.Find("SetBtn").GetComponent<Button>();
            CaptureBtn = BtnParent.Find("Capture").GetComponent<Button>();
            LocalLogBtn = BtnParent.Find("LocalLog").GetComponent<Toggle>();
            WsConnectBtn = BtnParent.Find("WsConnect").GetComponent<Toggle>();
            NetLogBtn = BtnParent.Find("NetLog").GetComponent<Toggle>();
            SetHandlerBtn = BtnParent.Find("CustomLogHandler").GetComponent<Toggle>();



            user = PlayerPrefs.GetString("user");
            address = PlayerPrefs.GetString("address");
            port = PlayerPrefs.GetInt("port");

            userText.text = user;
            addressText.text = address;
            portText.text = port.ToString();

            SetBtn.onClick.AddListener(SetIpAndPort);
            CaptureBtn.onClick.AddListener(OnCapture);
            WsConnectBtn.onValueChanged.AddListener(OnWsConnect);
            LocalLogBtn.onValueChanged.AddListener(OnLocalLog);
            NetLogBtn.onValueChanged.AddListener(OnNetLog);
            SetHandlerBtn.onValueChanged.AddListener(OnSetHandleBtn);

            sourceLogHandler = Debug.unityLogger.logHandler;
        }

        private void OnSetHandleBtn(bool isOn)
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

        private async void OnWsConnect(bool isOn)
        {
            if (isOn)
            {
                await WsLogLogic.Instance.OpenWebsocket();
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
                PlayerPrefs.SetString("user", user);
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
            if (isOn)
            {
                logEnable = true;
                LocalOrNetLogHandler.Instance.Log2Local = true;
            }
            else
            {
                logEnable = false;
                LocalOrNetLogHandler.Instance.Log2Local = false;
            }
        }
        private void OnNetLog(bool isOn)
        {
            isNetLogEnable = isOn;
        }




        private void OnDestroy()
        {
            WsLogLogic.Instance.CloseWebsocket();
        }

    }
}
