using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager_Game : MonoBehaviour
{
    [SerializeField][HideInInspector]
    GameObject eatenApplePrefab = null;

    [SerializeField][HideInInspector]
    Text scoreText = null;

    Manager_GameOver managerGameOver;
    Manager_Sounds managerSounds;

    Mechanics_Snake snake;
    Mechanics_Grid grid;

    [SerializeField]
    int initialSize = 0;
    int score = 0;

    [SerializeField]
    float baseSpeed = 0, difficultyStep = 0, maximumSpeed = 0;
    float gameSpeed = 0;

    #region GETS & SETS

    public int Score
    {
        get { return this.score; }
    }

    #endregion

    private void Start()
    {
        this.gameSpeed = baseSpeed;
        this.managerGameOver = GameObject.FindObjectOfType<Manager_GameOver>();
        this.managerSounds = GameObject.FindObjectOfType<Manager_Sounds>();
        this.grid = GameObject.FindObjectOfType<Mechanics_Grid>();
        this.snake = GameObject.FindObjectOfType<Mechanics_Snake>();
    }

    private void Update()
    {
        GameStartButton();
    }

    void GameStartButton()
    {
        if (Input.GetKeyDown(KeyCode.Return) && this.managerGameOver.GameOver && this.grid.FlippingFinished)
        {
            this.managerGameOver.GameOver = false;
            this.gameSpeed = baseSpeed;
            this.score = 0;
            StartCoroutine(this.grid.SpinGrid());
        }
    }

    public void ResetGame()
    {
        this.scoreText.text = "Score: " + score.ToString();
        this.grid.FoodSpawn();
        SnakeSpawn();
        StartCoroutine(SnakeWalk());
    }

    void SnakeSpawn()
    {
        Vector2 initialPos = new Vector2(Random.Range((int)this.initialSize, (int)this.grid.GridSize.x - this.initialSize),
                                         Random.Range(this.initialSize, (int)this.grid.GridSize.y - this.initialSize));
        Vector2 initialDirection;
        if (Random.Range(0, 2) == 0)
            initialDirection = new Vector2(Libraries_Formulas.GetDirection((int)initialPos.x, (int)this.grid.GridSize.x/2), 0);
        else
            initialDirection = new Vector2(0, Libraries_Formulas.GetDirection((int)initialPos.y, (int)this.grid.GridSize.y / 2));

        this.snake.Direction = this.snake.DirectionBuffer = initialDirection;
        this.snake.Position = initialPos;
        this.snake.AddPart(initialPos, true, initialDirection);
        for (int i = 1; i < this.initialSize; i++)
            this.snake.AddPart(initialPos - initialDirection * i, false, initialDirection);
        this.snake.SnakeFadeIn();
    }

    IEnumerator SnakeWalk()
    {
        yield return new WaitForSeconds(this.gameSpeed);
        this.snake.SetPosition(this.snake.Position + this.snake.Direction, this.snake.Direction);
        this.snake.CantMove = false;
        FoodCheck();
        if(!this.managerGameOver.GameOver)
            StartCoroutine(SnakeWalk());
    }

    void FoodCheck()
    {
        if (this.snake.Position == this.grid.FoodPosition)
        {
            this.score++;
            this.scoreText.text = "Score: " + score.ToString();
            GameObject.Instantiate(this.eatenApplePrefab, this.grid.TileGrid[(int)this.snake.Position.x, (int)this.snake.Position.y].transform.position, Quaternion.identity);
            this.managerSounds.PlaySound(0, 1, Random.Range(0.75f, 1f));
            this.scoreText.transform.parent.GetComponent<Animator>().SetTrigger("Shake");
            this.snake.BackIncrease();
            if(this.grid.HasSpaceForFood())
                this.grid.FoodSpawn();
            this.gameSpeed = Mathf.Clamp(this.gameSpeed - this.difficultyStep, this.maximumSpeed, this.gameSpeed);
        }
    }


}
