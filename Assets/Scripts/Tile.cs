using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int row, column;
    public int pre_row, pre_column;
    public float swipeAngle = 0;
    public int targetX;
    public int targetY;
    public bool isMatch = false;

    private int matchCount = 1;
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    public GameObject otherTile;
    private Board board;
    private Vector2 tempPosition;
    private FindMatch findMatch;

    [Header("Powerup Stuff")]
    public bool isColumnItem;
    public bool isRowItem;
    public GameObject rowItem;
    public GameObject columnItem;
    // Start is called before the first frame update

    void Start()
    {
        board = FindObjectOfType<Board>();
        findMatch = FindObjectOfType<FindMatch>();
        isColumnItem = false;
        isRowItem = false;
        //targetX = (int)transform.position.x;
        //targetY = (int)transform.position.y;
        //row = targetY;
        //column = targetX;
        //pre_row = row;
        //pre_column = column;
    }

    // Update is called once per frame
    void Update()
    {
        findMatch.FindAllMatches();
        targetX = column;
        targetY = row;
        if(Mathf.Abs(targetX - transform.position.x) > 0.1)
        {
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .4f);
            if (row < board.height)
            {
                if (board.totalTiles[column, row] != this.gameObject)
                {
                    board.totalTiles[column, row] = this.gameObject;

                }

            }
        }
        else
        {
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;
            
        }
        if (Mathf.Abs(targetY - transform.position.y) > 0.1)
        {
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .4f);
            if(row < board.height)
            {
                if (board.totalTiles[column, row] != this.gameObject)
                {
                    board.totalTiles[column, row] = this.gameObject;

                }

            }

        }
        else
        {
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;

        }
        
    }
    //private void FixedUpdate()
    //{
    //    if(row < board.height)
    //    {
    //        MatchTile();
    //        TempMatchTile();
    //    }
            
    //}

    private void OnMouseDown()
    {
        firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
    }

    private void OnMouseUp()
    {
        finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalculateAngle();
    }

    void CalculateAngle()
    {
        swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
        MoveTile();
        board.currentTile = this;
        
    }

    void MoveTile()
    {
        if(swipeAngle > -45 && swipeAngle <= 45 && column < board.width -1 && swipeAngle != 0)
        {
            //Right move
            otherTile = board.totalTiles[column + 1, row];
            pre_column = column;
            pre_row = row;
            otherTile.GetComponent<Tile>().column -= 1;
            column += 1;
        }
        else if(swipeAngle > 45 && swipeAngle <= 135 && row < board.height -1 && swipeAngle != 0)
        {
            //Up move
            otherTile = board.totalTiles[column, row + 1];
            pre_column = column;
            pre_row = row;
            otherTile.GetComponent<Tile>().row -= 1;
            row += 1;
        }
        else if(swipeAngle > 135 || swipeAngle <= -135 && column > 0 && swipeAngle != 0)
        {
            //Left move
            otherTile = board.totalTiles[column - 1, row];
            pre_column = column;
            pre_row = row;
            otherTile.GetComponent<Tile>().column += 1;
            column -= 1;
        }
        else if(swipeAngle < -45 && swipeAngle >= -135 && row > 0 && swipeAngle != 0)
        {
            //Down move
            otherTile = board.totalTiles[column, row - 1];
            pre_column = column;
            pre_row = row;
            otherTile.GetComponent<Tile>().row += 1;
            row -= 1;
        }

        StartCoroutine(ResetMove());
    }

    void MatchTile()
    {
        if (column < board.width - 1 && column > 0 && board.totalTiles[column - 1, row] != null && board.totalTiles[column + 1, row] != null)
        {
            if(board.totalTiles[column, row].tag == board.totalTiles[column - 1, row].tag && board.totalTiles[column + 1, row].tag == board.totalTiles[column, row].tag)
            {
                isMatch = true;
                board.totalTiles[column + 1, row].GetComponent<Tile>().isMatch = true;
                board.totalTiles[column - 1, row].GetComponent<Tile>().isMatch = true;
               
                //Destroy(board.totalTiles[column, row], 0.1f);
                //Destroy(board.totalTiles[column + 1, row], 0.1f);
                //Destroy(board.totalTiles[column - 1, row], 0.1f);

                //board.totalTiles[column, row] = null;
                //board.totalTiles[column + 1, row] = null;
                //board.totalTiles[column - 1, row] = null;
            }

        }

        if (row < board.height - 1 && row > 0 && board.totalTiles[column, row - 1] != null && board.totalTiles[column, row + 1] != null)
        {
            if(board.totalTiles[column, row].tag == board.totalTiles[column, row - 1].tag && board.totalTiles[column, row + 1].tag == board.totalTiles[column, row].tag)
            {
                isMatch = true;
                board.totalTiles[column, row + 1].GetComponent<Tile>().isMatch = true;
                board.totalTiles[column, row - 1].GetComponent<Tile>().isMatch = true;
               
                //Destroy(board.totalTiles[column, row], 0.1f);
                //Destroy(board.totalTiles[column, row + 1], 0.1f);
                //Destroy(board.totalTiles[column, row - 1], 0.1f);

                //board.totalTiles[column, row] = null;
                //board.totalTiles[column, row + 1] = null;
                //board.totalTiles[column, row - 1] = null;
            }

        }
       
    }

    public IEnumerator ResetMove()
    {
        yield return new WaitForSeconds(0.5f);
        if(otherTile != null)
        {
            if (!isMatch && !otherTile.GetComponent<Tile>().isMatch)
            {
                int temp = otherTile.GetComponent<Tile>().row;
                otherTile.GetComponent<Tile>().row = row;
                otherTile.GetComponent<Tile>().column = column;
                row = temp;
                column = pre_column;
                board.currentTile = null;
            }
            else
            {
                board.limitCount--;
                board.currentTile = this;
                board.DestroyTile();
            }
            //otherTile = null;
        }               
    }

    public void MakeRowItem()
    {
        isRowItem = true;
        this.gameObject.GetComponent<SpriteRenderer>().sprite = rowItem.GetComponent<SpriteRenderer>().sprite;
        //rowItem = Instantiate(rowItem, transform.localPosition, Quaternion.identity) as GameObject;
        //Destroy(gameObject);
        print("row");
    }

    public void MakeColumnItem()
    {
        isColumnItem = true;
        this.gameObject.GetComponent<SpriteRenderer>().sprite = columnItem.GetComponent<SpriteRenderer>().sprite;
        //columnItem = Instantiate(columnItem, transform.localPosition, Quaternion.identity) as GameObject;
        //Destroy(gameObject);
        print("column");
    }

}
