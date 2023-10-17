


using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


namespace FDebugTools
{
    [Serializable]
    public class SyncDataParamForMat
    {
        public bool enable = true;
        public string fieldName;
        public MaterialContentParamType valueType;
        public float fvalue;
        public Color color;
        public string GetValue()
        {
            string ret = "";
            switch (valueType)
            {
                case MaterialContentParamType.F:
                    ret = fvalue.ToString();
                    break;
                case MaterialContentParamType.C:
                    ret = $"{color.r},{color.g},{color.b},{color.a}";
                    break;
            }
            return ret;
        }
    }





    [Serializable]
    public class SyncDataForMatItem
    {
        public bool enableSync = false;
        public string path = "";
        public List<SyncDataParamForMat> args;
    }



    [Serializable]
    [CreateAssetMenu(fileName = "{SyncDataForMat}", menuName = "FantasticLog/SyncDataForMat", order = 0)]
    public class SyncDataForMat : WsSyncData
    {
        public string targetUser;
        public List<SyncDataForMatItem> dataList;

        StringBuilder contentBuilder = new StringBuilder();
        StringBuilder argsBuilder = new StringBuilder();

        public override bool TryConvertInfo(ref StringBuilder dataStr)
        {
            contentBuilder.Clear();
            argsBuilder.Clear();

            dataStr.Clear();
            // dataStr.Append($"{targetUser}|1|");
            // xx,xx,xx ^B materialName ^B fieldName : valueType : value ^C fieldName : valueType : value ^A  
            // "Sample,Capsule^BBlue^BSample,Cube^BBlue^B_BaseColor:C:0.8862745,0.02175328,0.003921544,1^C_Smoothness:F:0.76_BaseColor:C:0.8862745,0.003921544,0.7761298,1^C_Smoothness:F:0.11"
            contentBuilder.Clear();
            foreach (SyncDataForMatItem syncDataForMatItem in dataList)
            {
                if (syncDataForMatItem.enableSync)
                {
                    if (contentBuilder.Length > 0) contentBuilder.Append("^A");
                    contentBuilder.Append(syncDataForMatItem.path).Append("^B");
                    if (syncDataForMatItem.args.Count > 0)
                    {
                        argsBuilder.Clear();
                        foreach (var arg in syncDataForMatItem.args)
                        {
                            if (!arg.enable) continue;
                            if (argsBuilder.Length > 0) argsBuilder.Append("^C");
                            argsBuilder.Append($"{arg.fieldName}:{arg.valueType}:{arg.GetValue()}");
                        }
                        if (argsBuilder.Length > 0)
                            contentBuilder.Append(argsBuilder);
                        else
                            contentBuilder.Clear();
                    }
                }
            }
            if (contentBuilder.Length < 1)
            {
                dataStr.Clear();
                return false;
            }
            else
            {
                dataStr.Append($"{targetUser}|1|").Append(contentBuilder);
                return true;
            }
        }
    }



}