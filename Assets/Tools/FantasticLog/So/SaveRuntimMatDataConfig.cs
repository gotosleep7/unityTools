using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


[CreateAssetMenu(fileName = "{SaveRuntimMatDataConfig}", menuName = "FantasticLog/SaveRuntimMatDataConfig", order = 0)]
public class SaveRuntimMatDataConfig : ScriptableObject
{

    public Dictionary<string, BaseSaveRuntimeData> dict = new Dictionary<string, BaseSaveRuntimeData> { };
}


public class BaseSaveRuntimeData
{
    public SaveRuntimeDataParamType type;
    public virtual object GetValue()
    {
        return null;
    }
}
[Serializable]
public class SaveRuntimeDataForColor : BaseSaveRuntimeData
{
    public Color value;
    public override object GetValue()
    {
        return value;
    }
}
[Serializable]
public class SaveRuntimeDataForFloat : BaseSaveRuntimeData
{
    public float value;
    public override object GetValue()
    {
        return value;
    }
}
public enum SaveRuntimeDataParamType
{
    F, C
}