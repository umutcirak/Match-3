using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MatchFinder : MonoBehaviour
{
    Board board;
    public List<Gem> currentMatches = new List<Gem>();
    private void Awake()
    {
        board = FindObjectOfType<Board>();
    }

    public void FindMatches()
    {
        currentMatches.Clear();

        for (int x = 0; x < board.getWidth(); x++)
        {
            for (int y = 0; y < board.getHeight(); y++)
            {
                Gem currentGem = board.allGems[x, y];
                if(currentGem != null)
                {
                    // CONTROL HORIZONTALLY
                    if(x > 0 && x < board.getWidth() -1)
                    {                        
                        Gem leftGem = board.allGems[x - 1, y];
                        Gem rightGem = board.allGems[x + 1, y];

                        if(leftGem != null && rightGem != null)
                        {
                            if(leftGem.type == currentGem.type && currentGem.type == rightGem.type)
                            {
                                currentGem.isMatched = true;
                                leftGem.isMatched = true;
                                rightGem.isMatched = true;

                                currentMatches.Add(currentGem);
                                currentMatches.Add(leftGem);
                                currentMatches.Add(rightGem);
                            }
                        }

                    }
                    // CONTROL VERTICALLY
                    if (y > 0 && y < board.getHeight() - 1)
                    {
                        Gem belowGem = board.allGems[x, y - 1];
                        Gem aboveGem = board.allGems[x, y + 1];

                        if (belowGem != null && aboveGem != null)
                        {
                            if (belowGem.type == currentGem.type && currentGem.type == aboveGem.type)
                            {
                                currentGem.isMatched = true;
                                belowGem.isMatched = true;
                                aboveGem.isMatched = true;

                                currentMatches.Add(currentGem);
                                currentMatches.Add(belowGem);
                                currentMatches.Add(aboveGem);
                            }
                        }

                    }

                }


            }

        }
        // using System.Linq
        currentMatches = currentMatches.Distinct().ToList();

    }
   


}
