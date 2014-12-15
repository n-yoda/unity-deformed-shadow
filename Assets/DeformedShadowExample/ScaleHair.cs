using UnityEngine;
using System.Collections;

public class ScaleHair : MonoBehaviour
{
    void Start()
    {
        var filters = GetComponentsInChildren<MeshFilter>();
        foreach (var filter in filters) {
            if (filter.name.Contains("HairTail")) {
                var mesh = filter.mesh;
                var verts = mesh.vertices;
                for (int i = 0; i < verts.Length; i ++) {
                    verts[i] = new Vector3(verts[i].x, verts[i].y * 8, verts[i].z * 8);
                }
                mesh.vertices = verts;
            }
        }
    }
}
