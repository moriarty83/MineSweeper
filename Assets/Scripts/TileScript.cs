using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class TileScript : MonoBehaviour, IPointerClickHandler
{
    public BoardManager boardManager;
    private Text text;
    public bool inPlay = true;
    public int xCoordinate, yCoordinate;
    private bool marked = false;
    private Image tileSprite;
    public Sprite unclickedSprite, bombSprite, markedSprite, clickedSprite;

    // Start is called before the first frame update
    void Start()
    {
        tileSprite = this.gameObject.GetComponent<Image>();
        text = GetComponentInChildren<Text>();
        boardManager = GameObject.Find("GameCanvas").GetComponent<BoardManager>();
    }

    public void setCoordinates(int x, int y)
    {
        xCoordinate = x;
        yCoordinate = y;
    }

    public void clickMe()
    {
        if(inPlay == false)
        {
            return;
        }

        if (marked)
        {
            markSpot();
        }

        else
        {
            inPlay = false;
            boardManager.totalClicked += 1;
            int mines = boardManager.sweepMe(xCoordinate, yCoordinate);
            if (mines > 0)
            {
                text.text = mines.ToString();
                tileSprite.sprite = clickedSprite;
            };
            if (mines == 0)
            {
                tileSprite.sprite = clickedSprite;
            }
        }
    }

    public void showMine()
    {
        tileSprite.sprite = bombSprite;
    }

    public void markSpot()
    {
        if (marked)
        {
            tileSprite.sprite = unclickedSprite;
            marked = false;
            return;
        }
        else
        {
            tileSprite.sprite = markedSprite;
            marked = true;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            markSpot();
        }
    }

}
