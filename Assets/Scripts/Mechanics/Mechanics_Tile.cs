using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mechanics_Tile : MonoBehaviour
{
    [SerializeField][HideInInspector]
    Color flipColor;
    Color lastColor;

    public void Initialize(Color color, Color flipColor)
    {
        this.GetComponent<SpriteRenderer>().color = color;
        this.flipColor = flipColor;
    }

    public void FlipColor()
    {
        this.lastColor = this.GetComponent<SpriteRenderer>().color;
        this.GetComponent<SpriteRenderer>().color = this.flipColor;
        this.flipColor = this.lastColor;
        if (this.GetComponent<SpriteRenderer>().sortingOrder == 2)
            this.GetComponent<SpriteRenderer>().sortingOrder = 0;
        else
            this.GetComponent<SpriteRenderer>().sortingOrder = 2;
    }
}
