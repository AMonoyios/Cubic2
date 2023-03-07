using UnityEngine;

[CreateAssetMenu(fileName = "Block Data", menuName = "Tools/New Block Data")]
public class BlockDataScriptableObject : ScriptableObject
{
    public Vector2 textureSize;
    public TextureData[] textures;
}

[System.Serializable]
public class TextureData
{
    public BlockType type = BlockType.Null;
    public Vector2Int up, down, side;
    public bool isSolid = true;
    public bool isCollidable = true;
}
