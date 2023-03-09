using System;
using UnityEngine;

public static class Chunk
{
    public static void LoopThroughTheBlocks(this ChunkData chunkData, Action<Vector3Int> action)
    {
        for (int i = 0; i < chunkData.blocks.Length; i++)
        {
            Vector3Int position = GetPositionFromIndex(chunkData, i);
            action?.Invoke(position);
        }
    }

    private static Vector3Int GetPositionFromIndex(ChunkData chunkData, int index)
    {
        return new
        (
            x: index % chunkData.Size,
            y: index / chunkData.Size % chunkData.Height,
            z: index / (chunkData.Size * chunkData.Height)
        );
    }

    private static int GetIndexFromPosition(ChunkData chunkData, Vector3Int position)
    {
        return position.x + (chunkData.Size * position.y) + (chunkData.Size * chunkData.Height * position.z);
    }

    private static bool InChunkRange(ChunkData chunkData, Vector3Int position)
    {
        if (InAxisRange(chunkData, position.x) && InHeightRange(chunkData, position.y) && InAxisRange(chunkData, position.z))
        {
            return true;
        }
        else
        {
            Debug.LogError("Need to ask World for correct chunk!");
            return false;
        }
    }

    private static bool InAxisRange(ChunkData chunkData, int axis)
    {
        return axis >= 0 && axis < chunkData.Size;
    }

    private static bool InHeightRange(ChunkData chunkData, int height)
    {
        return height >= 0 && height < chunkData.Height;
    }

    public static Vector3Int GetBlockInChunkPosition(this ChunkData chunkData, Vector3Int position)
    {
        return position - chunkData.Position;
    }

    public static BlockType GetBlockFromChunkPosition(this ChunkData chunkData, Vector3Int position)
    {
        if (InChunkRange(chunkData, position))
        {
            int index = GetIndexFromPosition(chunkData, position);
            return chunkData.blocks[index];
        }

        return chunkData.World.GetBlockFromChunk(chunkData.Position + position);
    }

    public static Vector3Int GetChunkPositionFromBlock(Vector3Int position)
    {
        return new
        (
            x: Mathf.FloorToInt(position.x / (float)World.Instance.Size) * World.Instance.Size,
            y: Mathf.FloorToInt(position.y / (float)World.Instance.Height) * World.Instance.Height,
            z: Mathf.FloorToInt(position.z / (float)World.Instance.Size) * World.Instance.Size
        );
    }

    public static void SetBlock(this ChunkData chunkData, Vector3Int position, BlockType block)
    {
        if (InChunkRange(chunkData, position))
        {
            int index = GetIndexFromPosition(chunkData, position);
            chunkData.blocks[index] = block;
        }
    }

    public static MeshData GetChunkMeshData(this ChunkData chunkData)
    {
        MeshData meshData = new(true);

        LoopThroughTheBlocks
        (
            chunkData,
            (position) => meshData = BlockHelper.GetMeshData
                (
                    chunkData,
                    position,
                    meshData,
                    chunkData.blocks[GetIndexFromPosition
                        (
                            chunkData,
                            position
                        )]
                )
        );

        return meshData;
    }
}
