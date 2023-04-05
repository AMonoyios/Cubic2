using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class BlockDataManager : MonoSingleton<BlockDataManager>
{
    [SerializeField]
    private BlockDataScriptableObject blocksData;

    public static float TextureOffset => 0.001f;
    public static Vector2 TileSize { get; private set; }
    private static Dictionary<BlockType, TextureData> blocksTextureDataDict = new();
    public static Dictionary<BlockType, TextureData> BlocksTextureDataDict => blocksTextureDataDict;

    private void Awake()
    {
        foreach (TextureData block in blocksData.textures)
        {
            blocksTextureDataDict[block.type] = block;
        }

        TileSize = new(blocksData.textureSize.x, blocksData.textureSize.y);
    }
}
