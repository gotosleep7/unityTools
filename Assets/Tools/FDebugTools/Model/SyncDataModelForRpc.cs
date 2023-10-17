
using System;
using System.Collections.Generic;
using UnityEngine;
namespace FDebugTools
{
    public class SyncDataModel
    {
        public string user;
        public int msgType;
        public string contentStr;

        public SyncDataModel(string dataStr)
        {
            string[] messages = dataStr.Split("|");
            user = messages[0];
            msgType = int.Parse(messages[1]);
            contentStr = messages[2];
        }



    }
    public class SyncDataModelForRpc
    {

        public SyncDataModelContent[] dataContents;

        public static bool TryConvert(string contentStr, out SyncDataModelForRpc syncDataModel)
        {

            syncDataModel = new SyncDataModelForRpc();
            List<SyncDataModelContent> contentList = new List<SyncDataModelContent>();

            string[] contentArr, path, parames, param;
            string classFunllName, methodName, pType, pValue;
            // string[] messages = syncData.Split("|");
            // if (messages.Length < 2) return false;
            // syncDataModel.user = messages[0];
            // syncDataModel.msgType = int.Parse(messages[1]);
            // path,classname,parm1^Cparm1^B ^B  ^A
            // string contentStr = syncData;
            string[] allContent = contentStr.Split("^A");
            foreach (var content in allContent)
            {
                SyncDataModelContent syncDataModelContent = new();
                contentArr = content.Split("^B");
                if ("".Equals(content) || contentArr.Length < 3) continue;
                path = contentArr[0].Split(",");
                classFunllName = contentArr[1];
                methodName = contentArr[2];
                parames = contentArr[3]?.Split("^C");
                syncDataModelContent.classFullName = contentArr[1];
                syncDataModelContent.methodName = methodName;
                syncDataModelContent.sourcePath = contentArr[0];
                syncDataModelContent.path = syncDataModelContent.sourcePath.Split(",");
                syncDataModelContent.parmas = new object[parames.Length];
                syncDataModelContent.parmaTypes = new Type[parames.Length];

                for (int i = 0; i < parames.Length; i++)
                {
                    param = parames[i].Split(":");
                    if (param.Length < 2) continue;
                    pType = param[0];
                    pValue = param[1];
                    switch (Enum.Parse<SyncParamType>(pType))
                    {
                        case SyncParamType.PFloat:
                            if (float.TryParse(pValue, out float parsedFloat))
                            {
                                syncDataModelContent.parmas[i] = parsedFloat;
                                syncDataModelContent.parmaTypes[i] = typeof(float);
                            }
                            break;
                        case SyncParamType.PInt:
                            if (int.TryParse(pValue, out int parsedInt))
                            {
                                syncDataModelContent.parmas[i] = parsedInt;
                                syncDataModelContent.parmaTypes[i] = typeof(int);
                            }
                            break;
                        case SyncParamType.PString:
                            syncDataModelContent.parmas[i] = pValue;
                            syncDataModelContent.parmaTypes[i] = typeof(string);
                            break;
                        case SyncParamType.PV3:
                            string[] xyz = pValue.Split(",");
                            if (xyz.Length != 3) break;
                            syncDataModelContent.parmas[i] = new Vector3(float.Parse(xyz[0]), float.Parse(xyz[1]), float.Parse(xyz[2]));
                            syncDataModelContent.parmaTypes[i] = typeof(Vector3);
                            break;
                        case SyncParamType.PV2:
                            string[] xy = pValue.Split(",");
                            if (xy.Length < 2) break;
                            syncDataModelContent.parmas[i] = new Vector2(float.Parse(xy[0]), float.Parse(xy[1]));
                            syncDataModelContent.parmaTypes[i] = typeof(Vector2);
                            break;
                    }
                }
                contentList.Add(syncDataModelContent);
            }
            syncDataModel.dataContents = contentList.ToArray();

            return true;
        }
    }





    public class SyncDataModelContent
    {
        public string classFullName;
        public string methodName;
        public string[] path;
        public string sourcePath;
        public object[] parmas;
        public Type[] parmaTypes;
    }

}