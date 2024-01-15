using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "KeyDataConfigSO", menuName = "BMW/KeyDataConfigSO", order = 0)]
public class KeyDataConfigSO : ScriptableObject
{
    public List<KeyDataSO> list;
}