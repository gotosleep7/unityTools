using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Progress;

public class SaveRuntimeMaterialData : MonoBehaviour
{
    [SerializeField] SaveRuntimeMatDataConfig config;
    [SerializeField] String SaveMatPath = "Assets/";

    [SerializeField] Material syncMat;
    [SerializeField] GameObject syncTarget;



    [SerializeField] Dictionary<string, Material> DicMat;

    //string[] allMatPropertyNamesForFloat;
    // Start is called before the first frame update
    void Start()
    {

        DicMat = new Dictionary<string, Material>();
        // allMatPropertyNames = mr.sharedMaterial.GetPropertyNames(MaterialPropertyType.Int);
        UpdateTempData();

    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnDestroy()
    {

    }


    List<GameObject> GetAllChildrenGameObject(GameObject root)
    {
        List<GameObject> result = new List<GameObject>();
        for (int i = 0; i < root.transform.childCount; i++)
        {
            GameObject child = root.transform.GetChild(i).gameObject;
            result.Add(child);

            result.AddRange(GetAllChildrenGameObject(child));
        }

        return result;
    }


#if UNITY_EDITOR

    [ContextMenu("SaveMaterials")]

    public void SaveMaterial()
    {
        if (config.dicMat.Count != 0)
        {
            foreach (var paths in config.dicMat.Values)
            {

                foreach (var path in paths)
                {
                    Material existingMat = AssetDatabase.LoadAssetAtPath<Material>(path);
                    if (existingMat)
                    {
                        AssetDatabase.DeleteAsset(path);
                    }
                    AssetDatabase.CreateAsset(Material.Instantiate(DicMat[path]), path);
                    AssetDatabase.SaveAssets();
                }
            }
        }
    }


    [ContextMenu("SaveRuntimeChange")]
    void UpdateTempData()
    {
        List<GameObject> tempGameObjectList = GetAllChildrenGameObject(transform.gameObject);
        config.AllGameObjectInRoot = tempGameObjectList;
        foreach (GameObject child in tempGameObjectList)
        {
            HandlerMaterialProperties(child);
            //������������Ŀ��޸ı���

        }
    }

    void HandlerMaterialProperties(GameObject gameObject)
    {
        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            Material[] materials = meshRenderer.materials;
            List<string> paths = new List<string>();
            foreach (var material in materials)
            {
                Debug.Log("��֤�Ƿ���Mat===" + material.color);
                string path = SaveMatPath + material.name.Replace(" (Instance)", "") + ".mat";
                // Debug.Log("��û��Mat" + materials.Length);
                paths.Add(path);
                DicMat[path] = material;
            }
            config.dicMat[gameObject] = paths;

        }
    }
    // 加上物体的名字是为了防止，后面的材质把前面的材质在保存时，覆盖掉。
    // 但是加上物体的名字，在应用的时候，后面的材质把前面的材质覆盖掉。



    [ContextMenu("ApplyConfig")]
    public void ApplyConfig()
    {

        foreach (var item in config.AllGameObjectInRoot)
        {
            MeshRenderer meshRenderer = item.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                Debug.Log(item.name);
                foreach (var path in config.dicMat[item])
                {
                    var m = AssetDatabase.LoadAssetAtPath<Material>(path);
                    item.GetComponent<MeshRenderer>().sharedMaterial.CopyMatchingPropertiesFromMaterial(m);

                }
            }
        }

    }
#endif
}
