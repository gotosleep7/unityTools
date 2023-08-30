using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace FantasticLog
{
    [Serializable]
    public enum SyncParamType
    {
        PFloat = 1, PInt, PString, PV3, PV2
    }
    [Serializable]
    public class SyncDataParam
    {
        public bool enable = true;
        public SyncParamType type;
        public float floatValue;
        public int intValue;
        public string stringValue;
        public Vector3 v3;
        public string GetValue()
        {
            string ret = "";
            switch (type)
            {
                case SyncParamType.PFloat:
                    ret = floatValue.ToString();
                    break;
                case SyncParamType.PInt:
                    ret = intValue.ToString();
                    break;
                case SyncParamType.PString:
                    ret = stringValue;
                    break;
                case SyncParamType.PV3:
                    ret = $"{v3.x},{v3.y},{v3.z}";
                    break;
                case SyncParamType.PV2:
                    ret = $"{v3.x},{v3.y}";
                    break;
            }

            return ret;
        }
    }


    [Serializable]
    public class SyncDataItem
    {
        public string path = "";
        public string className = "";
        public string methodName = "";
        public bool enable = false;
        [SerializeField] public List<SyncDataParam> args;
    }


    [Serializable]
    [CreateAssetMenu(fileName = "{SyncData}", menuName = "FantasticLog/SyncData", order = 0)]
    public class SyncData : WsSyncData
    {
        public string targetUser;
        public List<SyncDataItem> dataList;
        private StringBuilder content = new();
        StringBuilder args = new();
        public override bool TryConvertInfo(ref StringBuilder ret)
        {
            content.Clear();
            //  "userid|1|
            // xxx,xx,xx^Aclass.fullname^Amethodname^A0:1,0:1^B
            // xxx,xx,xx^Aclass.fullname^Amethodname^A0:1,0:1
            ret.Append(targetUser).Append("|").Append(2).Append("|");
            foreach (SyncDataItem data in dataList)
            {
                if (!data.enable) continue;
                if (content.Length > 0) content.Append("^A");
                content.Append(data.path).Append("^B").Append(data.className).Append("^B").Append(data.methodName).Append("^B");
                args.Clear();
                foreach (var arg in data.args)
                {
                    if (!arg.enable) continue;
                    if (args.Length > 0) args.Append("^C");
                    args.Append($"{arg.type}:{arg.GetValue()}");
                    // args.Append(arg.type).Append(":").Append(arg.GetValue());
                }
                if (args.Length > 0)
                    content.Append(args);
                else
                    content.Clear();
            }
            if (content.Length > 0)
            {
                ret.Append(content);
                return true;
            }
            else
            {
                ret.Clear();
                return false;
            }
        }
    }


    public class WsSyncData : ScriptableObject
    {
        public virtual bool TryConvertInfo(ref StringBuilder retStr)
        {
            return false;
        }
    }
}