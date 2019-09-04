using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FindMatch : MonoBehaviour
{
    private Board board;
    public List<GameObject> currentMatches = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator FindAllMatchesCo()
    {
        yield return new WaitForSeconds(.2f);
        for(int i = 0; i< board.width; i++)
        {
            for(int j = 0; j < board.height; j++)
            {
                GameObject current = board.totalTiles[i, j];
                if(current != null)
                {
                    if(i > 0 && i < board.width - 1)
                    {
                        GameObject left = board.totalTiles[i - 1, j];
                        GameObject right = board.totalTiles[i + 1, j];
                        if(left != null && right != null)
                        {
                            if (left.tag == current.tag && right.tag == current.tag)
                            {
                                if (current.GetComponent<Tile>().isColumnItem || left.GetComponent<Tile>().isColumnItem || right.GetComponent<Tile>().isColumnItem)
                                {
                                    currentMatches.Union(GetColumnPieces(j));
                                }
                                if (current.GetComponent<Tile>().isRowItem)
                                {
                                    currentMatches.Union(GetRowPieces(i));
                                }
                                if (left.GetComponent<Tile>().isRowItem)
                                {
                                    currentMatches.Union(GetRowPieces(i - 1));
                                }
                                if (right.GetComponent<Tile>().isRowItem)
                                {
                                    currentMatches.Union(GetRowPieces(i + 1));
                                }
                                //
                                if (!currentMatches.Contains(left))
                                {
                                    currentMatches.Add(left);
                                }
                                left.GetComponent<Tile>().isMatch = true;

                                if (!currentMatches.Contains(right))
                                {
                                    currentMatches.Add(right);
                                }
                                right.GetComponent<Tile>().isMatch = true;

                                if (!currentMatches.Contains(current))
                                {
                                    currentMatches.Add(current);
                                }
                                current.GetComponent<Tile>().isMatch = true;
                            }
                        }
                        
                       
                    }

                    if (j > 0 && j < board.height - 1)
                    {
                        GameObject up = board.totalTiles[i, j+1];
                        GameObject down = board.totalTiles[i, j-1];
                        if(up != null && down != null)
                        {
                            if (up.tag == current.tag && down.tag == current.tag)
                            {
                                if (current.GetComponent<Tile>().isRowItem || up.GetComponent<Tile>().isRowItem || down.GetComponent<Tile>().isRowItem)
                                {
                                    currentMatches.Union(GetRowPieces(i));
                                }
                                if (current.GetComponent<Tile>().isColumnItem)
                                {
                                    currentMatches.Union(GetColumnPieces(j));
                                }
                                if (up.GetComponent<Tile>().isColumnItem)
                                {
                                    currentMatches.Union(GetColumnPieces(j + 1));
                                }
                                if (down.GetComponent<Tile>().isColumnItem)
                                {
                                    currentMatches.Union(GetColumnPieces(j - 1));
                                }
                                //
                                if (!currentMatches.Contains(up))
                                {
                                    currentMatches.Add(up);
                                }
                                up.GetComponent<Tile>().isMatch = true;

                                if (!currentMatches.Contains(down))
                                {
                                    currentMatches.Add(down);
                                }
                                down.GetComponent<Tile>().isMatch = true;

                                if (!currentMatches.Contains(current))
                                {
                                    currentMatches.Add(current);
                                }
                                current.GetComponent<Tile>().isMatch = true;
                            }
                        }
                        
                    }
        
                }
            }
        }
    }

    public void FindAllMatches()
    {
        StartCoroutine(FindAllMatchesCo());
    }

    List<GameObject> GetColumnPieces(int column)
    {
        List<GameObject> tiles = new List<GameObject>();
        for(int i = 0; i<board.height; i++)
        {
            if(board.totalTiles[i, column] != null)
            {
                tiles.Add(board.totalTiles[i, column]);
                board.totalTiles[i, column].GetComponent<Tile>().isMatch = true;
            }
        }
        return tiles;
    }

    List<GameObject> GetRowPieces(int row)
    {
        List<GameObject> tiles = new List<GameObject>();
        for (int i = 0; i < board.width; i++)
        {
            if (board.totalTiles[row, i] != null)
            {
                tiles.Add(board.totalTiles[row, i]);
                board.totalTiles[row, i].GetComponent<Tile>().isMatch = true;
            }
        }
        return tiles;
    }

    public void CheckItem(Tile currentTile)
    {
        if (currentTile != null)
        {
            print("enter");
            if(currentMatches.Count >= 4) {
           
                if (currentTile.isMatch)
                {
                    print("My create");
                    currentTile.isMatch = false;
                
                    if(board.currentTile.swipeAngle > 45 && board.currentTile.swipeAngle <= 135)
                    {
                        currentTile.MakeRowItem();
                    }
                    else if(board.currentTile.swipeAngle < -45 && board.currentTile.swipeAngle >= -135)
                    {
                        currentTile.MakeRowItem();

                    }
                    else if(board.currentTile.swipeAngle > -45 && board.currentTile.swipeAngle <= 45)
                    {
                        currentTile.MakeColumnItem();
                    }
                    else if(board.currentTile.swipeAngle > 135 || board.currentTile.swipeAngle <= -135)
                    {
                        currentTile.MakeColumnItem();
                    }
                    else
                        currentTile.MakeColumnItem();

                }
                //else if (currentTile.otherTile != null)
                //    {
                //        Tile otherTile = currentTile.otherTile.GetComponent<Tile>();
                //        if (otherTile.isMatch)
                //        {
                //            print("Other create");
                //            otherTile.isMatch = false;
                //            if (currentTile.swipeAngle > 45 && currentTile.swipeAngle <= 135)
                //            {
                //                otherTile.MakeRowItem();
                //            }
                //            else if (currentTile.swipeAngle < -45 && currentTile.swipeAngle >= -135)
                //            {
                //                otherTile.MakeRowItem();

                //            }
                //            else if (currentTile.swipeAngle > -45 && currentTile.swipeAngle <= 45)
                //            {
                //                otherTile.MakeColumnItem();
                //            }
                //            else if (currentTile.swipeAngle > 135 || currentTile.swipeAngle <= -135)
                //            {
                //                otherTile.MakeColumnItem();

                //            }
                //            else
                //                currentTile.MakeColumnItem();

                //    }
                //}
            }
        }
    }
}
