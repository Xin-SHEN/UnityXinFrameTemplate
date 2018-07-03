using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Diagnostics;
using UnityEngine;
public class UnityWindowControl : MonoBehaviour
{
    [DllImport("user32.dll")]
    static extern IntPtr SetWindowLong(IntPtr hwnd, int _nIndex, int dwNewLong);
    [DllImport("user32.dll")]
    static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    static extern IntPtr GetActiveWindow();
    [DllImport("user32.dll")]
    static extern bool LockSetForegroundWindow(uint uLockCode);
    [DllImport("user32.dll")]
    static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

    // not used rigth now
    //const uint SWP_NOMOVE = 0x2;
    //const uint SWP_NOSIZE = 1;
    //const uint SWP_NOZORDER = 0x4;
    //const uint SWP_HIDEWINDOW = 0x0080;
    const uint SWP_SHOWWINDOW = 0x0040;
    const int GWL_STYLE = -16;
    const int WS_BORDER = 1;

    //是否使用本功能
    bool IsWindowModEnabled = false;

    //窗体分辨率与位置
    Rect screenPosition;

    //是否自动检测窗体状态
    bool IsAutoResetResolution = false;
    bool WindowForegroundCheck = false;
    int WindowCheckInterval = 60;
    private const uint LOCK = 1;
    private const uint UNLOCK = 2;
    private IntPtr window;

    void Start()
    {
        LockSetForegroundWindow(LOCK);
        window = GetActiveWindow();

        try
        {
            WindowCheckInterval = int.Parse(ConfigXML.XMLData["WindowCheckInterval"].InnerText);
            WindowForegroundCheck = ConfigXML.XMLData["WindowForegroundCheck"].InnerText == "true";

            IsWindowModEnabled = ConfigXML.XMLData["WindowMod"].InnerText == "true";
            screenPosition.x = int.Parse(ConfigXML.XMLData["WindowPositionX"].InnerText);
            screenPosition.y = int.Parse(ConfigXML.XMLData["WindowPositionY"].InnerText);
            screenPosition.width = int.Parse(ConfigXML.XMLData["WindowWidth"].InnerText);
            screenPosition.height = int.Parse(ConfigXML.XMLData["WindowHeight"].InnerText);
            IsAutoResetResolution = ConfigXML.XMLData["AutoResetResolution"].InnerText == "true";

            ResetWindow();
        }
        catch (Exception)
        {
            UnityEngine.Debug.Log("WindowMod 外部配置错误！");
        }

#if !UNITY_EDITOR
        StartCoroutine(Checker(WindowCheckInterval));
#endif
    }

    IEnumerator Checker(int interval)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            ResetWindow();
        }
    }

    void ResetWindow()
    {
#if !UNITY_EDITOR
        IntPtr currentWindow = GetActiveWindow();
        if (window != currentWindow && WindowForegroundCheck)
        {
            SwitchToThisWindow(window, true);
        }

        if (IsAutoResetResolution || (screenPosition.width!= Screen.currentResolution.width && screenPosition.height != Screen.currentResolution.height))
        {
            SetWindowLong(window, GWL_STYLE, WS_BORDER);
            bool result = SetWindowPos(window, 0, (int)screenPosition.x, (int)screenPosition.y, (int)screenPosition.width, (int)screenPosition.height, SWP_SHOWWINDOW);
        }
#endif
    }
}
