using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SocketClientManager : MonoBehaviour
{
    // private static SocketClient socketClient;
    private static SocketClient socketClient;
    public string address = "127.0.0.1";
    public int port = 9999;
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
    }

    [ContextMenu("openSocket")]
    public void ToOpenSocket()
    {
        socketClient = new SocketClient(address, port);
        // clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        // OpenSocket();
    }
    [ContextMenu("closeSocket")]
    public void ToCloseSocket()
    {
        socketClient.Close();
        // CloseSocket();
    }
    public void SendToSocket(string msg)
    {
        socketClient.Send(msg);
    }

}




public class SocketClient
{
    private TcpClient client; // 用于连接服务端的对象

    public SocketClient(string ip, int port)
    {
        // 初始化客户端对象，使用指定的IP地址和端口号
        client = new TcpClient(ip, port);
    }

    public void Send(string message)
    {
        // 获取客户端的网络流对象
        NetworkStream stream = client.GetStream();
        // 将要发送的消息转换为字节数组
        byte[] buffer = Encoding.UTF8.GetBytes(message);
        // 将字节数组写入网络流
        stream.Write(buffer, 0, buffer.Length);
        // 刷新网络流
        stream.Flush();
    }

    public string Receive()
    {
        // 获取客户端的网络流对象
        NetworkStream stream = client.GetStream();
        // 创建一个字节数组用于存储接收到的数据
        byte[] buffer = new byte[4096];
        // 从网络流中读取数据，如果没有数据则阻塞
        int bytesRead = stream.Read(buffer, 0, 4096);
        // 将接收到的字节数组转换为字符串
        string data = Encoding.UTF8.GetString(buffer, 0, bytesRead);
        // 返回接收到的字符串
        return data;
    }

    public void Close()
    {
        // 关闭客户端连接
        client.Close();
    }
}