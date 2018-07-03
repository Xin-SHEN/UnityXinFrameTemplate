using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UdpSocket
{
    public delegate void UDPSocketDelegate(byte[] pBuf, int dwCount, string tmpIp, ushort tmpPort);
    private UDPSocketDelegate udpDelegate;
    private IPEndPoint udpIP;
    private Socket udpSocket;
    private byte[] recvData;
    private Thread recvThread;
    private int _bufferLength;

    public bool BindSocket(ushort port, int bufferLength, UDPSocketDelegate tmpDelegate)
    {
        udpIP = new IPEndPoint(IPAddress.Any, port);
        UDPConnect();
        udpDelegate = tmpDelegate;
        _bufferLength = bufferLength;
        recvData = new byte[bufferLength];
        recvThread = new Thread(RecvDataThread);
        recvThread.Start();
        return true;
    }

    public void UDPConnect()
    {
        udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        //带有服务器功能
        udpSocket.Bind(udpIP);
    }

    public void UDPDisconnect()
    {
        if (udpSocket.Connected)
        {
            udpSocket.Shutdown(SocketShutdown.Both);
            udpSocket.Close();
        }
    }

    private bool IsRunning = true;
    public void RecvDataThread()
    {
        while (IsRunning)
        {
            if (udpSocket == null || udpSocket.Available < 1)
            {
                Thread.Sleep(50);
                continue;
            }
            lock (this)
            {
                IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
                EndPoint remote = (EndPoint)sender;
                int myCount = udpSocket.ReceiveFrom(recvData, ref remote);
                if (udpDelegate != null)
                {
                    udpDelegate(recvData, myCount, remote.AddressFamily.ToString(), (ushort)sender.Port);
                }
                recvData = new byte[_bufferLength];
            }
        }
    }

    public int SendData(string ip, byte[] data, ushort port)
    {
        IPEndPoint sendToIp = new IPEndPoint(IPAddress.Parse(ip), port);
        //if (!udpSocket.Connected) UDP有Connected么？　
        //{
        //    UDPConnect();
        //}
        int mySend = udpSocket.SendTo(data, data.Length, SocketFlags.None, sendToIp);
        return mySend;
    }
}
