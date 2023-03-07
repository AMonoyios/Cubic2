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

    public static void SetBlock(this ChunkData chunkData, Vector3Int position, BlockType block)
    {
        if (InChunkRange(chunkData, position))
        {
            int index = GetIndexFromPosition(chunkData, position);
            chunkData.blocks[index] = block;
        }
    }

    public static Vector3Int GetBlockInChunk(this ChunkData chunkData, Vector3Int position)
    {
        return new
        (
            x: position.x - chunkData.Position.x,
            y: position.y - chunkData.Position.y,
            z: position.z - chunkData.Position.z
        );
    }

    public static BlockType GetBlockFromChunk(this ChunkData chunkData, Vector3Int position)
    {
        if (InChunkRange(chunkData, position))
        {
            int index = GetIndexFromPosition(chunkData, position);
            return chunkData.blocks[index];
        }

        throw new Exception("Block type is not in chunk range.");
    }

    public static MeshData GetChunkMeshData(this ChunkData chunkData)
    {
        MeshData meshData = new(true);

        return meshData;
    }
}
