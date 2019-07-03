using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mechanics_SnakePart : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer spriteRenderer = null;
    [SerializeField]
    List<Sprite> snakeSprites = null;

    public void ChangeSprite(int spriteId, int angle)
    {
        this.spriteRenderer.sprite = this.snakeSprites[spriteId];
        this.transform.localRotation = Quaternion.Euler(0, 0, angle);
    }
}
