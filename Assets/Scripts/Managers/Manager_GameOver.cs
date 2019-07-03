using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_GameOver : MonoBehaviour
{
    Manager_Game managerGame;
    Manager_Sounds managerSounds;
    Manager_Score managerScore;

    Mechanics_Grid grid;
    Mechanics_Snake snake;

    bool gameOver = true;

    #region GETS & SETS

    public bool GameOver
    {
        get { return this.gameOver; }
        set { this.gameOver = value; }
    }

    #endregion

    void Start()
    {
        this.managerScore = GameObject.FindObjectOfType<Manager_Score>();
        this.managerGame = GameObject.FindObjectOfType<Manager_Game>();
        this.managerSounds = GameObject.FindObjectOfType<Manager_Sounds>();
        this.snake = GameObject.FindObjectOfType<Mechanics_Snake>();
        this.grid = GameObject.FindObjectOfType<Mechanics_Grid>();
    }

    public void EndGame()
    {
        this.managerSounds.PlaySound(2, 1, 0.8f);
        this.gameOver = true;
        this.managerScore.SubmitScore(this.managerGame.Score);
        this.managerScore.PrintScores();
        StartCoroutine(this.snake.DestroySnake());
    }
}
