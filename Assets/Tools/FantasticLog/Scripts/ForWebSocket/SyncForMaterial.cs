// using System.Collections;
// using System.Collections.Generic;
// using System.Text;
// using UnityEngine;
// using UnityEngine.Profiling;
// using UnityEngine.UI;


// namespace FantasticLog
// {
//     public enum ValueType
//     {
//         Float = 0, Color
//     }
//     public class SyncForMaterial : MonoBehaviour
//     {
//         public string targetUser;
//         // public string path;
//         // public ValueType valueType;
//         // public string fieldName;
//         // public string materialName;
//         // public string value;
//         public SyncDataForMat syncDataForMat;
//         // public Color color;
//         [Range(0.01f, 10)]
//         public float updateRate;
//         public bool isSync;
//         // private string messageType = "1";
//         float timer;
//         private void Start()
//         {
//         }
//         private void Update()
//         {
//             if (isSync)
//             {
//                 timer += Time.deltaTime;
//                 if (timer >= updateRate)
//                 {
//                     if (TryConvertInfoForMat(out string data))
//                     {
//                         // Debug.Log("SyncForMaterial");
//                         // WsLogLogic.Instance.SendWsMessage(data);
//                     }
//                     timer = 0;
//                 }
//             }
//         }
//         StringBuilder dataStr = new StringBuilder();
//         StringBuilder contentBuilder = new StringBuilder();
//         StringBuilder argsBuilder = new StringBuilder();
//         public bool TryConvertInfoForMat(out string message)
//         {

//             dataStr.Clear();
//             // dataStr.Append($"{targetUser}|1|");
//             // xx,xx,xx ^B materialName ^B fieldName : valueType : value ^C fieldName : valueType : value ^A  
//             // "Sample,Capsule^BBlue^BSample,Cube^BBlue^B_BaseColor:C:0.8862745,0.02175328,0.003921544,1^C_Smoothness:F:0.76_BaseColor:C:0.8862745,0.003921544,0.7761298,1^C_Smoothness:F:0.11"
//             contentBuilder.Clear();
//             foreach (SyncDataForMatItem syncDataForMatItem in syncDataForMat.data)
//             {
//                 if (syncDataForMatItem.enableSync)
//                 {
//                     if (contentBuilder.Length > 0) contentBuilder.Append("^A");
//                     contentBuilder.Append(syncDataForMatItem.path).Append("^B");
//                     if (syncDataForMatItem.args.Count > 0)
//                     {
//                         argsBuilder.Clear();
//                         foreach (var arg in syncDataForMatItem.args)
//                         {
//                             if (!arg.enable) continue;
//                             if (argsBuilder.Length > 0) argsBuilder.Append("^C");
//                             argsBuilder.Append($"{arg.fieldName}:{arg.valueType}:{arg.GetValue()}");
//                         }
//                         if (argsBuilder.Length > 0)
//                             contentBuilder.Append(argsBuilder);
//                         else
//                             contentBuilder.Clear();
//                     }
//                 }
//             }
//             if (contentBuilder.Length < 1)
//             {
//                 message = "";
//                 return false;
//             }
//             else
//             {
//                 dataStr.Append($"{targetUser}|1|").Append(contentBuilder);
//                 message = dataStr.ToString();
//                 return true;
//             }
//         }
//     }
// }