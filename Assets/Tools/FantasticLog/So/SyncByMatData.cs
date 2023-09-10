using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SyncByMatData", menuName = "FantasticLog/SyncByMatData", order = 0)]
public class SyncByMatData : ScriptableObject
{
    [SerializeField] public List<SyncByMatDataItem> matData = new();
}

[Serializable]
public class SyncByMatDataItem
{
    public string key;
    public object value;
}
