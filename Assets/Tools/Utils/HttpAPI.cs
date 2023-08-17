using System;
using System.Collections.Generic;
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


}