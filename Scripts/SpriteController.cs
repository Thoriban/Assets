using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SpriteController : MonoBehaviour
{
    public static bool LoadArt(GameObject icon, string filepath)
    {
        Texture2D spriteTexture = LoadTexture(filepath);

        if (spriteTexture != null)
        {
            Sprite pokemonSprite = Sprite.Create(spriteTexture, new Rect(0, 0, spriteTexture.width, spriteTexture.height), new Vector2(0, 0), 100, 0, SpriteMeshType.Tight);

            Image image = icon.GetComponent<Image>();
            image.sprite = pokemonSprite;

            return true;
        }

        return false;
    }

    static Texture2D LoadTexture(string filePath)
    {
        Texture2D texture2D;
        byte[] fileData;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            texture2D = new Texture2D(1080, 1080);

            if (texture2D.LoadImage(fileData))
            {
                return texture2D;
            }
        }

        return null;
    }
}
