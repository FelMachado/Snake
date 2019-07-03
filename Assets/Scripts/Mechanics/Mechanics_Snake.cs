using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mechanics_Snake : MonoBehaviour
{
    [SerializeField] [HideInInspector]
    GameObject snakePartPrefab = null, explosionPrefab = null;

    List<GameObject> snakeParts = new List<GameObject>();

    List<Vector2> directionHistory = new List<Vector2>(), positionHistory = new List<Vector2>();

    Manager_GameOver managerGameOver;
    Manager_Sounds managerSounds;

    Mechanics_Grid grid;

    bool cantMove;

    Vector2 position, direction, directionBuffer;

    #region GETS & SETS

    public List<GameObject> SnakeParts
    {
        get { return this.snakeParts; }
    }

    public List<Vector2> PositionHistory
    {
        get { return this.positionHistory; }
        set { this.positionHistory = value; }
    }

    public Vector2 Position
    {
        get { return this.position; }
        set { this.position = value; }
    }

    public Vector2 Direction
    {
        get { return this.direction; }
        set { this.direction = value; }
    }
    public Vector2 DirectionBuffer
    {
        set { this.directionBuffer = value; }
    }

    public bool CantMove
    {
        set { this.cantMove = value; }
    }

    #endregion

    void Awake()
    {
        this.grid = GameObject.FindObjectOfType<Mechanics_Grid>();
        this.managerGameOver = GameObject.FindObjectOfType<Manager_GameOver>();
        this.managerSounds = GameObject.FindObjectOfType<Manager_Sounds>();
    }

    private void Update()
    {
        if (!this.managerGameOver.GameOver && this.snakeParts.Count > 0)
            ControlSnake();
    }

    void ControlSnake()
    {
        if (Input.GetButtonDown("Horizontal"))
            this.directionBuffer = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
        if (Input.GetButtonDown("Vertical"))
            this.directionBuffer = new Vector2(0, Input.GetAxisRaw("Vertical"));
        if (!this.cantMove)
        {
            if (directionBuffer.x != 0 && this.direction.x == 0)
            {
                this.cantMove = true;
                this.direction = new Vector2(directionBuffer.x, 0);
                this.managerSounds.PlaySound(1, 1, 0.7f + this.direction.x * 0.18f);
            }
            if (directionBuffer.y != 0 && this.direction.y == 0)
            {
                this.cantMove = true;
                this.direction = new Vector2(0, directionBuffer.y);
                this.managerSounds.PlaySound(1, 1, 0.7f + this.direction.y * 0.3f);
            }
        }
    }

    public void SetPosition(Vector2 pos, Vector2 dir)
    {
        this.position = pos;
        AddPart(pos, true, dir);
        if(!this.managerGameOver.GameOver)
            BackDecrease();
        TurnCells();
    }

    public void AddPart(Vector2 pos, bool front, Vector2 dir)
    {
        if (pos.x >= this.grid.GridSize.x || pos.x < 0 || pos.y < 0 || pos.y >= this.grid.GridSize.y || this.positionHistory.Contains(new Vector2(pos.x,pos.y)))
            this.managerGameOver.EndGame();
        else
        {
            GameObject part = GameObject.Instantiate(this.snakePartPrefab, this.grid.TileGrid[(int)pos.x, (int)pos.y].transform.position, Quaternion.identity, this.transform);
            if (front)
            {
                this.snakeParts.Add(part);
                this.directionHistory.Add(dir);
                this.positionHistory.Add(pos);
            }
            else
            {
                this.snakeParts.Insert(0, part);
                this.directionHistory.Insert(0, dir);
                this.positionHistory.Insert(0, pos);
            }
        }
    }

    void TurnCells()
    {
        for (int i = 0; i < this.snakeParts.Count; i++)
        {
            int choosenSprite = 0, choosenAngle = 0;
            if (this.directionHistory.Count > i + 1 && this.directionHistory[i + 1] != this.directionHistory[i] && i != 0)
            {
                choosenSprite = 1;
                if ((directionHistory[i].x == 0 && directionHistory[i].y == 1 && directionHistory[i + 1].x == -1 && directionHistory[i + 1].y == 0) ||
                    (directionHistory[i].x == 1 && directionHistory[i].y == 0 && directionHistory[i + 1].x == 0 && directionHistory[i + 1].y == -1))
                    choosenAngle = -90;
                if ((directionHistory[i].x == 0 && directionHistory[i].y == -1 && directionHistory[i + 1].x == -1 && directionHistory[i + 1].y == 0) ||
                    (directionHistory[i].x == 1 && directionHistory[i].y == 0 && directionHistory[i + 1].x == 0 && directionHistory[i + 1].y == 1))
                    choosenAngle = -180;
                if ((directionHistory[i].x == 0 && directionHistory[i].y == -1 && directionHistory[i + 1].x == 1 && directionHistory[i + 1].y == 0) ||
                    (directionHistory[i].x == -1 && directionHistory[i].y == 0 && directionHistory[i + 1].x == 0 && directionHistory[i + 1].y == 1))
                    choosenAngle = -270;
            }
            else
            {
                Vector2 directions = new Vector2(directionHistory[i].x, directionHistory[i].y);
                if (i == 0)
                {
                    choosenSprite = 3;
                    directions = new Vector2(directionHistory[i + 1].x, directionHistory[i + 1].y);
                }
                else if (i < this.directionHistory.Count - 1) choosenSprite = 0; else choosenSprite = 2;
                if (directions.x != 0)
                    choosenAngle = -90 + (1 - (int)directions.x) / 2 * -180;
                if (directions.y != 0)
                    choosenAngle = (1 - (int)directions.y) / 2 * -180;
            }
            this.snakeParts[i].GetComponent<Mechanics_SnakePart>().ChangeSprite(choosenSprite, choosenAngle);
        }

    }
    
    public void BackIncrease()
    {
        AddPart((Vector2)this.positionHistory[0] - this.directionHistory[0], false, this.directionHistory[0]);
        TurnCells();
    }

    void BackDecrease()
    {
        Destroy(this.snakeParts[0]);
        this.snakeParts.RemoveAt(0);
        this.positionHistory.RemoveAt(0);
        this.directionHistory.RemoveAt(0);
    }

    public IEnumerator DestroySnake()
    {
        int snakeSize = this.snakeParts.Count;
        GameObject.Instantiate(this.explosionPrefab, this.grid.FoodDestroy(), Quaternion.identity);
        
        for (int i = 0; i < snakeSize; i++)
        {
            yield return new WaitForSeconds(0.05f);
            this.managerSounds.PlaySound(3, 1, 0.6f);
            GameObject.Instantiate(this.explosionPrefab, this.snakeParts[0].transform.position, Quaternion.identity);
            BackDecrease();
        }
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(this.grid.SpinGrid());
    }

    public void SnakeFadeIn()
    {
        for (int i = 0; i < this.snakeParts.Count; i++)
            this.snakeParts[i].GetComponent<Animator>().SetTrigger("Appear");
    }
}
