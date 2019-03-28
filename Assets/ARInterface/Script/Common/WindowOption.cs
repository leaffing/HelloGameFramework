using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
//using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using System.Runtime.InteropServices;


public class WindowOption : MonoBehaviour
{
    private Rect screenPosition;

    [DllImport("user32.dll")]
    static extern IntPtr SetWindowLong(IntPtr hwnd, int _nIndex, int dwNewLong);
    [DllImport("user32.dll")]
    static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();
    [DllImport("user32.dll")]
    public static extern bool ReleaseCapture();
    [DllImport("user32.dll")]
    public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
    [DllImport("user32.dll")]
    public static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);

    /// <summary>  
    /// 窗口宽度  
    /// </summary>  
    public int winWidth;
    /// <summary>  
    /// 窗口高度  
    /// </summary>  
    public int winHeight;
    /// <summary>  
    /// 窗口左上角x  
    /// </summary>  
    public int winPosX;
    /// <summary>  
    /// 窗口左上角y  
    /// </summary>  
    public int winPosY;


    const uint SWP_SHOWWINDOW = 0x0040;
    const int GWL_STYLE = -16;
    const int WS_BORDER = 1;
    const int WS_POPUP = 0x800000;
    const int SW_SHOWMINIMIZED = 2; //{最小化, 激活}
    const int SW_SHOWMAXIMIZED = 3; //{最大化, 激活} 

    /// <summary>
    /// 最小化窗口
    /// </summary>
    public void MinimizedWindow()
    { 
        ShowWindow(GetForegroundWindow(), SW_SHOWMINIMIZED);
    }

    /// <summary>
    /// 最大化窗口
    /// </summary>
    public void MaximizedWindow()
    { 
        ShowWindow(GetForegroundWindow(), SW_SHOWMAXIMIZED);
    }

    /// <summary>
    /// 窗口化显示
    /// </summary>
    public void WindowDisplay(int width, int height)
    {
        winWidth = width;
        winHeight = height;
        Screen.fullScreen = false;
        Screen.SetResolution(width, height, false);
    }

    /// <summary>
    /// 全屏播放
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public void FullScreen(int width, int height)
    {
        Screen.SetResolution(width, height, true);
    }

    /// <summary>
    /// 默认分辨率无边框显示
    /// </summary>
    public void WindowWithoutPopup(int width, int height)
    {
        winWidth = width;
        winHeight = height;
        //显示器支持的所有分辨率  
        int i = Screen.resolutions.Length;
        int resWidth = Screen.resolutions[i - 1].width;
        int resHeight = Screen.resolutions[i - 1].height;
        winPosX = resWidth / 2 - winWidth / 2;
        winPosY = resHeight / 2 - winHeight / 2;
        SetWindowLong(GetForegroundWindow(), GWL_STYLE, WS_POPUP);
        bool result = SetWindowPos(GetForegroundWindow(), 0, winPosX, winPosY, winWidth, winHeight, SWP_SHOWWINDOW);
    }

    public void WindowWithoutPopupBySecondResolution()
    {
        //第二分辨率显示
        Resolution[] resolutions = Screen.resolutions;
        int i = resolutions.Length;
        winWidth = resolutions[i - 2].width;
        winHeight = resolutions[i - 2].height;

        int resWidth = Screen.resolutions[i - 1].width;
        int resHeight = Screen.resolutions[i - 1].height;

        winPosX = resWidth / 2 - winWidth / 2;
        winPosY = resHeight / 2 - winHeight / 2;

        SetWindowLong(GetForegroundWindow(), GWL_STYLE, WS_POPUP);
        bool result = SetWindowPos(GetForegroundWindow(), 0, winPosX, winPosY, winWidth, winHeight, SWP_SHOWWINDOW);
    }


    IntPtr Handle;
    bool bx;
    bool isMainScene = false;


    void Awake()
    {
        bx = false;
#if UNITY_STANDALONE_WIN
        Resolution[] r = Screen.resolutions;
        screenPosition = new Rect((r[r.Length - 1].width - Screen.width) / 2, (r[r.Length - 1].height - Screen.height) / 2, Screen.width, Screen.height);
        SetWindowLong(GetForegroundWindow(), GWL_STYLE, WS_POPUP);//将网上的WS_BORDER替换成WS_POPUP  
        Handle = GetForegroundWindow();   //FindWindow ((string)null, "popu_windows");
        SetWindowPos(GetForegroundWindow(), 0, (int)screenPosition.x, (int)screenPosition.y, (int)screenPosition.width, (int)screenPosition.height, SWP_SHOWWINDOW);
#endif
        //WindowWithoutPopup(900, 540);
        WindowWithoutPopupBySecondResolution();
    }

    void Update()
    {
        MyDrag();
        //测试各个分辨率的按键。
        if (Input.GetKey(KeyCode.A))
        {
            WindowWithoutPopupBySecondResolution();
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            MinimizedWindow();
        }
        if (Input.GetKey(KeyCode.W))
        {
            MaximizedWindow();
        }
        if (Input.GetKey(KeyCode.R))
        {
            WindowDisplay(900, 540);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            WindowWithoutPopup(900, 540);
        }
        if (Input.GetKey(KeyCode.F))
        {
            FullScreen(1920, 1080);
        }
    }

    /// <summary>
    /// 自定义的只能在某些区域内点击有效的拖拽
    /// </summary>
    private void MyDrag()
    {
        if (bx)
        {
            //这样做为了区分界面上面其它需要滑动的操作
            ReleaseCapture();
            SendMessage(Handle, 0xA1, 0x02, 0);
            SendMessage(Handle, 0x0202, 0, 0);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        bx = true;
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        bx = false;
    }
}