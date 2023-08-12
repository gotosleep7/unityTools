using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class HttpImageUtils
{
    private static HttpImageUtils _instance = null;
    private HttpImageUtils() { }
    public static HttpImageUtils Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new HttpImageUtils();
            }
            return _instance;
        }
    }

    private Dictionary<string, byte[]> imageCache = new Dictionary<string, byte[]>();

    public void LoadSprite(string url, RectTransform targetTransform, Action<Sprite> onLoaded)
    {
        var imageSize = targetTransform.rect.size;
        LoadSprite(url, imageSize, onLoaded);
    }
    public void LoadSprite(string url, Vector2 size, Action<Sprite> onLoaded)
    {
        HttpImageUtils.Instance.LoadImageBytes(url, (bytes) =>
           {
               Texture2D tex = new Texture2D((int)size.x, (int)size.y);
               tex.LoadImage(bytes);
               var sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
               onLoaded?.Invoke(sprite);
           });
    }
    public async void LoadImageBytes(string url, System.Action<byte[]> onImageLoaded)
    {
        if (imageCache.TryGetValue(url, out byte[] cachedImage))
        {
            onImageLoaded?.Invoke(cachedImage);
        }
        else
        {
            await DownloadImage(url, onImageLoaded);
        }
    }

    private async Task DownloadImage(string url, System.Action<byte[]> onImageLoaded)
    {

        await HttpClientManager.Instance.DownloadTexture(url,
        (bytes) =>
        {
            imageCache[url] = bytes;
            onImageLoaded?.Invoke(bytes);
        }
       );
    }


}
