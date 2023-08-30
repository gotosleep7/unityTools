using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Net.Http;
using System.Text;
using UnityEngine.UI;
using System.Text.RegularExpressions;
namespace FantasticLog
{
    public class MsgDat
    {
        public string data { get; set; }
    }

    public class Data
    {
        public string gameobject { get; set; }
        public string valueType { get; set; }
        public string fieldName { get; set; }
        public string value { get; set; }
        public ColorData color { get; set; }
    }
    public class ColorData
    {
        public float r;
        public float g;
        public float b;
        public float a;
    }

    public class PushMessageData
    {
        public string msgType { get; set; }
        // public Data data { get; set; }
        public string data { get; set; }
    }



    public class HttpLogLogic : MonoBehaviour
    {
        [SerializeField] private Button closeBtn;
        public static HttpLogLogic Instance;
        LogInfoPanelController logInfoPanelController;
        static HttpClient client = new HttpClient();
        Queue<string> capture = new Queue<string>();

        private void Awake()
        {
            Instance = this;
            logInfoPanelController = GetComponentInChildren<LogInfoPanelController>(true);
        }
        private void Update()
        {
            // if (logInfoPanelController.logEnable)
            // {
            //     Debug.unityLogger.logEnabled = true;
            // }
            // else
            // {
            //     Debug.unityLogger.logEnabled = false;
            // }

            if (capture.Count > 0)
            {
                string fileName = capture.Dequeue();
                DoRecordFrame(fileName);
            }
        }
        public bool IsUrl(string str)
        {
            string pattern = @"^(http|https|ftp)://[^\s/$.?#].[^\s]*(:\d{1,5})?(/[^\s]*)?$";
            Regex regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return regex.IsMatch(str);
        }
        public void SendLog(object message)
        {
            if (!LogInfoPanelController.isNetLogEnable) return;
            if (LogInfoPanelController.isWsEnable)
            {
                WsLogLogic.Instance.SendWsMessage($"x|0|{message}");

            }
            else
            {
                var formContent = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "data",message.ToString()}
            });
                client.PostAsync($"{logInfoPanelController.Url}/log", formContent);
            }
            // try
            // {
            //     // 创建HttpClient实例
            // if (!LogInfoPanelController.isNetLogEnable) return;
            //     if (!IsUrl(logInfoPanelController.Url)) return;

            //     var formContent = new FormUrlEncodedContent(new Dictionary<string, string>
            // {
            //     { "data",message.ToString()}
            // });
            //     client.PostAsync($"{logInfoPanelController.Url}/log", formContent);

            // }
            // catch (System.Exception e)
            // {
            //     Debuger.LogWarning(e);
            // }
        }
        public void SendLogErr(object message)
        {

            try
            {
                // 创建HttpClient实例
                if (!LogInfoPanelController.isNetLogEnable) return;
                if (!IsUrl(logInfoPanelController.Url)) return;

                var formContent = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "data",message.ToString()}
            });
                client.PostAsync($"{logInfoPanelController.Url}/logerr", formContent);

            }
            catch (System.Exception e)
            {
                Debuger.LogWarning(e);
            }
        }


        public void DoRecordFrame(string fileName = "image")
        {
            StartCoroutine(RecordFrame(fileName));
        }


        IEnumerator RecordFrame(string fileName = "image")
        {
            yield return new WaitForEndOfFrame();
            var screenshotTexture = ScreenCapture.CaptureScreenshotAsTexture();
            // 对纹理做一些事情
            byte[] screenshotData = screenshotTexture.EncodeToPNG();

            SendImage(screenshotData, fileName);
            // 清理
            UnityEngine.Object.Destroy(screenshotTexture);
        }

        public async void SendImage(byte[] imageBytes, string fileName)
        {
            // 创建MultipartFormDataContent对象
            using (MultipartFormDataContent formData = new MultipartFormDataContent())
            {
                // 创建ByteArrayContent对象，并将图片字节数组作为内容
                ByteArrayContent imageContent = new ByteArrayContent(imageBytes);
                long timestamp = System.DateTime.UtcNow.Ticks / System.TimeSpan.TicksPerMillisecond;

                // 添加图片内容到表单数据
                formData.Add(imageContent, "image", $"{timestamp}-{fileName}.png");

                // client.PostAsync($"{httpLogController.Url}/upload", formData);
                // 发送POST请求
                HttpResponseMessage response = await client.PostAsync($"{logInfoPanelController.Url}/upload", formData);

                // 检查响应是否成功
                if (response.IsSuccessStatusCode)
                {
                }
                else
                {
                    Debuger.LogWarning(response);
                }
            }

        }




        [ContextMenu("getImage")]
        public void CaptureScreen()
        {
            logInfoPanelController.gameObject.SetActive(false);
            closeBtn.gameObject.SetActive(false);
            capture.Enqueue("");
        }



    }
}
