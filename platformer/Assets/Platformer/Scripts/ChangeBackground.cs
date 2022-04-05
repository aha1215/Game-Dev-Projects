using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBackground : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public Sprite sprite;
    
    public void UpdateSprite()
    {
        spriteRenderer.sprite = sprite;
    }
}
