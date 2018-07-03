using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class UDPMSG {
    public string IP { get; set; }
    public int Port { get; set; }
    public string DataString { get; set; }

    public UDPMSG(string IP, int Port, string DataString) {
        this.IP = IP;
        this.Port = Port;
        this.DataString = DataString;
    }
}

public class UDPAgent : MonoBehaviour
{
    //---单例设置---------------------------------
    public static UDPAgent Instance = null;

    //---序列化变量-------------------------------
    [SerializeField] private bool bindOnAwake = true;
    [SerializeField] [Tooltip("UDP发送队列时间间隔，单位秒")]private float udpQueueInterval = 0.05f;

    //---内部变量---------------------------------
    private UdpSocket udpSocket;
    private string ip_Target;
    private uint udp_Port_Receive, udp_Port_Send;
    private int bufferLength = 1024;
    private List<UDPMSG> msgList;

    //---公共变量---------------------------------
    [HideInInspector] public string recvData;

    void Awake()
    {
        Instance = this;
        if (bindOnAwake) Initial();
    }

    //初始化
    void Initial()
    {
        msgList = new List<UDPMSG>();
        udpSocket = new UdpSocket();
        if (!CheckConfig())
        {
            Debug.LogError("XML文件中通信设置读取错误！UDP模块初始化失败");
            return;
        }
        if(udpSocket.BindSocket((ushort)udp_Port_Receive, bufferLength, DataReceive)) { 
            Debug.Log("UDP模块初始化成功。");
            StartCoroutine(SendUdpLoop());
        }
    }

    //读取配置文件
    bool CheckConfig()
    {
        try
        {
            udp_Port_Receive = uint.Parse(ConfigXML.XMLData["UDP_Port_Receive"].InnerText);
            udp_Port_Send = uint.Parse(ConfigXML.XMLData["UDP_Port_Send"].InnerText);
            ip_Target = ConfigXML.XMLData["IP_Target"].InnerText;
        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }

    // 处理从网络收到的套接字报文，并处理
    void DataReceive(byte[] pBuf, int dwCount, string tmpIp, ushort tmpPort)
    {
        byte[] realData = new byte[dwCount];
        Buffer.BlockCopy(pBuf, 0, realData, 0, dwCount);
        var str = Encoding.Default.GetString(realData);
        Debug.Log(str);
        //todo : 在这里处理所有的UDP消息，要加上校验和MsgId！！！
        recvData = str;
    }

    /// <summary>
    /// API:向默认的IP/PORT发送UDP报文,将报文加入队列依次发送
    /// </summary>
    /// <param name="udpData">字符串类型报文</param>
    public void SendUDP(string udpDataString)
    {
        //udpSocket.SendData(ip_Target, Encoding.Default.GetBytes(udpDataString), (ushort)udp_Port_Send);
        msgList.Add(new UDPMSG(ip_Target, (int)udp_Port_Send, udpDataString));
    }

    /// <summary>
    /// API:向指定的IP/PORT发送UDP报文,将报文加入队列依次发送
    /// </summary>
    /// <param name="udpData">字符串类型报文</param>
    public void SendUDPByIpPort(string udpDataString, string ip, int port)
    {
        //udpSocket.SendData(ip, Encoding.Default.GetBytes(udpDataString), (ushort)port);
        msgList.Add(new UDPMSG(ip, port, udpDataString));
    }

    /// <summary>
    /// 循环发送UDP消息队列
    /// </summary>
    /// <returns></returns>
    IEnumerator SendUdpLoop()
    {
        while (true)
        {
            if (msgList.Count > 0)
            {
                SendData(msgList[0]);
                msgList.RemoveAt(0);
            }
            yield return new WaitForSecondsRealtime(udpQueueInterval); 
        }
    }

    /// <summary>
    /// 发送UDP指令
    /// </summary>
    /// <param name="msg"></param>
    private void SendData(UDPMSG msg) {
        udpSocket.SendData(msg.IP, Encoding.Default.GetBytes(msg.DataString), (ushort)msg.Port);
        Debug.Log(msg.IP+" "+msg.Port+" "+msg.DataString);
    }

    void OnApplicationQuit()
    {
        udpSocket.UDPDisconnect();
    }
}
