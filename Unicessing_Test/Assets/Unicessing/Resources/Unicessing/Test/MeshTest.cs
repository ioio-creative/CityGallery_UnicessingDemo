// Meshを毎フレーム生成する場合の解放処理の比較テスト

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTest : MonoBehaviour
{
    public enum Mode
    {
        None,
        Clear,
        Destroy,
        DestroyImmediate,
        DestroyImmediateSetDarty,
        UnloadUnusedAssets,
    }

    [SerializeField] Mode mode = Mode.Clear;
    [SerializeField, Range(0, 1000)] int VertexCount = 100;
    [SerializeField] Material material;

    Mesh mesh;

    void Start()
    {
        mesh = new Mesh();
        mesh.name = "TestMesh";
    }

    void Update()
    {
        ClearMesh();
        SetMesh();
        Graphics.DrawMesh(mesh, Vector3.zero, Quaternion.identity, material, 0);
    }

    void SetMesh()
    {
        Vector3[] vertices = new Vector3[VertexCount];
        int[] indices = new int[VertexCount];
        for (int i = 0; i < VertexCount; i++)
        {
            vertices[i] = Random.insideUnitSphere;
            indices[i] = i;
        }
        mesh.vertices = vertices;
        mesh.SetIndices(indices, MeshTopology.LineStrip, 0);
    }

    void ClearMesh()
    {
        switch (mode)
        {
            case Mode.None:
                mesh = null;
                break;
            case Mode.Clear:
                mesh.Clear();
                break;
            case Mode.Destroy:
                Destroy(mesh);
                mesh = null;
                break;
            case Mode.DestroyImmediate:
                DestroyImmediate(mesh, true);
                mesh = null;
                break;
            case Mode.DestroyImmediateSetDarty:
                #if UNITY_EDITOR 
                UnityEditor.EditorUtility.SetDirty(mesh);
                #endif
                DestroyImmediate(mesh, true);
                mesh = null;
                break;
            case Mode.UnloadUnusedAssets:
                mesh = null;
                Resources.UnloadUnusedAssets();
                break;
        }

        if (!mesh)
        {
            mesh = new Mesh();
            mesh.name = "TestMesh " + Time.frameCount;
        }
    }
}
