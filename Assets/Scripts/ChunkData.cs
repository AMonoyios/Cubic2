using UnityEngine;

public class ChunkData
{
    public BlockType[] blocks;
    public byte Size { get; }
    public byte Height { get; }
    public World World { get; }
    public Vector3Int Position { get; }

    public bool modifiedByPlayer = false;

    public ChunkData(byte size, byte height, World world, Vector3Int position)
    {
        Size = size;
        Height = height;
        World = world;
        Position = position;

        blocks = new BlockType[size * height * size];
    }
}
