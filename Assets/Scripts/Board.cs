using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public int width;
    public int height;
    public int count;
    public int limitCount;
    public GameObject tilePrefab;
    public GameObject[] tiles;
    public GameObject[,] totalTiles;
    public Text scoreText;
    public Text limitCountText;
    public Tile currentTile;

    private int score;
    private int[,] countArray;
    private FindMatch findMatch;
    private BackgroundTile[,] backgroundTiles;

    void Start()
    {
        countArray = new int[width, height];
        backgroundTiles = new BackgroundTile[width, height];
        totalTiles = new GameObject[width, height];
        findMatch = FindObjectOfType<FindMatch>();
        SetUp();
        
    }

    private void Update()
    {
        scoreText.text = "Score : " + score;
        //limitCountText.text = "LimitCount : " + limitCount;
        //if (limitCount <= 0)
        //{
        //    Application.LoadLevel(0);
        //}
        
    }


    private void SetUp()
    {
        for(int i = 0; i<width; i++)
        {
            for(int j = 0; j<height; j++)
            {
                Vector2 tempPosition = new Vector2(i, j);
                GameObject backgroundTile = Instantiate(tilePrefab, tempPosition, Quaternion.identity) as GameObject;
                backgroundTile.transform.parent = this.transform;
                backgroundTile.name = "(" + i +"," + j + ")";
                count = Random.Range(0, tiles.Length);
                //CheckNum();
                while (CheckTile(i, j, count))
                {
                    count = Random.Range(0, tiles.Length);
                }

                GameObject tile = Instantiate(tiles[count], tempPosition, Quaternion.identity);
                tile.GetComponent<Tile>().row = j;
                tile.GetComponent<Tile>().column = i;
                tile.transform.parent = this.transform;
                tile.name = "Tile(" + i + "," + j + ")";
                totalTiles[i, j] = tile;
            }
        }
    }

    public bool CheckTile(int i, int j, int count)
    {
        if (i > 1)
        {
            if (totalTiles[i - 1, j].GetComponent<SpriteRenderer>().sprite == tiles[count].GetComponent<SpriteRenderer>().sprite && totalTiles[i - 2, j].GetComponent<SpriteRenderer>().sprite == tiles[count].GetComponent<SpriteRenderer>().sprite)
            {
                return true;
            }
        }
        if(j > 1) { 
            if (totalTiles[i, j - 1].GetComponent<SpriteRenderer>().sprite == tiles[count].GetComponent<SpriteRenderer>().sprite && totalTiles[i, j - 2].GetComponent<SpriteRenderer>().sprite == tiles[count].GetComponent<SpriteRenderer>().sprite)
            {
                return true;
            }
           
        }
        return false;
    }

    public void DestroyTile()
    {
        
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (totalTiles[i, j] != null)
                {
                    if (totalTiles[i, j].GetComponent<Tile>().isMatch)
                    {
                        print(findMatch.currentMatches.Count);
                        //if (findMatch.currentMatches.Count == 4 || findMatch.currentMatches.Count == 7 || findMatch.currentMatches.Count == 8)
                        //{
                        //    //currentTile = totalTiles[i, j].GetComponent<Tile>();
                        //    findMatch.CheckItem();
                        //    print("check");
                        //}

                        if (findMatch.currentMatches.Count == 4 || findMatch.currentMatches.Count == 6 || findMatch.currentMatches.Count == 7 || findMatch.currentMatches.Count == 8)
                        {
                             Tile itemTile = totalTiles[i, j].GetComponent<Tile>();
                            findMatch.CheckItem(itemTile);
                            findMatch.currentMatches.Remove(totalTiles[i, j]);
                            
                        }

                        //GameObject.Find("SoundManager").GetComponent<AudioSource>().Play();

                        //findMatch.currentMatches.Remove(totalTiles[i, j]);
                        if (totalTiles[i, j].GetComponent<Tile>().isMatch)
                        {
                            Destroy(totalTiles[i, j]);
                            totalTiles[i, j] = null;
                        }
                       
                        score += 10;
                        
                    }
                }
            }
        }
        StartCoroutine(DownTile());
        
    }

    public IEnumerator DownTile()
    {
        yield return new WaitForSeconds(.3f);
        int nullCount = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (totalTiles[i, j] == null)
                {
                    nullCount++;

                }
                else if (nullCount > 0)
                {
                    totalTiles[i, j].GetComponent<Tile>().row -= nullCount;
                    totalTiles[i, totalTiles[i, j].GetComponent<Tile>().row] = null;

                    totalTiles[i, totalTiles[i, j].GetComponent<Tile>().row] = totalTiles[i, j];
                    totalTiles[i, j] = null;
                }
            }
            nullCount = 0;
        }
        StartCoroutine(RealTimeFill());
    }

    public void RefillTile()
    {
        //yield return new WaitForSeconds(.5f);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (totalTiles[i, j] == null)
                {
                    Vector2 tempPosition = new Vector2(i, j+15);
                    
                    count = Random.Range(0, tiles.Length);
                    GameObject tile = Instantiate(tiles[count], tempPosition, Quaternion.identity);
                    //tile.transform.position = Vector2.Lerp(tile.transform.position, tempPosition, .3f);
                    tile.GetComponent<Tile>().row = j;
                    tile.GetComponent<Tile>().column = i;
                    tile.transform.parent = this.transform;
                    tile.name = "Tile(" + i + "," + j + ")";
                    totalTiles[i, j] = tile;
                }
            }

        }
        
    }

    private bool CheckMatch()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (totalTiles[i, j] != null)
                {
                    if (totalTiles[i, j].GetComponent<Tile>().isMatch)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public IEnumerator RealTimeFill()
    {
        yield return new WaitForSeconds(.3f);
        RefillTile();
        yield return new WaitForSeconds(.3f);
        while (CheckMatch())
        {
            yield return new WaitForSeconds(.4f);
            DestroyTile();
        }
        findMatch.currentMatches.Clear();
        currentTile = null;
    }

    //void CheckNum()
    //{
    //    for (int i = 0; i < width; i++)
    //    {
    //        for (int j = 0; j < height; j++)
    //        {

    //            if (i < width - 2 && countArray[i, j] == countArray[i + 1, j] && countArray[i + 1, j] == countArray[i + 2, j])
    //            {
    //                for(int k = 1; k<3; k++)
    //                {
    //                    countArray[i+k,j] = Random.Range(0, tiles.Length);
    //                    if(countArray[i+k,j] == countArray[i + k - 1, j])
    //                    {
    //                        k--;
    //                    }
    //                }

    //            }

    //            if (j < height - 2 && countArray[i, j] == countArray[i, j + 1] && countArray[i, j + 1] == countArray[i, j + 2])
    //            {
    //                for (int t = 1; t < 3; t++)
    //                {
    //                    countArray[i, j+t] = Random.Range(0, tiles.Length);
    //                    if (countArray[i, j+t] == countArray[i, j+t-1])
    //                    {
    //                        t--;
    //                    }
    //                }
    //            }

    //        }
    //    }
    //}
}
