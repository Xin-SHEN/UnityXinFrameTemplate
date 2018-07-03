using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 1. 检测外部配置文件夹
/// 2. 隐藏鼠标
/// 3. 定时返回
/// </summary>
public class KioTools : LoadConfigXml_VerifyMediaFolder
{
    //----单例------------------
    public static KioTools Instance;
    void OnEnable() {
        base.OnEnable();
        Instance = this;
        RecheckUDPConfig();
    }

    //----全局静态变量
    public static string VERSION = "1.2.3";
    public static string UNITY_COMPATIBLE_VERSION = "2017.3.0f3";

    //----序列化变量
    [SerializeField] private GameObject _returnActionTarget;  //执行返回操作的GameObject

    //----内部变量
    [HideInInspector] public int _returnHomepageDelay = 0;  //无人操作返回主页的延迟
    [HideInInspector] public bool _mouseHide = false;       //是否隐藏鼠标
    [HideInInspector] public string _targetIPAddress = "";  //IP地址
    [HideInInspector] public int _receivePort = 0;
    [HideInInspector] public int _sendPort = 0;
    private bool clockBlock = false;                        //定时返回时钟锁，防止时钟被误触发

    //此处将在编辑器中运行
    void Start()
    {
        RecheckHideMouse();
        RecheckReturnHomeDelay();

        //@已弃用
        //加载WindowMod模块，是否使用将在内部判断
#if !UNITY_EDITOR
        //gameObject.AddComponent<WindowMod>();
#endif
    }

    //执行返回主页,向相关的脚本发送“ReturnHome”消息
    IEnumerator ReturnHome(int delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            if(_returnActionTarget != null)
                _returnActionTarget.SendMessage("ReturnHome",SendMessageOptions.RequireReceiver);
        }
    }

    /// <summary>
    /// API：重置时钟
    /// </summary>
    public void ResetReturnHomeClock()
    {
        StopAllCoroutines();
        if (!Instance.clockBlock)
            StartCoroutine(ReturnHome(_returnHomepageDelay));
    }

    /// <summary>
    /// API：重置时钟
    /// </summary>
    public static void ResetReturnHomeClockStatic()
    {
        Instance.StopAllCoroutines();
        if (!Instance.clockBlock)
            Instance.StartCoroutine(Instance.ReturnHome(Instance._returnHomepageDelay));
    }

    /// <summary>
    /// API：停止时钟，加锁
    /// </summary>
    public static void StopReturnHomeClockStatic()
    {
        Instance.clockBlock = true;
        Instance.StopAllCoroutines();
    }

    /// <summary>
    /// API：解锁，开始时钟
    /// </summary>
    public static void StartReturnHomeClockStatic()
    {
        Instance.clockBlock = false;
        Instance.StartCoroutine(Instance.ReturnHome(Instance._returnHomepageDelay));
    }

    //隐藏鼠标
    void RecheckHideMouse()
    {
        try
        {
            _mouseHide = ConfigXML.XMLData["MouseHide"].InnerText == "true";
#if !UNITY_EDITOR
            Cursor.visible = !_mouseHide;
#endif
        }
        catch (Exception)
        {
            Debug.LogError("XML文件中 MouseHide 字段有误！更改字段或从模板生成外部配置文件！");
        }
    }

    //检测返回主页时钟设置，并开始执行
    void RecheckReturnHomeDelay()
    {
        try
        {
            if (int.Parse(ConfigXML.XMLData["TimeOut"].InnerText) > 0)
            {
                _returnHomepageDelay = int.Parse(ConfigXML.XMLData["TimeOut"].InnerText);
                StartCoroutine(ReturnHome(_returnHomepageDelay));
            }
            else
            {
                Debug.LogError("XML文件中 TimeOut 字段必须大于零！");
            }
        }
        catch (Exception)
        {
            Debug.LogError("XML文件中 TimeOut 字段有误！更改字段或从模板生成外部配置文件！");
        }
    }

    //检测UDP配置
    void RecheckUDPConfig()
    {
        try
        {
            _targetIPAddress = ConfigXML.XMLData["IP_Target"].InnerText;
            _receivePort = int.Parse(ConfigXML.XMLData["UDP_Port_Receive"].InnerText);
            _sendPort = int.Parse(ConfigXML.XMLData["UDP_Port_Send"].InnerText);
            if (_receivePort == _sendPort)
            Debug.LogError("发送和接收端口号不能相同！");
        }
        catch (Exception)
        {
            Debug.LogError("XML文件中 UDP相关字段有误！更改字段！");
        }
    }

    /// <summary>
    /// API: 
    /// 1. 初始化外部资源
    /// 2. 隐藏鼠标
    /// 3. 返回主页时钟设置，并开始执行
    /// </summary>
    public override void ReCheck()
    {
        base.ReCheck();
        RecheckHideMouse();
        RecheckReturnHomeDelay();
    }
}
