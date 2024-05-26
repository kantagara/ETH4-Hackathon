using System;
using UnityEngine;

[Serializable]
public class SpriteData
{
    public ResourceType Type;
    public Sprite Sprite;
}

public class SpritesManager : Singleton<SpritesManager>
{
    [SerializeField] private SpriteData[] sprites;

    public Sprite GetSprite(ResourceType type)
    {
        foreach (var spriteData in sprites)
        {
            if (spriteData.Type == type)
            {
                return spriteData.Sprite;
            }
        }

        return null;
    }
}