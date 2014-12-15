using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class RenderShadow : MonoBehaviour
{
    public int width = 1024;
    public int height = 1024;
    public Material xBlur;
    public Material yBlur;
    public Shader shader;
    public Transform plane;
    public Vector3 planeNormal;
    public bool updateMesh;
    public RenderTexture mid;
    public UnityEngine.UI.RawImage image;

    void Start()
    {
        if (camera.targetTexture)
        {
            DestroyImmediate(camera.targetTexture);
        }
        if (mid)
        {
            DestroyImmediate(mid);
        }
        var meshFilter = GetComponent<MeshFilter>();
        if (meshFilter.sharedMesh)
        {
            DestroyImmediate(meshFilter.sharedMesh);
        }
    }

    void Update()
    {
        var rt = camera.targetTexture;
        if (rt == null || rt.width != width || rt.height != height)
        {
            if (rt)
            {
                DestroyImmediate(rt);
            }
            rt = new RenderTexture(width, height, 1, RenderTextureFormat.R8);
            camera.targetTexture = rt;

            if (mid)
            {
                DestroyImmediate(mid);
            }
        }

        if (mid == null)
        {
            mid = new RenderTexture(width, height, 1, RenderTextureFormat.R8);
            mid.filterMode = FilterMode.Point;
        }

        if (renderer.sharedMaterial == null)
        {
            renderer.sharedMaterial = new Material(shader);
        }

        if (renderer.sharedMaterial.mainTexture == null)
        {
            renderer.sharedMaterial.mainTexture = camera.targetTexture;
        }

        var meshFilter = GetComponent<MeshFilter>();
        if (meshFilter.sharedMesh == null)
        {
            meshFilter.sharedMesh = new Mesh();
            UpdateMesh(meshFilter.sharedMesh);
        }
        if (meshFilter.sharedMesh != null && updateMesh)
        {
            updateMesh = false;
            UpdateMesh(meshFilter.sharedMesh);
        }

        if (image.texture == null)
        {
            image.texture = mid;
        }
    }

    void UpdateMesh(Mesh mesh)
    {
        Plane p = new Plane(plane.rotation * planeNormal, plane.transform.position);
        var dir = transform.rotation * Vector3.forward;
        var y = transform.rotation * new Vector3(0, camera.orthographicSize, 0);
        var x = transform.rotation * new Vector3(camera.orthographicSize / height * width, 0, 0);
        var origin = transform.position;
        var p0 = RaycastPosition(p, origin - x - y, dir);
        var p1 = RaycastPosition(p, origin - x + y, dir);
        var p2 = RaycastPosition(p, origin + x + y, dir);
        var p3 = RaycastPosition(p, origin + x - y, dir);
        mesh.Clear();
        mesh.vertices = new Vector3[]{ p0, p1, p2, p3 };
        mesh.triangles = new int[]{ 0, 1, 2, 0, 2, 3 };
        mesh.uv = new Vector2[]{ Vector2.zero, Vector2.up, Vector2.up + Vector2.right, Vector2.right };
        mesh.RecalculateBounds();
    }

    Vector3 RaycastPosition(Plane p, Vector3 pos, Vector3 dir)
    {
        float result = 0f;
        p.Raycast(new Ray(pos, dir), out result);
        return transform.InverseTransformPoint(pos + dir * result);
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (xBlur && yBlur)
        {
            src.filterMode = FilterMode.Point;
            mid.DiscardContents();
            Graphics.Blit(src, mid, xBlur);
            Graphics.Blit(mid, dst, yBlur);
        }
        else
        {
            Graphics.Blit(src, dst);
        }
    }
}
