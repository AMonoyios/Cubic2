using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class World : MonoSingleton<World>
{
    [SerializeField]
    private ushort worldInChunks = 16;
    [SerializeField]
    private ushort chunkSize = 16;
    public ushort Size => chunkSize;
    [SerializeField]
    private ushort chunkHeight = 128;
    public ushort Height => chunkHeight;
    [SerializeField]
    private ushort waterLevel = 64;
    [SerializeField]
    private float noiseScale = 0.03f;
    [SerializeField]
    private GameObject chunkPrefab;

    private Dictionary<Vector3Int, ChunkData> chunkDataDictionary = new();
    private Dictionary<Vector3Int, ChunkRenderer> chunkRendererDictionary = new();

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
                ChunkData chunkData = new(chunkSize, chunkHeight, this, new(x * chunkSize, 0, z * chunkSize));

                GenerateVoxels(chunkData);
                chunkDataDictionary.Add(chunkData.Position, chunkData);
            }
        }

        foreach (ChunkData chunkData in chunkDataDictionary.Values)
        {
            MeshData meshData = chunkData.GetChunkMeshData();
            GameObject chunkGameObject = Instantiate(chunkPrefab, chunkData.Position, Quaternion.identity);
            ChunkRenderer chunkRenderer = chunkGameObject.GetComponent<ChunkRenderer>();

            chunkRendererDictionary.Add(chunkData.Position, chunkRenderer);

            chunkRenderer.Init(chunkData);
            chunkRenderer.UpdateChunk(meshData);
        }
    }

    private void GenerateVoxels(ChunkData chunkData)
    {
        for (int x = 0; x < chunkData.Size; x++)
        {
            for (int z = 0; z < chunkData.Size; z++)
            {
                float noiseValue = Mathf.PerlinNoise((chunkData.Position.x + x) * noiseScale, (chunkData.Position.z + z) * noiseScale);
                int groundPosition = Mathf.RoundToInt(noiseValue * chunkHeight);

                for (int y = 0; y < chunkHeight; y++)
                {
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

                    chunkData.SetBlock(new(x, y, z), blockType);
                }
            }
        }
    }

    public BlockType GetBlockFromChunk(Vector3Int position)
    {
        Vector3Int pos = Chunk.GetChunkPositionFromBlock(position);

        chunkDataDictionary.TryGetValue(pos, out ChunkData chunk);
        if (chunk == null)
        {
            return BlockType.Air;
        }
        Vector3Int chunkPositionOfBlock = chunk.GetBlockInChunkPosition(position);

        return chunk.GetBlockFromChunkPosition(chunkPositionOfBlock);
    }
}
