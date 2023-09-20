using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

public class HttpClientManager
{
    private static HttpClientManager _httpClientHelper = null;

    private HttpClient _httpClient;

    private HttpClientManager() { }
    public string Token { get; set; }

    public static HttpClientManager Instance
    {
        get
        {
            if (_httpClientHelper == null)
            {
                HttpClientManager httpClientHelper = new HttpClientManager();

                //取消使用默认的Cookies
                HttpClientHandler handler = new HttpClientHandler() { UseCookies = false };
                httpClientHelper._httpClient = new HttpClient(handler);
                return httpClientHelper;
            }

            return _httpClientHelper;
        }
    }
    public static bool cancleAllDownTask;
    public async Task GetAsync(string url)
    {
        await GetAsync(url, null, null, null);
    }

    public async Task DownloadTexture(string url, Action<byte[]> onSuccess)
    {
        await DownloadTexture(url, onSuccess, null);
    }
    public async Task DownloadTexture(string url, Action<byte[]> onSuccess, Action<string> onFailure)
    {
        HttpResponseMessage response = await _httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            byte[] textureData = await response.Content.ReadAsByteArrayAsync();
            onSuccess?.Invoke(textureData);
        }
        else
        {
            onFailure?.Invoke("纹理下载失败: " + response.ReasonPhrase);
        }
    }
    /// <summary>
    /// 用于json请求
    /// </summary>
    public async Task PostAsync(string url, string content)
    {
        await PostAsync(url, content, null, null, null);
    }
    /// <summary>
    /// 带请求头的json请求
    /// </summary>
    public async Task PostAsync(string url, string content, List<KeyValuePair<string, string>> headers)
    {
        await PostAsync(url, content, null, null, headers);
    }
    /// <summary>
    /// form表单
    /// </summary>
    public async Task PostAsync(string url, List<KeyValuePair<String, String>> paramList)
    {
        await PostAsync(url, paramList, null, null, null);
    }
    /// <summary>
    /// 带请求头的form表单请求
    /// </summary>
    public async Task PostAsync(string url, List<KeyValuePair<String, String>> paramList, List<KeyValuePair<string, string>> headers)
    {
        await PostAsync(url, paramList, null, null, headers);
    }
    public async Task PutAsync(string url)
    {
        await PutAsync(url, null, null, null);
    }
    public async Task DeleteAsync(string url)
    {
        await DeleteAsync(url, null, null, null);
    }

    /// <summary>
    /// Get方法请求
    /// </summary>
    /// <param name="url">请求地址</param>
    /// <returns></returns>
    public async Task GetAsync(string url, Action<string> onSuccess, Action<string> onFailure, List<KeyValuePair<string, string>> headers = null)
    {
        try
        {
            HttpRequestMessage request = new HttpRequestMessage()
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Get,
            };
           
            if (headers != null && headers.Count > 0)
            {
                request.Headers.Clear();
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            if (!"".Equals(HttpAPI.Instance.Token) && HttpAPI.Instance.Token != null)
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer "+HttpAPI.Instance.Token);
               // httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpAPI.Instance.Token);

            }

           
            var response = await _httpClient.SendAsync(request);
            HandlerResponse(response, onSuccess, onFailure);
        }
        catch (System.Exception ex)
        {

            onFailure?.Invoke(ex.Message);
        }
    }



    /// <summary>
    /// Post方法请求 application/x-www-form-urlencoded格式
    /// </summary>
    /// <param name="url">请求地址</param>
    /// <param name="paramList">参数集合</param>
    /// <returns></returns>
    // public HttpResponseMessage Post(string url, List<KeyValuePair<String, String>> paramList, List<KeyValuePair<string, string>> headers = null)
    // {
    //     FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(paramList);
    //     if (headers != null && headers.Count > 0)
    //     {
    //         formUrlEncodedContent.Headers.Clear();
    //         foreach (var header in headers)
    //         {
    //             formUrlEncodedContent.Headers.Add(header.Key, header.Value);
    //         }
    //     }
    //     HttpResponseMessage response = httpClient.PostAsync(new Uri(url), formUrlEncodedContent).Result;
    //     return response;
    // }

    public async Task PostAsync(string url, List<KeyValuePair<String, String>> paramList, Action<string> onSuccess, Action<string> onFailure, List<KeyValuePair<string, string>> headers)
    {
        try
        {

            FormUrlEncodedContent formUrlEncodedContent = new(paramList);
            formUrlEncodedContent = (FormUrlEncodedContent)HandleHeaders(headers, formUrlEncodedContent);
            var response = await _httpClient.PostAsync(new Uri(url), formUrlEncodedContent);
            HandlerResponse(response, onSuccess, onFailure);
        }
        catch (System.Exception ex)
        {
            onFailure?.Invoke(ex.Message);
        }
    }

    /// <summary>
    /// Post方法请求 raw data
    /// </summary>
    /// <param name="url">请求地址</param>
    /// <param name="content">raw data</param>
    /// <returns></returns>
    // public HttpResponseMessage Post(string url, string content, Action<string> onSuccess, Action<string> onFailure = null, List<KeyValuePair<string, string>> headers = null)
    // {
    //     StringContent stringContent = new StringContent(content, Encoding.UTF8);
    //     if (headers != null && headers.Count > 0)
    //     {
    //         stringContent.Headers.Clear();
    //         foreach (var header in headers)
    //         {
    //             //if (!header.Key.ToLower().Contains("accept") && header.Key!= "Cache-Control" && header.Key!= "Connection" && header.Key!= "Host" &&  header.Key != "User-Agent")
    //             //{
    //             //    stringContent.Headers.TryAddWithoutValidation(header.Key, header.Value);
    //             //}
    //             stringContent.Headers.TryAddWithoutValidation(header.Key, header.Value);
    //         }
    //     }

    //     HttpResponseMessage response = httpClient.PostAsync(new Uri(url), stringContent).Result;
    //     return response;
    // }

    /// <summary>
    /// Post方法请求 raw data
    /// </summary>
    /// <param name="url">请求地址</param>
    /// <param name="content">raw data</param>
    /// <returns></returns>
    public async Task PostAsync(string url, string content, Action<string> onSuccess, Action<string> onFailure, List<KeyValuePair<string, string>> headers = null)
    {
        try
        {
            StringContent stringContent = new(content, Encoding.UTF8);
            stringContent = (StringContent)HandleHeaders(headers, stringContent);
            HttpResponseMessage response = await _httpClient.PostAsync(new Uri(url), stringContent);
            HandlerResponse(response, onSuccess, onFailure);
        }
        catch (Exception ex)
        {
            onFailure?.Invoke(ex.Message);
        }
    }
    /// <summary>
    /// Put方法请求 raw data
    /// </summary>
    /// <param name="url">请求地址</param>
    /// <param name="content">raw data</param>
    /// <returns></returns>
    public async Task PutAsync(string url, string content, Action<string> onSuccess, Action<string> onFailure, List<KeyValuePair<string, string>> headers = null)
    {
        try
        {

            StringContent stringContent = new(content, Encoding.UTF8);

            stringContent = (StringContent)HandleHeaders(headers, stringContent);

            HttpResponseMessage response = await _httpClient.PutAsync(new Uri(url), stringContent);
            HandlerResponse(response, onSuccess, onFailure);
        }
        catch (System.Exception ex)
        {

            onFailure?.Invoke(ex.Message);
        }

    }

    /// <summary>
    /// Put方法请求 raw data
    /// </summary>
    /// <param name="url">请求地址</param>
    /// <param name="content">raw data</param>
    /// <returns></returns>
    public async Task DeleteAsync(string url, string content, Action<string> onSuccess, Action<string> onFailure, List<KeyValuePair<string, string>> headers = null)
    {
        try
        {
            StringContent stringContent = new(content, Encoding.UTF8);
            stringContent = (StringContent)HandleHeaders(headers, stringContent);
            HttpRequestMessage request = new()
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Delete,
                Content = stringContent
            };
            var response = await _httpClient.SendAsync(request);
            HandlerResponse(response, onSuccess, onFailure);
        }
        catch (System.Exception ex)
        {

            onFailure?.Invoke(ex.Message);
        }
    }
    public async Task UploadFileByBytes(string url, byte[] imageBytes, string fileName, Action<string> onSuccess, Action<string> onFailure, List<KeyValuePair<string, string>> headers = null)
    {
        // 创建MultipartFormDataContent对象
        using MultipartFormDataContent formData = new();

        // 创建ByteArrayContent对象，并将图片字节数组作为内容
        ByteArrayContent imageContent = new(imageBytes);
        imageContent = (ByteArrayContent)HandleHeaders(headers, imageContent);
        long timestamp = System.DateTime.UtcNow.Ticks / System.TimeSpan.TicksPerMillisecond;

        // 添加图片内容到表单数据
        formData.Add(imageContent, "file", $"{timestamp}{fileName}.png");

        // client.PostAsync($"{httpLogController.Url}/upload", formData);
        // 发送POST请求
        using HttpResponseMessage response = await _httpClient.PostAsync(url, formData);
        HandlerResponse(response, onSuccess, onFailure);

    }

    public async Task UploadFile(string url, string filePath, Action<string> onSuccess, Action<string> onFailure, List<KeyValuePair<string, string>> headers = null)
    {
        {
            using FileStream fileStream = File.OpenRead(filePath);
            using MultipartFormDataContent formData = new();
            StreamContent fileContent = new(fileStream);
            fileContent = (StreamContent)HandleHeaders(headers, fileContent);
            fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
            {
                Name = "file",
                FileName = Path.GetFileName(filePath)
            };

            formData.Add(fileContent, "file", Path.GetFileName(filePath));
            using HttpResponseMessage response = await _httpClient.PostAsync(url, formData);
            HandlerResponse(response, onSuccess, onFailure);
            fileContent.Dispose();
            // response.EnsureSuccessStatusCode();
        }
    }
    public async Task DownloadFile(string url, string savePath, Action<float> onProgress, int speed = 1024)
    {
        using HttpResponseMessage response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
        response.EnsureSuccessStatusCode();

        using Stream contentStream = await response.Content.ReadAsStreamAsync();
        long totalBytes = response.Content.Headers.ContentLength ?? -1;
        long downloadedBytes = 0;

        using FileStream fileStream = new(savePath, FileMode.Create, FileAccess.Write, FileShare.None);
        byte[] buffer = new byte[1024 * speed];
        int bytesRead;
        float progress;

        while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
        {

            await fileStream.WriteAsync(buffer, 0, bytesRead);

            downloadedBytes += bytesRead;
            progress = (float)downloadedBytes / totalBytes;
            onProgress(progress);
            if (cancleAllDownTask) { cancleAllDownTask = false; return; }
        }
    }

    private HttpContent HandleHeaders(List<KeyValuePair<string, string>> headers, HttpContent httpContent)
    {
        if (headers != null && headers.Count > 0)
        {
            httpContent.Headers.Clear();
            foreach (var header in headers)
            {
                httpContent.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }
        if (!"".Equals(HttpAPI.Instance.Token) && HttpAPI.Instance.Token != null)
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpAPI.Instance.Token);
        return httpContent;
    }



    /// <summary>
    /// 释放httpclient
    /// </summary>
    public void Release()
    {
        _httpClient.Dispose();
    }

    /// <summary>
    /// 设置默认请求头
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    public void SetDefaultHeaders(string name, string value)
    {
        _httpClient.DefaultRequestHeaders.Add(name, value);
    }

    /// <summary>
    /// 删除默认请求头
    /// </summary>
    /// <param name="name"></param>
    public void RemoveDefaultHeaders(string name)
    {
        _httpClient.DefaultRequestHeaders.Remove(name);
    }
    private async void HandlerResponse(HttpResponseMessage response, Action<string> onSuccess, Action<string> onFailure)
    {
        try
        {
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                onSuccess?.Invoke(responseBody);
            }
            else
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                onFailure?.Invoke(responseBody);
            }
        }
        catch (System.Exception)
        {
            throw;
        }
    }
}