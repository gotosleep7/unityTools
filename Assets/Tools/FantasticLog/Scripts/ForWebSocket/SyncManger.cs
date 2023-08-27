using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;

namespace FantasticLog
{
    public class SyncManger : MonoBehaviour
    {
        [Range(0.01f, 10f)]
        [SerializeField] private float updateRate;

        [SerializeField] List<bool> syncEnable;
        [SerializeField] List<WsSyncData> syncDatas;
        // [SerializeField] SyncDataForMat syncDataForMat;


        private float timer;
        // Start is called before the first frame update

        StringBuilder sb = new();
        private void Update()
        {
            timer += Time.deltaTime;
            if (timer >= updateRate)
            {
                sb.Clear();

                for (int i = 0; i < syncEnable.Count; i++)
                {
                    if (syncEnable[i])
                    {
                        syncDatas[i].TryConvertInfo(out string message);
                        sb.Append(message).Append("\r\n");
                    }
                }
                timer = 0;
                if (sb.Length > 0) WsLogLogic.Instance.SendWsMessage(sb.ToString());
            }
            // if (syncEnable)
            // {
            //     timer += Time.deltaTime;
            //     if (timer >= updateRate)
            //     {
            //         if (TryConvertInfo(out string message))
            //         {
            //         }
            //         if (TryConvertInfoForMat(out string messageForMat))
            //         {
            //             sb.Append(messageForMat).Append("\r\n");
            //         }
            //         timer = 0;
            //     }
            // }
        }



        // StringBuilder dataStr = new StringBuilder();
        // StringBuilder contentBuilder = new StringBuilder();
        // StringBuilder argsBuilder = new StringBuilder();
        // public bool TryConvertInfoForMat(out string message)
        // {

        //     dataStr.Clear();
        //     // dataStr.Append($"{targetUser}|1|");
        //     // xx,xx,xx ^B materialName ^B fieldName : valueType : value ^C fieldName : valueType : value ^A  
        //     // "Sample,Capsule^BBlue^BSample,Cube^BBlue^B_BaseColor:C:0.8862745,0.02175328,0.003921544,1^C_Smoothness:F:0.76_BaseColor:C:0.8862745,0.003921544,0.7761298,1^C_Smoothness:F:0.11"
        //     contentBuilder.Clear();
        //     foreach (SyncDataForMatItem syncDataForMatItem in syncDataForMat.data)
        //     {
        //         if (syncDataForMatItem.enableSync)
        //         {
        //             if (contentBuilder.Length > 0) contentBuilder.Append("^A");
        //             contentBuilder.Append(syncDataForMatItem.path).Append("^B");
        //             if (syncDataForMatItem.args.Count > 0)
        //             {
        //                 argsBuilder.Clear();
        //                 foreach (var arg in syncDataForMatItem.args)
        //                 {
        //                     if (!arg.enable) continue;
        //                     if (argsBuilder.Length > 0) argsBuilder.Append("^C");
        //                     argsBuilder.Append($"{arg.fieldName}:{arg.valueType}:{arg.GetValue()}");
        //                 }
        //                 if (argsBuilder.Length > 0)
        //                     contentBuilder.Append(argsBuilder);
        //                 else
        //                     contentBuilder.Clear();
        //             }
        //         }
        //     }
        //     if (contentBuilder.Length < 1)
        //     {
        //         message = "";
        //         return false;
        //     }
        //     else
        //     {
        //         dataStr.Append($"{targetUser}|1|").Append(contentBuilder);
        //         message = dataStr.ToString();
        //         return true;
        //     }
        // }

    }
}