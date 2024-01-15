// using System;
// using System.Net.Sockets;
// using UnityEngine;

// public class SocketClientManager : MonoBehaviour
// {
//     public static SocketClientManager Instance { get; private set; }
//     private static SocketClient socketClient;
//     TcpClient tcpClient;
//     public string address = "127.0.0.1";
//     public int port = 9999;
//     NetworkStream writeStream;
//     float heartTimer;
//     float heartTimerMax = 5f;
//     void Awake()
//     {
//         Instance = this;
//     }

//     [ContextMenu("openSocket")]
//     public void ToOpenSocket()
//     {
//         socketClient?.Close();
//         socketClient = null;
//         socketClient = new SocketClient(address, port);
//         socketClient.StartListenForServer();
//     }
//     [ContextMenu("closeSocket")]
//     public void ToCloseSocket()
//     {
//         socketClient?.Close();
//         socketClient = null;
//         // CloseSocket();
//     }
//     public void SendMessageToServer(string message)
//     {

//         try
//         {
//             if (socketClient != null && socketClient.Connected)
//                 socketClient?.Send(message);
//         }
//         catch (Exception e)
//         {
//             Debug.LogError($"Error sending message: {e.Message}");
//         }
//     }


//     private void Update()
//     {
//         if (socketClient == null) return;
//         if (ClientMsgQueue.Instance.TryGetMsg(out string msg))
//         {
//             Debug.Log(msg);
//         }

//         heartTimer -= Time.deltaTime;
//         if (heartTimer < 0)
//         {
//             socketClient.Send("ping");
//             heartTimer = heartTimerMax;
//         }
//     }
//     private void OnDestroy()
//     {
//         writeStream?.Close();
//     }
// }


