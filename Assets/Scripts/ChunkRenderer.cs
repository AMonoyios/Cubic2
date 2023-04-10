using UnityEditor;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public sealed class ChunkRenderer : MonoBehaviour
{
    private MeshFilter meshFilter;
    private MeshCollider meshCollider;
    private Mesh mesh;

    [SerializeField]
    private bool showGizmos = false;

    public ChunkData ChunkData { get; private set; }

    public bool ModifiedByPlayer
    {
        get
        {
            return ChunkData.modifiedByPlayer;
        }
        set
        {
            ChunkData.modifiedByPlayer = value;
        }
    }

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
        mesh = meshFilter.mesh;
    }

    public void Init(ChunkData chunkData)
    {
        ChunkData = chunkData;
    }

    private void RenderMesh(MeshData meshData)
    {
        mesh.Clear();
        mesh.subMeshCount = 2;
        mesh.vertices = meshData.vertices.Concat(meshData.waterMeshData.vertices).ToArray();

        mesh.SetTriangles(meshData.triangles.ToArray(), 0);
        mesh.SetTriangles(meshData.waterMeshData.triangles.Select(val => val + meshData.vertices.Count).ToArray(), 1);

        mesh.uv = meshData.uvs.Concat(meshData.waterMeshData.uvs).ToArray();
        mesh.RecalculateNormals();

        meshCollider.sharedMesh = null;
        Mesh collisionMesh = new()
        {
            vertices = meshData.collidableVertices.ToArray(),
            triangles = meshData.collidableTriangles.ToArray()
        };
        collisionMesh.RecalculateNormals();
        meshCollider.sharedMesh = collisionMesh;
    }

    public void UpdateChunk()
    {
        RenderMesh(Chunk.GetChunkMeshData(ChunkData));
    }

    public void UpdateChunk(MeshData meshData)
    {
        RenderMesh(meshData);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (showGizmos && Application.isPlaying && ChunkData != null)
        {
            Gizmos.color = Selection.activeObject == gameObject ? Color.green / 2.0f : Color.red / 2.0f;
            Gizmos.DrawCube(transform.position + new Vector3(ChunkData.Size / 2, ChunkData.Height / 2, ChunkData.Size / 2), new(ChunkData.Size, ChunkData.Height, ChunkData.Size));
        }
    }
#endif
}
