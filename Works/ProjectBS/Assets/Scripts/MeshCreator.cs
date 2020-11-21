using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCreator : MonoBehaviour
{
    public Transform[] pos;
    //public Mesh ex;
    public Vector2[] uv;
    Mesh mesh;

    // Use this for initialization
    void Start()
    {
        Create();
    }
    private void Update()
    {
        //mesh.uv = uv;
    }

    void Create()
    {
        GameObject poly = new GameObject("Poly");
        uv = new Vector2[pos.Length];
        Vector3[] ver = new Vector3[pos.Length];
        MeshFilter meshFilter = poly.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = poly.AddComponent<MeshRenderer>();
        mesh = new Mesh();

        for (int i = 0; i < ver.Length; i++)
        {
            ver[i] = pos[i].position;
            uv[i] = pos[i].position;
        }

        mesh.vertices = ver;

        uv[0] = new Vector2(0.1f, 1);
        uv[1] = new Vector2(0.1f, 0);
        uv[2] = new Vector2(-0.9f, 0);
        uv[3] = new Vector2(0.1f, 0.55f);
        mesh.uv = uv;

        mesh.triangles = new int[] { 0, 1, 2, 2, 3, 0, 0, 4, 5, 5, 1, 0, 1, 5, 6, 6, 2, 1, 2, 6, 7, 7, 3, 2, 3, 7, 4, 4, 0, 3 };
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;
    }
}

//https://dhkvmf88.tistory.com/19
