
using System;
using System.Collections.Generic;

namespace FantasticLog
{
    public class SyncDataModelForMaterial
    {
        public SyncDataModelForMatContent[] dataContents;


        public static bool TryConvert(string contentStr, out SyncDataModelForMaterial syncDataModelForMaterial)
        {
            syncDataModelForMaterial = new SyncDataModelForMaterial();
            List<SyncDataModelForMatContent> contentList = new();
            // string[] dataArr = dataStr.Split("|");
            // syncDataModelForMaterial.user = dataArr[0];
            // syncDataModelForMaterial.msgType = int.Parse(dataArr[1]);

            // string contentStr = dataArr[2];
            string[] allContent = contentStr.Split("^A");
            // xx,xx,xx ^B materialName ^B fieldName : valueType : value ^C fieldName : valueType : value ^A  
            foreach (string content in allContent)
            {
                SyncDataModelForMatContent syncDataModelMaterialContent = new SyncDataModelForMatContent();
                string[] contentArr = content.Split("^B");
                syncDataModelMaterialContent.sourcePath = contentArr[0];
                syncDataModelMaterialContent.path = syncDataModelMaterialContent.sourcePath.Split(",");
                string[] allParam = contentArr[1].Split("^C");
                List<SyncDataModelForMatContentParam> paramList = new();
                for (int i = 0; i < allParam.Length; i++)
                {
                    string paramStr = allParam[i];
                    SyncDataModelForMatContentParam syncDataModelMaterialContentParam = new();
                    string[] paramArr = paramStr.Split(":");
                    syncDataModelMaterialContentParam.fieldName = paramArr[0];
                    syncDataModelMaterialContentParam.valueType = Enum.Parse<MaterialContentParamType>(paramArr[1]);
                    syncDataModelMaterialContentParam.value = paramArr[2];
                    paramList.Add(syncDataModelMaterialContentParam);
                }
                syncDataModelMaterialContent.paramArr = paramList.ToArray();
                contentList.Add(syncDataModelMaterialContent);
            }

            syncDataModelForMaterial.dataContents = contentList.ToArray();
            return true;
        }
    }

    public class SyncDataModelForMatContent
    {
        public string sourcePath;
        public string[] path;
        public string materialName;

        public SyncDataModelForMatContentParam[] paramArr;

    }
    public enum MaterialContentParamType
    {
        F = 1, C
    }
    public class SyncDataModelForMatContentParam
    {
        public string fieldName;
        public MaterialContentParamType valueType;
        public string value;
    }
}