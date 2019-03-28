using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Leaf.Common;

namespace Leaf.VideoCapture
{
    public class VideoCaptureHelper
    {
        public static void Capture(byte[] data, string path, string name)
        {
            FileHelper.SaveBinaryFile(data, path, name);
        }

        public static void Capture(IntPtr handle, int size, string path, string name)
        {
            byte[] byteBuf = new byte[size];
            Marshal.Copy(handle, byteBuf, 0, size);
            Capture(byteBuf, path, name);
        }

        /// <summary>
        /// 抓拍Texture2D保存为png文件
        /// </summary>
        /// <param name="texture">需要抓拍的Texture</param>
        /// <param name="path">文件保存路径</param>
        /// <param name="name">保存文件名</param>
        public static void CaptureToPng(Texture2D texture, string path, string name)
        {
            var bytes = texture.EncodeToPNG();
            FileHelper.SaveBinaryFile(bytes, path, name + ".png");
        }

        /// <summary>
        /// 抓拍屏幕图像保存为png文件（使用协程调用）
        /// </summary>
        /// <param name="path">文件保存路径</param>
        /// <param name="name">保存文件名</param>
        /// <returns></returns>
        public static IEnumerator CaptureScreenToPng(string path, string name)
        {
            yield return new WaitForEndOfFrame();
            var texture = GetScreenShot(Camera.allCameras);
            //var texture = new Texture2D(Screen.width, Screen.height);
            //texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            //yield return 0;
            //texture.Apply();
            CaptureToPng(texture, path, name);
            UnityEngine.Object.Destroy(texture);
        }

        /// <summary>
        /// 抓拍指定相机组图像保存为png文件（使用协程调用）
        /// </summary>
        /// <param name="renderCameras">相机组</param>
        /// <param name="path">文件保存路径</param>
        /// <param name="name">保存文件名</param>
        /// <returns></returns>
        public static IEnumerator CaptureScreenToPng(Camera[] renderCameras, string path, string name)
        {
            yield return new WaitForEndOfFrame();
            var screenShot = GetScreenShot(renderCameras);
            //Texture2D screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            //RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
            //foreach (Camera cam in renderCameras)
            //{
            //    cam.targetTexture = rt;
            //    cam.Render();
            //    cam.targetTexture = null;
            //}
            //RenderTexture.active = rt;
            //screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            //Camera.main.targetTexture = null;
            //RenderTexture.active = null;
            //UnityEngine.Object.Destroy(rt);
            yield return 0;
            CaptureToPng(screenShot, path, name);
            UnityEngine.Object.Destroy(screenShot);
        }

        /// <summary>
        /// 抓拍屏幕图像保存为jpg文件（使用协程调用）
        /// </summary>
        /// <param name="path">文件保存路径</param>
        /// <param name="name">保存文件名</param>
        /// <returns></returns>
        public static IEnumerator CaptureScreenToJpg(string path, string name)
        {
            yield return new WaitForEndOfFrame();
            var texture = GetScreenShot(Camera.allCameras);
            //var texture = new Texture2D(Screen.width, Screen.height);
            //texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            //yield return 0;
            //texture.Apply();
            //var bytes = texture.EncodeToJPG();
            CaptureToJpg(texture, path, name);
            UnityEngine.Object.Destroy(texture);
        }

        /// <summary>
        /// 抓拍指定相机组图像保存为jpg文件（使用协程调用）
        /// </summary>
        /// <param name="renderCameras">相机组</param>
        /// <param name="path">文件保存路径</param>
        /// <param name="name">保存文件名</param>
        /// <returns></returns>
        public static IEnumerator CaptureScreenToJpg(Camera[] renderCameras, string path, string name)
        {
            yield return new WaitForEndOfFrame();
            var screenShot = GetScreenShot(renderCameras);
            //Texture2D screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            //RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
            //foreach (Camera cam in renderCameras)
            //{
            //    cam.targetTexture = rt;
            //    cam.Render();
            //    cam.targetTexture = null;
            //}
            //RenderTexture.active = rt;
            //screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            //Camera.main.targetTexture = null;
            //RenderTexture.active = null;
            //UnityEngine.Object.Destroy(rt);
            //yield return 0;
            CaptureToJpg(screenShot, path, name);
            UnityEngine.Object.Destroy(screenShot);
        }

        /// <summary>
        /// 抓拍Texture2D保存为jpg文件
        /// </summary>
        /// <param name="texture">需要抓拍的Texture</param>
        /// <param name="path">文本保存路径</param>
        /// <param name="name">保存文件名</param>
        public static void CaptureToJpg(Texture2D texture, string path, string name)
        {
            var bytes = texture.EncodeToJPG();
            FileHelper.SaveBinaryFile(bytes, path, name + ".jpg");
        }

        public static void CaptureToPng(byte[] data, string path, string name, TextureFormat textureFormat)
        {
            Texture2D texture = new Texture2D(1, 1, textureFormat, false);
            texture.LoadRawTextureData(data);
            texture.Apply();
            byte[] bytes = texture.EncodeToPNG();
            FileHelper.SaveBinaryFile(bytes, path, name + ".png");
        }

        public static void CaptureToPng(IntPtr handle, int size, string path, string name, TextureFormat textureFormat)
        {
            byte[] byteBuf = new byte[size];
            Marshal.Copy(handle, byteBuf, 0, size);
            CaptureToPng(byteBuf, path, name, textureFormat);
        }

        /// <summary>
        /// 以IO形式加载抓拍图像
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <param name="textureFormat"></param>
        /// <returns></returns>
        public static Texture2D LoadCaptureImageByIO(string path, string name, TextureFormat textureFormat)
        {
            byte[] data = FileHelper.LoadBinaryFile(path, name);
            Texture2D texture = new Texture2D(1, 1, textureFormat, false);
            texture.LoadRawTextureData(data);
            texture.Apply();
            return texture;
        }

        public static void LoadCaptureImageByIO(string path, string name, TextureFormat textureFormat, Action<Texture2D> TextureHandle)
        {
            Texture2D texture = LoadCaptureImageByIO(path, name, textureFormat);
            if (TextureHandle != null)
                TextureHandle(texture);
        }

        public static List<Texture2D> LoadCaptureImagesByIO(string path, TextureFormat textureFormat)
        {
            string[] files = FileHelper.GetDirectoryFiles(path);
            List<Texture2D> textures = new List<Texture2D>();
            foreach (var item in files)
            {
                var tex = LoadCaptureImageByIO(path, item, textureFormat);
                textures.Add(tex);
            }
            return textures;
        }

        /// <summary>
        /// 以IO形式加载抓拍png图像
        /// </summary>
        /// <param name="path">图像路径</param>
        /// <param name="name">图像文件名</param>
        /// <returns>加载的Texture2D</returns>
        public static Texture2D LoadCapturePngByIO(string path, string name)
        {
            byte[] data = FileHelper.LoadBinaryFile(path, name + ".png");
            if (data != null)
            {
                Texture2D tex = new Texture2D(1, 1);
            tex.LoadImage(data);
            return tex;
            }
            else
                return Texture2D.whiteTexture;
        }

        /// <summary>
        /// 以IO形式加载抓拍jpg图像
        /// </summary>
        /// <param name="path">图像路径</param>
        /// <param name="name">图像文件名</param>
        /// <returns>加载的Texture2D</returns>
        public static Texture2D LoadCaptureJpgByIO(string path, string name)
        {
            byte[] data = FileHelper.LoadBinaryFile(path, name + ".jpg");
            if (data != null)
            {
                Texture2D tex = new Texture2D(1, 1);
                tex.LoadImage(data);
                return tex;
            }
            else
                return Texture2D.whiteTexture;
        }

        /// <summary>
        /// 以IO形式加载目录下所有抓拍png图像
        /// </summary>
        /// <param name="path">文件目录路径</param>
        /// <returns>加载的Texture2D列表</returns>
        public static List<Texture2D> LoadCapturePngsByIO(string path)
        {
            string[] files = FileHelper.GetDirectoryFiles(path);
            List<Texture2D> textures = new List<Texture2D>();
            foreach (var item in files)
            {
                if (item.EndsWith(".png"))
                {
                    var tex = LoadCapturePngByIO(path, item.Replace(".png", ""));
                    textures.Add(tex);
                }
            }
            return textures;
        }

        /// <summary>
        /// 以IO形式加载目录下所有抓拍jpg图像
        /// </summary>
        /// <param name="path">文件目录路径</param>
        /// <returns>加载的Texture2D列表</returns>
        public static List<Texture2D> LoadCaptureJpgsByIO(string path)
        {
            string[] files = FileHelper.GetDirectoryFiles(path);
            List<Texture2D> textures = new List<Texture2D>();
            foreach (var item in files)
            {
                if (item.EndsWith(".jpg"))
                {
                    var tex = LoadCaptureJpgByIO(path, item.Replace(".jpg", ""));
                    textures.Add(tex);
                }
            }
            return textures;
        }

        public static IEnumerator LoadCaptureImageByWWW(string _path, TextureFormat textureFormat, Action<Texture2D> TextureHandle)
        {
#if UNITY_EDITOR || UNITY_IOS
            _path = "file://" + _path;
#endif
            WWW www = new WWW(_path);
            yield return www;
            if (string.IsNullOrEmpty(www.error) == false)
            {
                Debug.Log("error");
            }
            else
            {
                Texture2D texture = new Texture2D(1, 1, textureFormat, false);
                texture.LoadRawTextureData(www.bytes);
                texture.Apply();
                if (TextureHandle != null)
                    TextureHandle(texture);
            }
        }

        public static Texture2D GetScreenShot(Camera[] renderCameras)
        {
            RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
            Texture2D screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            foreach (Camera cam in renderCameras)
            {
                cam.targetTexture = rt;
                cam.Render();
                cam.targetTexture = null;
            }
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            if (Camera.main != null)
                Camera.main.targetTexture = null;
            RenderTexture.active = null;
            UnityEngine.Object.Destroy(rt);
            return screenShot;
        }
    }
}