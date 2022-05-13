using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [Header("Board Settings")]
    [SerializeField] int width;
    [SerializeField] int height;
    [SerializeField] GameObject bgTilePrefab;
    public float gemSpeed = 3f;

    [Header("Gem Settings")]
    [SerializeField] Gem[] gemPrefabs;


    [HideInInspector] public Gem[,] allGems;

    [HideInInspector] public MatchFinder matchFinder;


    void Awake()
    {
        matchFinder = FindObjectOfType<MatchFinder>();        
    }

    void Start()
    {
        allGems = new Gem[width, height];
        Setup();
    }

    private void Update()
    {
        matchFinder.FindMatches();
    }

    private void Setup()
    {
        // Fill the board with bgTilePrefab
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Set tile position
                Vector2 pos = new Vector2(x, y);
                // Instentiate background Tile, Quaternion.identity: Don't change rotation
                GameObject bgTile = Instantiate(bgTilePrefab, pos, Quaternion.identity);
                bgTile.transform.parent = transform;
                bgTile.name = "BG Tile-" + x + "," + y;


                int gemToUse = Random.Range(0, gemPrefabs.Length);

                while( isMatches(gemPrefabs[gemToUse], new Vector2Int(x,y)) )
                {
                    gemToUse = Random.Range(0, gemPrefabs.Length);
                }

                SpawnGem(new Vector2Int(x, y), gemPrefabs[gemToUse]);
            }
        }

    }

    void SpawnGem(Vector2Int pos, Gem gemPrefab)
    {            
        Gem gem = Instantiate(gemPrefab, new Vector3(pos.x,pos.y,0f), Quaternion.identity);

        gem.transform.parent = this.transform;
        gem.name = "Gem-" + pos.x + "," + pos.y;
        gem.SetupGem(pos);
        allGems[pos.x, pos.y] = gem;
    }

    bool isMatches(Gem gemToCheck, Vector2Int posToCheck)
    {
        // It checks gems until 2 gem before, because board creates gems at start linearly up.
        // Creat x=2 and x=3 so to check x=3 no need for x=4 because it does not exist.
        // to check x=3, need to check x=2 and x=1

        if(posToCheck.x > 1)
        {
            if (allGems[posToCheck.x - 1, posToCheck.y].type == gemToCheck.type &&
            allGems[posToCheck.x - 2, posToCheck.y].type == gemToCheck.type)
            {
                return true;
            }
        }
        
        if(posToCheck.y > 1)
        {
            if (allGems[posToCheck.x, posToCheck.y - 1].type == gemToCheck.type &&
            allGems[posToCheck.x, posToCheck.y - 2].type == gemToCheck.type)
            {
                return true;
            }
        }        

        return false;
    }

    void DestoyMatches()
    {
        for(int i = 0; i < matchFinder.currentMatches.Count; i++)
        {
            Gem gemToDestroy = matchFinder.currentMatches[i];
            Vector2Int pos = new Vector2Int(gemToDestroy.posIndex.x, gemToDestroy.posIndex.y);
            DestroyMatchedGemAt(pos);
        }

    }


    void DestroyMatchedGemAt(Vector2Int pos)
    {
        Gem gemToDestroy = allGems[pos.x, pos.y];

       if(gemToDestroy != null)
        {
            if (gemToDestroy.isMatched)
            {
                Destroy(gemToDestroy);
                allGems[pos.x, pos.y] = null;
            }            

        }

    }


    public int getWidth()
    {
        return this.width;
    }

    public int getHeight()
    {
        return this.height;
    }

}
