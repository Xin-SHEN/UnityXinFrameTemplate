using UnityEngine;
using System.Collections;

public class LogWithTimestamp{

    public static void Log(string text)
    {
        Debug.Log(System.DateTime.Now.Hour + @"时" + System.DateTime.Now.Minute + @"分" + System.DateTime.Now.Second + "秒|" + text);
    }

    public static void LogError(string text)
    {
        Debug.LogError(System.DateTime.Now.Hour + @"时" + System.DateTime.Now.Minute + @"分" + System.DateTime.Now.Second + "秒|" + text);
    }
}
