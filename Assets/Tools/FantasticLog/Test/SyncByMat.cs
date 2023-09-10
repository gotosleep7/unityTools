using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncByMat : MonoBehaviour
{
    [SerializeField] MeshRenderer mr;
    Material material;

    string[] allFloatPropertyNames;
    string[] allIntPropertyNames;
    string[] allColorPropertyNames;
    string[] allVectorPropertyNames;
    List<KeyValuePair<string, object>> matDatasForColor = new List<KeyValuePair<string, object>>();
    List<KeyValuePair<string, object>> matDatasForInt = new List<KeyValuePair<string, object>>();
    List<KeyValuePair<string, object>> matDatasForFloat = new List<KeyValuePair<string, object>>();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ReadMat();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            CheckDiff(matDatasForColor, 0);
            CheckDiff(matDatasForInt, 1);
            CheckDiff(matDatasForFloat, 2);
        }
    }
    [ContextMenu("ReadMat")]
    public void ReadMat()
    {
        // mr = GetComponent<MeshRenderer>();
        // material = new Material(mr.material);
        // mr.material = material;
        // allFloatPropertyNames = material.GetPropertyNames(MaterialPropertyType.Float);
        // allIntPropertyNames = material.GetPropertyNames(MaterialPropertyType.Int);
        // allVectorPropertyNames = material.GetPropertyNames(MaterialPropertyType.Vector);

        allFloatPropertyNames = mr.sharedMaterial.GetPropertyNames(MaterialPropertyType.Float);
        allIntPropertyNames = mr.sharedMaterial.GetPropertyNames(MaterialPropertyType.Int);
        allVectorPropertyNames = mr.sharedMaterial.GetPropertyNames(MaterialPropertyType.Vector);

        foreach (var item in allFloatPropertyNames)
        {
            matDatasForFloat.Add(new KeyValuePair<string, object>(item, mr.sharedMaterial.GetFloat(item)));
        }
        foreach (var item in allIntPropertyNames)
        {
            matDatasForInt.Add(new KeyValuePair<string, object>(item, mr.sharedMaterial.GetInt(item)));
        }
        foreach (var item in allVectorPropertyNames)
        {
            matDatasForColor.Add(new KeyValuePair<string, object>(item, mr.sharedMaterial.GetColor(item)));
        }
    }

    public void CheckDiff(List<KeyValuePair<string, object>> list, int type)
    {
        List<KeyValuePair<int, object>> dirtyList = new();
        for (int i = 0; i < list.Count; i++)
        {
            object newValue = null;
            object oldValue = list[i].Value;
            switch (type)
            {
                case 0:
                    newValue = mr.sharedMaterial.GetColor(list[i].Key);
                    break;
                case 1:
                    newValue = mr.sharedMaterial.GetInt(list[i].Key);
                    break;
                case 2:
                    newValue = mr.sharedMaterial.GetFloat(list[i].Key);
                    break;
            }
            if (!newValue.Equals(oldValue))
            {
                dirtyList.Add(new KeyValuePair<int, object>(i, newValue));
            }
        }

        foreach (var item in dirtyList)
        {
            object value = item.Value;
            list[item.Key] = new KeyValuePair<string, object>(list[item.Key].Key, value);
            Debug.Log(list[item.Key].Key + "，发生了改变");
        }

    }
}
