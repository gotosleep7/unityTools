using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class HttpAPI
{
    // string domain = "http://localhost:8080/";
    string domain = "https://flask-app-digital-human-icwoxazodg.cn-shanghai.fcapp.run/api/v1";
    private static HttpAPI _instance = null;

    private HttpAPI() { }
    public static HttpAPI Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new HttpAPI();
            }
            return _instance;
        }
    }

    public async void Login(string email, string password)
    {
        string url = $"{domain}/user/login";
        UserDataModel user = new UserDataModel();
        user.email = email;
        user.password = password;
        List<KeyValuePair<string, string>> head = new List<KeyValuePair<string, string>>();
        head.Add(new KeyValuePair<string, string>("Content-Type", "application/json"));
        await HttpClientManager.Instance.PostAsync(url, JsonConvert.SerializeObject(user), onSuccess: (data) =>
      {
          Dictionary<string, string> ret = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);
          if (ret.TryGetValue("token", out string token))
          {
              HttpClientManager.Instance.Token = token;
              Debug.Log("login success");
          }
      }, onFailure: (err) =>
      {
          Debug.LogError(err);
      }, head);
    }
    public async void QueryAll(Action<string> onSuccess)
    {
        string url = $"{domain}/task/queryAll";
        List<KeyValuePair<string, string>> head = new List<KeyValuePair<string, string>>();
        await HttpClientManager.Instance.GetAsync(url, onSuccess: (data) =>
        {
            Debug.Log("QueryAll" + data);
            onSuccess?.Invoke(data);
        }, onFailure: (err) =>
        {
            Debug.Log("QueryAll" + err);
        }, head);
    }

    // private List<KeyValuePair<string, string>> AddToken()
    // {
    //     List<KeyValuePair<string, string>> head = new List<KeyValuePair<string, string>>();
    //     head.Add(new KeyValuePair<string, string>("Authorization", $"Bearer {Token}"));
    //     return head;
    // }

    public async void test()
    {
        string url = "http://localhost:8080/hello";
        UserDataModel user = new();
        await HttpClientManager.Instance.GetAsync(url, onSuccess: (data) =>
        {
            Debug.Log("QueryAll" + data);
            Dictionary<string, string> ret = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);

        }, onFailure: (err) =>
        {
            Debug.Log("QueryAll" + err);
        });
    }

    public async void Predict(string path, Action<PredictData> onSuccess)
    {
        string url = $"{domain}/task/predict";
        List<KeyValuePair<string, string>> head = new List<KeyValuePair<string, string>>();
        await HttpClientManager.Instance.UploadFile(url, path, onSuccess: (data) =>
        {
            Debug.Log("UploadFile" + data);
            PredictData ret = JsonConvert.DeserializeObject<PredictData>(data);
            onSuccess.Invoke(ret);

        }, onFailure: (err) =>
        {
            Debug.Log("UploadFile" + err);
        }, head);
    }


    public class UserDataModel
    {
        public string email { get; set; }
        public string password { get; set; }
        // public string username { get; set; }
    }

}