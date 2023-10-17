using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;

namespace FDebugTools
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

        Dictionary<int, WsSyncData> dict = new();

        StringBuilder sb = new();
        StringBuilder message = new();
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
                        message.Clear();
                        syncDatas[i].TryConvertInfo(ref message);
                        sb.Append(message);
                        if (!sb.ToString().Equals("")) sb.Append("\r\n");
                        Debug.Log(sb.ToString());
                    }

                }
                timer = 0;
                if (sb.Length > 0) WsLogLogic.Instance.SendWsMessage(sb.ToString());
            }
        }

    }


}

