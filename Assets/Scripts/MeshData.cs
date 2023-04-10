using System.Collections.Generic;
using UnityEngine;

public sealed class MeshData
{
    public List<Vector3> vertices = new();
    public List<int> triangles = new();
    public List<Vector2> uvs = new();

    public List<Vector3> collidableVertices = new();
    public List<int> collidableTriangles = new();

    public MeshData waterMeshData;
    private bool isMainMesh = true;

    public MeshData(bool isMainMeshData)
    {
        if (isMainMeshData)
        {
            waterMeshData = new(false);
        }
    }

    public void AddVertex(Vector3 vertex, bool generatesCollider)
    {
        vertices.Add(vertex);
        if (generatesCollider)
        {
            collidableVertices.Add(vertex);
        }
    }

    public void AddQuadTriangles(bool generatesCollider)
    {
        triangles.Add(vertices.Count - 4);
        triangles.Add(vertices.Count - 3);
        triangles.Add(vertices.Count - 2);

        triangles.Add(vertices.Count - 4);
        triangles.Add(vertices.Count - 2);
        triangles.Add(vertices.Count - 1);

        if (generatesCollider)
        {
            collidableTriangles.Add(collidableVertices.Count - 4);
            collidableTriangles.Add(collidableVertices.Count - 3);
            collidableTriangles.Add(collidableVertices.Count - 2);
            collidableTriangles.Add(collidableVertices.Count - 4);
            collidableTriangles.Add(collidableVertices.Count - 2);
            collidableTriangles.Add(collidableVertices.Count - 1);
        }
    }
}
