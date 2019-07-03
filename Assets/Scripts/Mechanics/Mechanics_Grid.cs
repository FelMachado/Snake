using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mechanics_Grid : MonoBehaviour
{
    [SerializeField][HideInInspector]
    Animator scoreAnimator = null;

    [SerializeField][HideInInspector]
    GameObject tilePrefab = null, foodPrefab = null, titleScreen = null, gameOverScreen = null;
    GameObject foodObject = null;

    Mechanics_Tile[,] tileGrid;
    Mechanics_Snake snake;

    Manager_GameOver managerGameOver;
    Manager_Game managerGame;
    Manager_Sounds managerSounds;

    [SerializeField]
    Vector2 gridSize = Vector2.zero;
    Vector2 foodPosition;

    [SerializeField]
    List<Color> gridColors = null, flipColors = null;

    bool flippingFinished = true;

    #region GETS & SETS

    public bool FlippingFinished
    {
        get { return this.flippingFinished; }
        set { this.flippingFinished = value; }
    }

    public Mechanics_Tile[,] TileGrid
    {
        get { return this.tileGrid; }
    }

    public Vector2 FoodPosition
    {
        get { return this.foodPosition; }
    }

    public Vector2 GridSize
    {
        get { return this.gridSize; }
    }

    #endregion

    void Awake()
    {
        this.managerSounds = GameObject.FindObjectOfType<Manager_Sounds>();
        this.managerGame = GameObject.FindObjectOfType<Manager_Game>();
        this.managerGameOver = GameObject.FindObjectOfType<Manager_GameOver>();
        this.snake = GameObject.FindObjectOfType<Mechanics_Snake>();
        MountGrid();
    }

    void MountGrid()
    {
        tileGrid = new Mechanics_Tile[(int)this.gridSize.x, (int)this.gridSize.y];
        for (int i = 0; i < this.gridSize.x; i ++)
            for(int j = 0; j < this.gridSize.y; j++)
            {
                tileGrid[i, j] = GameObject.Instantiate(this.tilePrefab, this.transform.position - (Vector3)this.gridSize / 2 * 0.5f + new Vector3(i * 0.5f + 0.5f / 2, j * 0.5f + 0.5f / 2), 
                Quaternion.identity, this.transform).GetComponent<Mechanics_Tile>();
                tileGrid[i, j].Initialize(this.gridColors[(i + j) % 2], this.flipColors[(i + j) % 2]);
                tileGrid[i, j].GetComponent<Mechanics_Tile>().FlipColor();
            }
    }

    public void FoodSpawn()
    {
        if (this.foodObject != null) Destroy(this.foodObject);

        Vector2 randomPos = new Vector2(Random.Range(0, (int)this.gridSize.x), Random.Range(0, (int)this.gridSize.y));
        while(this.snake.PositionHistory.Contains(randomPos))
            randomPos = new Vector2(Random.Range(0, (int)this.gridSize.x), Random.Range(0, (int)this.gridSize.y));

        this.foodPosition = randomPos;
        this.foodObject = GameObject.Instantiate(this.foodPrefab, tileGrid[(int)randomPos.x, (int)randomPos.y].transform.position, Quaternion.identity);
    }

    public Vector2 FoodDestroy()
    {
        Vector2 foodPos = this.foodObject.transform.position;
        Destroy(this.foodObject);
        return foodPos;
    }

    public bool HasSpaceForFood()
    {
        if (this.tileGrid.Length > this.snake.SnakeParts.Count) return true; else return false;
    }

    void ChangePostGameScreen()
    {
        if(this.titleScreen.activeSelf)
        {
            this.titleScreen.SetActive(false);
            this.gameOverScreen.SetActive(true);
            this.scoreAnimator.SetTrigger("Appear");
        }
    }

    public IEnumerator SpinGrid()
    {
        this.flippingFinished = false;
        for (int i = 0; i < this.gridSize.x; i++)
        {
            this.managerSounds.PlaySound(4, 0.5f, 0.9f);
            yield return new WaitForSeconds(0.05f);
            for (int j = 0; j < this.gridSize.y; j++)
                this.tileGrid[i, j].GetComponent<Animator>().SetTrigger("Flip");
        }
        if (!this.managerGameOver.GameOver)
            this.managerGame.ResetGame();
        else
            this.flippingFinished = true;
        ChangePostGameScreen();
    }
}
