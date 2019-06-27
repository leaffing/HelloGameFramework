using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class TaskTest : MonoBehaviour
{

    public RawImage image;

    public Transform cube;
	// Use this for initialization
	void Start () {
		SetImage();
	}
	
	// Update is called once per frame
	void Update ()
    {
        cube.Rotate(0, 100 * Time.deltaTime, 0);
    }

    async void SetImage()
    {
        var task = Task.Run(() =>
        {
            //var buffer = File.ReadAllBytes("‪logo.png");
            FileStream fs = new FileStream("‪logo.png", FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[fs.Length];

            fs.Read(buffer, 0, buffer.Length);
            fs.Close();
            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(1000);
                Debug.LogError(i);
            }
            return buffer; 
        });

        Texture2D tex = new Texture2D(2,2);
        tex.LoadRawTextureData(await task);
        tex.Apply();
        image.texture = tex;
    }
}
