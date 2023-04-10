using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class World : MonoSingleton<World>
{
    [Header("World properties")]
    [SerializeField]
    private ushort worldInChunks = 16;

    [Header("Chunk properties")]
    [SerializeField]
    private ushort size = 16;
    public ushort Size => size;
    [SerializeField]
    private ushort height = 128;
    public ushort Height => height;
    [SerializeField]
    private ushort waterLevel = 64;
    [SerializeField]
    private float noiseScale = 0.03f;
    [SerializeField]
    private GameObject chunkPrefab;

    private readonly Dictionary<Vector3Int, ChunkData> chunkDataDictionary = new();
    private readonly Dictionary<Vector3Int, ChunkRenderer> chunkRendererDictionary = new();

    public void GenerateWorld()
    {
        chunkDataDictionary.Clear();
        foreach (ChunkRenderer chunkRenderer in chunkRendererDictionary.Values)
        {
            Destroy(chunkRenderer.gameObject);
        }
        chunkRendererDictionary.Clear();

        for (int x = 0; x < worldInChunks; x++)
        {
            for (int z = 0; z < worldInChunks; z++)
            {
                ChunkData chunkData = new(size, height, this, new(x * size, 0, z * size));

                GenerateVoxels(chunkData);
                chunkDataDictionary.Add(chunkData.WorldPosition, chunkData);
            }
        }

        foreach (ChunkData chunkData in chunkDataDictionary.Values)
        {
            MeshData meshData = Chunk.GetChunkMeshData(chunkData);
            GameObject chunkGameObject = Instantiate(chunkPrefab, chunkData.WorldPosition, Quaternion.identity, transform);
            ChunkRenderer chunkRenderer = chunkGameObject.GetComponent<ChunkRenderer>();

            chunkRendererDictionary.Add(chunkData.WorldPosition, chunkRenderer);

            chunkRenderer.Init(chunkData);
            chunkRenderer.UpdateChunk(meshData);
        }

        void GenerateVoxels(ChunkData chunkData)
        {
            for (int x = 0; x < chunkData.Size; x++)
            {
                for (int z = 0; z < chunkData.Size; z++)
                {
                    float noiseValue = Mathf.PerlinNoise((chunkData.WorldPosition.x + x) * noiseScale, (chunkData.WorldPosition.z + z) * noiseScale);
                    int groundPosition = Mathf.RoundToInt(noiseValue * height);

                    for (int y = 0; y < height; y++)
                    {
                        // World generation logic here

                        BlockType blockType = BlockType.Dirt;
                        if (y > groundPosition)
                        {
                            if (y < waterLevel)
                            {
                                blockType = BlockType.Water;
                            }
                            else
                            {
                                blockType = BlockType.Air;
                            }
                        }
                        else if (y == groundPosition)
                        {
                            blockType = BlockType.GrassyDirt;
                        }

                        Chunk.SetBlock(chunkData, new(x, y, z), blockType);
                    }
                }
            }
        }
    }

    public BlockType GetBlockFromChunkCoordinates(Vector3Int position)
    {
        Vector3Int pos = Chunk.GetPositionFromBlockCoordinates(position);

        chunkDataDictionary.TryGetValue(pos, out ChunkData containerChunk);
        if (containerChunk == null)
        {
            return BlockType.Null;
        }
        Vector3Int chunkPositionOfBlock = Chunk.GetBlockInChunkCoordinates(containerChunk, position);

        return Chunk.GetBlockFromChunkCoordinates(containerChunk, chunkPositionOfBlock);
    }
}
