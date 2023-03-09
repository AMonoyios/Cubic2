using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class BlockDataManager : MonoSingleton<BlockDataManager>
{
    [SerializeField]
    private BlockDataScriptableObject blocksData;

    public static float TextureOffset => 0.001f;
    public static Vector2 TileSize { get; private set; }
    public static Dictionary<BlockType, TextureData> BlocksTextureDataDictionary { get; private set; }

    private void Start()
    {
        BlocksTextureDataDictionary = new();
    }

    private void Awake()
    {
        foreach (TextureData block in blocksData.textures)
        {
            if (!BlocksTextureDataDictionary.ContainsKey(block.type))
            {
                BlocksTextureDataDictionary.Add(block.type, block);
            }
        }

        TileSize = new(blocksData.textureSize.x, blocksData.textureSize.y);
    }
}
