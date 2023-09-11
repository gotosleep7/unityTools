using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


[CreateAssetMenu(fileName = "{SaveRuntimMatDataConfig}", menuName = "FantasticLog/SaveRuntimMatDataConfig", order = 0)]
public class SaveRuntimeMatDataConfig : ScriptableObject
{
    public Dictionary<GameObject, List<string>> dicMat = new Dictionary<GameObject, List<string>>();

    public List<GameObject> AllGameObjectInRoot = new List<GameObject>();
}




