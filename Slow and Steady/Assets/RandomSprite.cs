using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSprite : MonoBehaviour
{
    public Sprite[] possibleSprites; 

    private void Start()
    {
        AssignRandomSprites();
    }

    private void AssignRandomSprites()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && possibleSprites.Length > 0)
        {
            spriteRenderer.sprite = possibleSprites[Random.Range(0, possibleSprites.Length)];
        }
    }
}