using System;
using UnityEngine;

public static class Chunk
{
    public static void LoopThroughTheBlocks(ChunkData chunkData, Action<Vector3Int> action)
    {
        for (int i = 0; i < chunkData.blocks.Length; i++)
        {
            Vector3Int position = GetPositionFromIndex(chunkData, i);
            action?.Invoke(position);
        }

        static Vector3Int GetPositionFromIndex(ChunkData chunkData, int index)
        {
            return new
            (
                x: index % chunkData.Size,
                y: index / chunkData.Size % chunkData.Height,
                z: index / (chunkData.Size * chunkData.Height)
            );
        }
    }

    private static int GetIndexFromPosition(ChunkData chunkData, Vector3Int position)
    {
        return position.x + (chunkData.Size * position.y) + (chunkData.Size * chunkData.Height * position.z);
    }

    private static bool InRange(ChunkData chunkData, int axis)
    {
        return axis >= 0 && axis < chunkData.Size;
    }

    private static bool InHeightRange(ChunkData chunkData, int height)
    {
        return height >= 0 && height < chunkData.Height;
    }

    public static Vector3Int GetBlockInChunkCoordinates(ChunkData chunkData, Vector3Int position)
    {
        return position - chunkData.WorldPosition;
    }

    public static BlockType GetBlockFromChunkCoordinates(ChunkData chunkData, Vector3Int chunkCoordinates)
    {
        if (InRange(chunkData, chunkCoordinates.x) && InHeightRange(chunkData, chunkCoordinates.y) && InRange(chunkData, chunkCoordinates.z))
        {
            int index = GetIndexFromPosition(chunkData, chunkCoordinates);
            return chunkData.blocks[index];
        }

        return chunkData.World.GetBlockFromChunkCoordinates(chunkData.WorldPosition + chunkCoordinates);
    }

    public static Vector3Int GetPositionFromBlockCoordinates(Vector3Int position)
    {
        return new
        (
            x: Mathf.FloorToInt(position.x / (float)World.Instance.Size) * World.Instance.Size,
            y: Mathf.FloorToInt(position.y / (float)World.Instance.Height) * World.Instance.Height,
            z: Mathf.FloorToInt(position.z / (float)World.Instance.Size) * World.Instance.Size
        );
    }

    public static void SetBlock(ChunkData chunkData, Vector3Int localPosition, BlockType block)
    {
        if (InRange(chunkData, localPosition.x) && InHeightRange(chunkData, localPosition.y) && InRange(chunkData, localPosition.z))
        {
            int index = GetIndexFromPosition(chunkData, localPosition);
            chunkData.blocks[index] = block;
        }
        else
        {
            throw new Exception("Need to ask World for correct chunk!");
        }
    }

    public static MeshData GetChunkMeshData(ChunkData chunkData)
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
