using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BlockHelper
{
    private static Direction[] directions =
    {
        Direction.back,
        Direction.down,
        Direction.front,
        Direction.left,
        Direction.right,
        Direction.up
    };

    public static Vector2Int TexturePosition(Direction direction, BlockType blockType)
    {
        return direction switch
        {
            Direction.up => BlockDataManager.BlocksTextureDataDict[blockType].up,
            Direction.down => BlockDataManager.BlocksTextureDataDict[blockType].down,
            _ => BlockDataManager.BlocksTextureDataDict[blockType].side
        };
    }

    public static Vector2[] FaceUVs(Direction direction, BlockType blockType)
    {
        Vector2[] uvs = new Vector2[4];
        Vector2Int tilePosition = TexturePosition(direction, blockType);

        uvs[0] = new
        (
            x: (BlockDataManager.TileSize.x * tilePosition.x) + BlockDataManager.TileSize.x - BlockDataManager.TextureOffset,
            y: (BlockDataManager.TileSize.y * tilePosition.y) + BlockDataManager.TextureOffset
        );

        uvs[1] = new
        (
            x: (BlockDataManager.TileSize.x * tilePosition.x) + BlockDataManager.TileSize.x - BlockDataManager.TextureOffset,
            y: (BlockDataManager.TileSize.y * tilePosition.y) + BlockDataManager.TextureOffset - BlockDataManager.TextureOffset
        );

        uvs[2] = new
        (
            x: (BlockDataManager.TileSize.x * tilePosition.x) + BlockDataManager.TextureOffset,
            y: (BlockDataManager.TileSize.y * tilePosition.y) + BlockDataManager.TextureOffset - BlockDataManager.TextureOffset
        );

        uvs[3] = new
        (
            x: (BlockDataManager.TileSize.x * tilePosition.x) + BlockDataManager.TextureOffset,
            y: (BlockDataManager.TileSize.y * tilePosition.y) + BlockDataManager.TextureOffset
        );

        return uvs;
    }

    public static void GetFaceVertices(Direction direction, Vector3Int position, MeshData meshData, BlockType blockType)
    {
        const float verticeOffset = 0.5f;
        bool hasColliders = BlockDataManager.BlocksTextureDataDict[blockType].isCollidable;

        switch (direction)
        {
            case Direction.back:
            {
                meshData.AddVertex(new(position.x - verticeOffset, position.y - verticeOffset, position.z - verticeOffset), hasColliders);
                meshData.AddVertex(new(position.x - verticeOffset, position.y + verticeOffset, position.z - verticeOffset), hasColliders);
                meshData.AddVertex(new(position.x + verticeOffset, position.y + verticeOffset, position.z - verticeOffset), hasColliders);
                meshData.AddVertex(new(position.x + verticeOffset, position.y - verticeOffset, position.z - verticeOffset), hasColliders);
                break;
            }
            case Direction.front:
            {
                meshData.AddVertex(new(position.x + verticeOffset, position.y - verticeOffset, position.z + verticeOffset), hasColliders);
                meshData.AddVertex(new(position.x + verticeOffset, position.y + verticeOffset, position.z + verticeOffset), hasColliders);
                meshData.AddVertex(new(position.x - verticeOffset, position.y + verticeOffset, position.z + verticeOffset), hasColliders);
                meshData.AddVertex(new(position.x - verticeOffset, position.y - verticeOffset, position.z + verticeOffset), hasColliders);
                break;
            }
            case Direction.left:
            {
                meshData.AddVertex(new(position.x - verticeOffset, position.y - verticeOffset, position.z + verticeOffset), hasColliders);
                meshData.AddVertex(new(position.x - verticeOffset, position.y + verticeOffset, position.z + verticeOffset), hasColliders);
                meshData.AddVertex(new(position.x - verticeOffset, position.y + verticeOffset, position.z - verticeOffset), hasColliders);
                meshData.AddVertex(new(position.x - verticeOffset, position.y - verticeOffset, position.z - verticeOffset), hasColliders);
                break;
            }
            case Direction.right:
            {
                meshData.AddVertex(new(position.x + verticeOffset, position.y - verticeOffset, position.z - verticeOffset), hasColliders);
                meshData.AddVertex(new(position.x + verticeOffset, position.y + verticeOffset, position.z - verticeOffset), hasColliders);
                meshData.AddVertex(new(position.x + verticeOffset, position.y + verticeOffset, position.z + verticeOffset), hasColliders);
                meshData.AddVertex(new(position.x + verticeOffset, position.y - verticeOffset, position.z + verticeOffset), hasColliders);
                break;
            }
            case Direction.down:
            {
                meshData.AddVertex(new(position.x - verticeOffset, position.y - verticeOffset, position.z - verticeOffset), hasColliders);
                meshData.AddVertex(new(position.x + verticeOffset, position.y - verticeOffset, position.z - verticeOffset), hasColliders);
                meshData.AddVertex(new(position.x + verticeOffset, position.y - verticeOffset, position.z + verticeOffset), hasColliders);
                meshData.AddVertex(new(position.x - verticeOffset, position.y - verticeOffset, position.z + verticeOffset), hasColliders);
                break;
            }
            case Direction.up:
            {
                meshData.AddVertex(new(position.x - verticeOffset, position.y + verticeOffset, position.z + verticeOffset), hasColliders);
                meshData.AddVertex(new(position.x + verticeOffset, position.y + verticeOffset, position.z + verticeOffset), hasColliders);
                meshData.AddVertex(new(position.x + verticeOffset, position.y + verticeOffset, position.z - verticeOffset), hasColliders);
                meshData.AddVertex(new(position.x - verticeOffset, position.y + verticeOffset, position.z - verticeOffset), hasColliders);
                break;
            }
        }
    }

    public static MeshData GetMeshData(ChunkData chunk, Vector3Int position, MeshData meshData, BlockType blockType)
    {
        if (blockType == BlockType.Air)
        {
            return meshData;
        }

        foreach (Direction direction in directions)
        {
            Vector3Int neighbourBlockPosition = position + direction.GetVector();
            BlockType neighbourBlockType = chunk.GetBlockFromChunkPosition(neighbourBlockPosition);

            if (neighbourBlockType != BlockType.Air && !BlockDataManager.BlocksTextureDataDict[neighbourBlockType].isSolid)
            {
                if (blockType == BlockType.Water)
                {
                    if (neighbourBlockType == BlockType.Air)
                    {
                        meshData.waterMeshData = GetFaceData(direction, chunk, position, meshData.waterMeshData, blockType);
                    }
                    else
                    {
                        meshData = GetFaceData(direction, chunk, position, meshData, blockType);
                    }
                }
            }
        }

        return meshData;
    }

    public static MeshData GetFaceData(Direction direction, ChunkData chunk, Vector3Int position, MeshData meshData, BlockType blockType)
    {
        GetFaceVertices(direction, position, meshData, blockType);

        meshData.AddQuadTriangles(BlockDataManager.BlocksTextureDataDict[blockType].isCollidable);
        meshData.uvs.AddRange(FaceUVs(direction, blockType));

        return meshData;
    }
}
