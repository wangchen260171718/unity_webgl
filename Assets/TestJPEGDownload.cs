//TestJPEGDownload.cs
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class TestJPEGDownload : MonoBehaviour
{

    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(ScrrenCapture(new Rect(0, 0, Screen.width, Screen.height)));
        }
    }

    Texture2D screenShot;
    IEnumerator ScrrenCapture(Rect rect)
    {
        screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
        yield return new WaitForEndOfFrame();
        screenShot.ReadPixels(rect, 0, 0);
        screenShot.Apply();
        DownLoad(screenShot);

    }

    Sprite sprite;
    private void DownLoad(Texture2D screenShot)
    {
        sprite = Sprite.Create(screenShot, new Rect(0, 0, screenShot.width, screenShot.height), new Vector2(0.5f, 0.5f));

        byte[] photoByte = getImageSprite();//获取jpeg图像的字节流
        if (photoByte != null)
        {
            DownloadImage(photoByte, sprite.name + ".jpg");
        }
        else
        {
            Debug.LogError("QAQ");
        }
    }

    private byte[] getImageSprite()
    {
        if (sprite)
        {
            return sprite.texture.EncodeToJPG();
        }
        return null;
    }



    /// <summary>
    /// 调用js方法下载
    /// </summary>
    /// <param name="str"></param>
    /// <param name="fn"></param>
    [DllImport("__Internal")]
    private static extern void ImageDownloader(string str, string fn);
    public void DownloadImage(byte[] imageData, string imageFileName = "newpic")
    {
        if (imageData != null)
        {
            Debug.Log("Downloading..." + imageFileName);
            ImageDownloader(System.Convert.ToBase64String(imageData), imageFileName);
        }
    }
}