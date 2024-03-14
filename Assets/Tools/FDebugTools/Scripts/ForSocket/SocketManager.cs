using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using System.Threading;

public class SocketManager : MonoBehaviour
{
    public static SocketManager Instance;
    public UnityEvent<string> OnReceive;
    SocketServer socketServer;
    [SerializeField] private string address = "127.0.0.1";
    [SerializeField] private int port = 8989;
    void Awake()
    {
        Instance = this;
        // OpenSocketServer("127.0.0.1", 8989);
    }
    public void OpenSocketServer()
    {
        socketServer = new SocketServer(address, port);
        socketServer.StartListenForClients();

    }
    void Update()
    {
        while (MsgQueue.Instance.TryGetMsg(out string msg))
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
    public void PushMessageToClient(string message)
    {
        socketServer?.PushMessageToClient(message);
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


