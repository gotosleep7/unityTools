
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class SocketServer
{
    private TcpListener listener; // 用于监听客户端连接请求的对象
    private Thread listenThread; // 用于执行监听任务的线程
    TcpClient currentClient;
    CancellationTokenSource cts;

    public void PushMessageToClient(string message)
    {
        // Debug.Log($"currentClient={currentClient == null}");
        if (currentClient == null) return;
        NetworkStream clientStream = currentClient.GetStream();
        byte[] buffer = Encoding.UTF8.GetBytes(message);
        clientStream?.Write(buffer, 0, buffer.Length);
        clientStream?.Flush();
    }


    public SocketServer(string address, int port)
    {
        cts = new CancellationTokenSource();
        Debug.Log($"listen to {address}:{port}");
        // 初始化监听对象，使用本机IP地址和指定端口号
        listener = new TcpListener(IPAddress.Parse(address), port);
        // 创建监听线程
        // listenThread = new Thread(new ThreadStart(ListenForClients));
        // 设置为后台线程
        // listenThread.IsBackground = true;
        // 设置运行标志为true
    }

    public void Start()
    {
        // 启动监听线程
        Task.Run(() => ListenForClients(cts));
        // listenThread.Start();
    }


    public void Stop()
    {
        // 设置运行标志为false
        cts?.Cancel();
        // 停止监听对象
        listener?.Stop();
    }

    private void ListenForClients(CancellationTokenSource cts)
    {
        // 开始监听客户端连接请求
        listener.Start();
        Debug.Log("waiting connect......");
        while (!cts.IsCancellationRequested)
        {
            try
            {
                // 接受一个客户端连接，如果没有连接请求则阻塞
                TcpClient client = listener.AcceptTcpClient();
                Task.Run(() => HandleClientConn(client));
                // 创建一个新的线程来处理该客户端的通信任务
                // Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientConn));
                // 设置为后台线程
                // clientThread.IsBackground = true;
                // 启动该线程，传入客户端对象作为参数
                // clientThread.Start(client);
            }
            catch (SocketException)
            {
                // 如果监听对象被停止，会抛出SocketException异常，此时退出循环
                break;
            }
        }
    }

    private void HandleClientConn(object client)
    {
        // 将参数转换为TcpClient对象
        currentClient = (TcpClient)client;
        // 获取该客户端的网络流对象
        NetworkStream clientStream = currentClient.GetStream();

        byte[] message = new byte[4096]; // 用于存储接收到的消息的字节数组
        int bytesRead; // 用于记录读取到的字节数
        Debug.Log("新的客户端连接");
        while (!cts.IsCancellationRequested)
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
            // Debug.Log("receive msg " + data);
            // 将接收到的消息原样发送回客户端（这里可以根据需要发送其他内容）
            // byte[] buffer = Encoding.UTF8.GetBytes(data);
            // clientStream.Write(buffer, 0, buffer.Length);
            // clientStream.Flush();
        }

        // 关闭客户端连接
        currentClient?.Close();
    }
}

