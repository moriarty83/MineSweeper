using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BoardManager : MonoBehaviour
{
    //Sets Board Rows and Columns
    public int boardCols, boardRows;

    //Sets spacing for each tile.
    public float boardXSpace = 50, boardYSpace = 50;

    //Placement position of the first tile.
    public float xStart = 0, yStart = 0;

    public GameObject tilePrefab;

    //2D Array for x and y coordiantes and 0 if safe exists, 1 if mined.
    private bool[,] mines;
    public TileScript[,] tiles;

    //Game Objects for the Board, Start Menu and Game Over Menu
    public GameObject boardObject, gameMenu, gameOverMenu;

    public Dropdown sizeDropdown;

    private bool gameOn;
    public int totalMines = 0, totalClicked = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        //Sets Window Size to that of a 'Small' game.
        Screen.SetResolution((int)boardXSpace * 10, (int)boardYSpace * 15, false);

        //Turns on the Start Game menue, off the Game Over Menu.
        gameOverMenu.SetActive(false);
        gameMenu.SetActive(true);

    }

    public void StartGame()
    {
        if (sizeDropdown.value == 2)
        {
            setGameParameters(25, 20);
        }
        else if (sizeDropdown.value == 1)
        {
            setGameParameters(15, 18);
        }
        else
        {
            setGameParameters(10, 15);
        }

        gameMenu.SetActive(false);

        gameOn = true;
        DealTiles();
    }

    private void DealTiles()
    {
        for (int x = 0; x < boardCols; x++)
        {
            for (int y = 0; y < boardRows; y++)
            {
                if (UnityEngine.Random.Range(1, 10) == 1)
                {
                    mines[x, y] = true;
                    totalMines += 1;
                }
                else
                {
                    mines[x, y] = false;
                }


                GameObject tile = GameObject.Instantiate(tilePrefab);
                tile.transform.SetParent(boardObject.transform);
                tile.transform.localPosition = new Vector3(xStart + boardXSpace * x, yStart + (-boardYSpace * y));

                TileScript tileScript = tile.GetComponent<TileScript>();
                tileScript.xCoordinate = x;
                tileScript.yCoordinate = y;
                tiles[x, y] = tileScript;
                    
            }
        }
    }

    public int sweepMe(int x, int y)
    {
        if(gameOn == false)
        {
            return -2;
        }
        
        int mineCount = 0;
        List<TileScript> tempTiles = new List<TileScript>();

        if (mines[x, y]) {

            GameOver(false);
            return -1; }

        for (int i = x-1; i <= x+1; i++)
        {
            for (int j = y-1; j <= y+1; j++)
            {
                if (i >= 0 && i <= boardCols-1 && j >= 0 && j <= boardRows-1)
                {
                    if (tiles[i, j].inPlay)
                    {
                        tempTiles.Add(tiles[i, j]);
                    }

                    if (mines[i, j] == true)
                    {
                        mineCount += 1;
                    }
                    else
                    {
                        mineCount += 0;
                    }
                }
            }
        }

        if(mineCount == 0)
        {
            sweepAdjacentTiles(tempTiles);
        }

        if(totalClicked+totalMines == boardCols * boardRows)
        {
            GameOver(true);
        }
        return mineCount;
    }

    private void sweepAdjacentTiles(List<TileScript> adjacentTiles)
    {
        for (int i = 0; i < adjacentTiles.Count; i++)
        {
            adjacentTiles[i].clickMe();
        }
    }

    private void showMines()
    {
        for (int i = 0; i < mines.GetLength(0); i++)
        {
            for (int j = 0; j < mines.GetLength(1); j++)
            {
                if (mines[i, j])
                {
                    tiles[i, j].showMine();
                }
            }
        }
    }

    private void setGameParameters(int cols, int rows)
    {
        boardCols = cols;
        boardRows = rows;

        mines = new bool[boardCols, boardRows];
        tiles = new TileScript[boardCols, boardRows];

        xStart = -(float)boardCols / 2 * boardXSpace + (boardXSpace / 2);
        yStart = (float)boardRows / 2 * boardYSpace - (boardYSpace / 2);

        Screen.SetResolution((int)boardXSpace * boardCols, (int)boardYSpace * boardRows, false);
    }

    private void GameOver(bool win)
    {
        gameOn = false;
        showMines();
        if (win)
        {
            gameOverMenu.SetActive(true);
            gameOverMenu.GetComponent<Text>().text = "You Win!";
        }

        else
        {
            gameOverMenu.SetActive(true);
            gameOverMenu.GetComponent<Text>().text = "BOOM!";
        }

    }

    public void reloadScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

}
