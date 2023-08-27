using System.Text;
using UnityEditor;
using UnityEngine;
namespace FantasticLog
{
#if UNITY_EDITOR

    public class LogTools : Editor
    {
        [MenuItem("FantasticLog/Copy path")]
        public static void GetPath()
        {
            Transform selectedObject = Selection.activeGameObject.transform;
            Transform parent = selectedObject.parent;
            StringBuilder path = new StringBuilder(selectedObject.name);

            while (parent != null)
            {
                path.Insert(0, $"{parent.name},");
                parent = parent.parent;
            }
            EditorGUIUtility.systemCopyBuffer = path.ToString();
        }
    }
#endif
}

