using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TaskTest : MonoBehaviour
{

    public RawImage image;
    public Text text;
    public Transform cube;

    private byte[] buffer;
	// Use this for initialization
	void Start () {
        //var buffer = File.ReadAllBytes("‪logo.png");
		SetImage();
        //StartCoroutine(ReadFile("C:/Users/yewei/Desktop/logo.png", s => SetImage(s)));
    }
	
	// Update is called once per frame
	void Update ()
    {
        cube.Rotate(0, 100 * Time.deltaTime, 0);
    }

    IEnumerator ReadFile(string url, Action<byte[]> CallBack)
    {
        UnityWebRequest request = new UnityWebRequest(url);
        yield return request.SendWebRequest();
        if (request.isHttpError || request.isNetworkError)
        {

        }
        else
        {
            CallBack?.Invoke(request.downloadHandler.data);
        }
    }

    async void SetImage()
    {
        var task = Task.Run(() =>
        {
            //FileStream fs = new FileStream("‪logo.png", FileMode.Open, FileAccess.Read);
            //byte[] buffer = new byte[fs.Length];
            //fs.Read(buffer, 0, buffer.Length);
            //fs.Close();
            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(1000);
                Debug.LogError(i);
            }
            Debug.LogError("Finish");
            return "Finish"; 
        });

        //Texture2D tex = new Texture2D(2,2);
        //tex.LoadRawTextureData(await task);
        //tex.Apply();
        //image.texture = tex;
        text.text = await task;
    }
}
