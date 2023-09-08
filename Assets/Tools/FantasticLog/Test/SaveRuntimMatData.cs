using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveRuntimMatData : MonoBehaviour
{
    [SerializeField] SaveRuntimMatDataConfig config;
    [SerializeField] MeshRenderer mr;
    [SerializeField] Material material;

    string[] allMatPropertyNamesForFloat;
    // Start is called before the first frame update
    void Start()
    {
        material = new Material(mr.material);
        mr.material = material;
        allMatPropertyNamesForFloat = mr.sharedMaterial.GetPropertyNames(MaterialPropertyType.Float);
        // allMatPropertyNames = mr.sharedMaterial.GetPropertyNames(MaterialPropertyType.Int);
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnDestroy()
    {
        foreach (var matName in allMatPropertyNamesForFloat)
        {
            config.dict[matName] = new SaveRuntimeDataForFloat() { value = material.GetFloat(matName) };
        }
        Debug.Log("destory");
    }
    [ContextMenu("ApplyConfig")]
    public void ApplyConfig()
    {
        foreach (var key in config.dict.Keys)
        {
            var info = config.dict[key];
            mr.sharedMaterial.SetFloat(key, (float)info.GetValue());
            // switch (item.)
            // {
            //     case SaveRuntimeDataParamType.C:
            //         mr.sharedMaterial.SetColor(item.key, item.colorValue);
            //         break;
            // }
        }
        // mr.sharedMaterial.GetColor("item.key");



        // foreach (var item in config.dataList)
        // {
        //     switch (item.type)
        //     {
        //         case SaveRuntimeDataParamType.C:
        //             mr.sharedMaterial.SetColor(item.key, item.colorValue);
        //             break;
        //         case SaveRuntimeDataParamType.F:
        //             mr.sharedMaterial.SetFloat(item.key, float.Parse(item.value));
        //             break;
        //     }
        // }

    }
}
