using UnityEngine;

public sealed class ChunkData
{
    public BlockType[] blocks;
    public ushort Size { get; }
    public ushort Height { get; }
    public World World { get; }
    public Vector3Int Position { get; }

    public bool modifiedByPlayer = false;

    public ChunkData(ushort size, ushort height, World world, Vector3Int position)
    {
        Size = size;
        Height = height;
        World = world;
        Position = position;

        blocks = new BlockType[size * height * size];
    }
}
