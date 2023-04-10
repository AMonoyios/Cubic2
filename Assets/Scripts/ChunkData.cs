using UnityEngine;

public sealed class ChunkData
{
    public BlockType[] blocks;

    public ushort Size { get; }
    public ushort Height { get; }
    public World World { get; }
    public Vector3Int WorldPosition { get; }

    public bool modifiedByPlayer = false;

    public ChunkData(ushort size, ushort height, World world, Vector3Int worldPosition)
    {
        Size = size;
        Height = height;
        World = world;
        WorldPosition = worldPosition;

        blocks = new BlockType[size * height * size];
    }
}
