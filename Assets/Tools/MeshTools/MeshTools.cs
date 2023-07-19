using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTools : MonoBehaviour
{
    private Mesh SplicingMesh(List<Mesh> meshList)
    {
        var tempMesh = new UnityEngine.Mesh();
        var vertices = new List<Vector3>();
        var uv = new List<Vector2>();
        var triangles = new List<int>();
        var normals = new List<Vector3>();
        int offset = 0;
        for (int i = 0; i < meshList.Count; i++)
        {
            var mesh = meshList[i];
            var ts = mesh.triangles;
            if (i != 0) offset = vertices.Count;
            if (offset > 0)
            {
                for (int j = 0; j < ts.Length; j++)
                {
                    // 拼接的时候需要offset，
                    ts[j] += offset;
                }
            }
            triangles.AddRange(ts);
            vertices.AddRange(mesh.vertices);
            uv.AddRange(mesh.uv);
            normals.AddRange(normals);
        }
        tempMesh.vertices = vertices.ToArray();
        tempMesh.normals = normals.ToArray();
        tempMesh.uv = uv.ToArray();
        tempMesh.triangles = triangles.ToArray();
        return tempMesh;
    }

}
