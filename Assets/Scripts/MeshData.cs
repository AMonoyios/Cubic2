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

    public MeshData(bool isMainMeshData)
    {
        if (isMainMeshData)
        {
            waterMeshData = new(false);
        }
    }

    public void AddVertex(Vector3 vertex, bool hasCollision)
    {
        vertices.Add(vertex);
        if (hasCollision)
        {
            collidableVertices.Add(vertex);
        }
    }

    public void AddQuadTriangles(bool hasCollision)
    {
        triangles.Add(vertices.Count - 4);
        triangles.Add(vertices.Count - 3);
        triangles.Add(vertices.Count - 2);

        triangles.Add(vertices.Count - 4);
        triangles.Add(vertices.Count - 2);
        triangles.Add(vertices.Count - 1);

        if (hasCollision)
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
