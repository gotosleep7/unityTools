using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using System.Threading;
using Unity.VisualScripting;

public class SocketManager : MonoBehaviour
{
    public static SocketManager Instance;

    public UnityEvent<string> OnReceive;
    SocketServer socketServer;
    void Awake()
    {
        Instance = this;
        SocketManager tetClass = new();
    }
    public void OpenSocketServer(string address, int port)
    {
        socketServer = new SocketServer(address, port);
        socketServer.Start();

    }
    void Update()
    {
        if (MsgQueue.Instance.TryGetMsg(out string msg))
        {
            Debug.Log(msg);
            OnReceive?.Invoke(msg);
        }
    }
    public void CloseSocketServer()
    {
        socketServer?.Stop();
    }
    private void OnDestroy()
    {
        socketServer?.Stop();
    }

}

public class MsgQueue
{
    private static MsgQueue _instance;
    public static MsgQueue Instance
    {
        get
        {
            if (_instance == null) _instance = new MsgQueue();
            return _instance;
        }
    }
    public static Queue<string> msgQueue = new Queue<string>();
    public void EnQueue(string msg)
    {
        msgQueue.Enqueue(msg);
    }
    public bool TryGetMsg(out string msg)
    {
        msg = "";
        if (msgQueue.Count > 0)
        {

            msg = msgQueue.Dequeue();
            return true;
        }
        return false;
    }
}



public class SocketServer
{
    private TcpListener listener; // 用于监听客户端连接请求的对象
    private Thread listenThread; // 用于执行监听任务的线程
    private bool running; // 用于控制监听是否继续的标志

    public SocketServer(string address, int port)
    {
        // 初始化监听对象，使用本机IP地址和指定端口号
        listener = new TcpListener(IPAddress.Parse(address), port);
        // 创建监听线程
        listenThread = new Thread(new ThreadStart(ListenForClients));
        // 设置为后台线程
        listenThread.IsBackground = true;
        // 设置运行标志为true
        running = true;
    }

    public void Start()
    {
        // 启动监听线程
        listenThread.Start();
    }

    public void Stop()
    {
        // 设置运行标志为false
        running = false;
        // 停止监听对象
        listener.Stop();
    }

    private void ListenForClients()
    {
        // 开始监听客户端连接请求
        listener.Start();
        Debug.Log("waiting connect......");
        while (running)
        {
            try
            {
                // 接受一个客户端连接，如果没有连接请求则阻塞
                TcpClient client = listener.AcceptTcpClient();
                // 创建一个新的线程来处理该客户端的通信任务
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                // 设置为后台线程
                clientThread.IsBackground = true;
                // 启动该线程，传入客户端对象作为参数
                clientThread.Start(client);
            }
            catch (SocketException)
            {
                // 如果监听对象被停止，会抛出SocketException异常，此时退出循环
                break;
            }
        }
    }

    private void HandleClientComm(object client)
    {
        // 将参数转换为TcpClient对象
        TcpClient tcpClient = (TcpClient)client;
        // 获取该客户端的网络流对象
        NetworkStream clientStream = tcpClient.GetStream();

        byte[] message = new byte[4096]; // 用于存储接收到的消息的字节数组
        int bytesRead; // 用于记录读取到的字节数
        Debug.Log("新的客户端连接");
        while (running)
        {
            bytesRead = 0;

            try
            {
                // 从网络流中读取数据，如果没有数据则阻塞
                bytesRead = clientStream.Read(message, 0, 4096);
            }
            catch (Exception)
            {
                // 如果发生异常，说明客户端断开连接，此时退出循环
                Debug.Log("客户端[异常]断开连接");
                break;
            }

            if (bytesRead == 0)
            {
                // 如果读取到的字节数为0，说明客户端断开连接，此时退出循环
                Debug.Log("客户端[正常]断开连接");
                break;
            }

            // 将接收到的字节数组转换为字符串
            string data = Encoding.UTF8.GetString(message, 0, bytesRead);
            // 打印出接收到的消息（这里可以根据需要进行其他处理）
            MsgQueue.Instance.EnQueue(data);
            // 将接收到的消息原样发送回客户端（这里可以根据需要发送其他内容）
            byte[] buffer = Encoding.UTF8.GetBytes(data);
            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();
        }

        // 关闭客户端连接
        tcpClient.Close();
    }
}

