using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public ParticleSystem destroyEffect;

    public Vector2Int posIndex;
    Board board;

    Vector2 firstTouchPosition;
    Vector2 finalTouchPosition;
    bool mousePressed;
    float swipeAngle = 0;
    public bool isMatched;

    Gem otherGem;

    [HideInInspector] Vector2Int previousPos;

    public enum GemType { blue, green, red, yellow, purple, bomb }
    public GemType type;

    void Awake()
    {
        board = FindObjectOfType<Board>();
    }

    void Start()
    {
                
    }

    void Update()
    {
        MouseUp();      
        SwapGems();               
    }
   

    void SwapGems()
    {
        if (Vector2.Distance(transform.position, new Vector3(posIndex.x,posIndex.y,transform.position.z)) > 0.01f)
        {
            transform.position = Vector2.Lerp(transform.position, posIndex, board.gemSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = new Vector2(posIndex.x,posIndex.y);
        }
    }
    public void SetupGem(Vector2Int position)
    {
        posIndex = position;
    }

    private void MouseUp()
    {
        if (mousePressed && Input.GetMouseButtonUp(0))
        {
            mousePressed = false;
            if(board.currentState == Board.BoardState.notProcessing)
            {
                finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                CalculateAngel();
            }            
        }
    }

    // OnMouseDown detectes if we press on an object.
    private void OnMouseDown()
    {    
        if(board.currentState == Board.BoardState.notProcessing)
        {
            firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePressed = true;
        }        
    }
       

    private void CalculateAngel()
    {
        swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y,
            finalTouchPosition.x - firstTouchPosition.x);
        swipeAngle = swipeAngle * 180 / Mathf.PI;
        Debug.Log("Angle: " + swipeAngle);

        // if player moves mouse so far away cancel the swapping
        if(Vector3.Distance(firstTouchPosition,finalTouchPosition) < 2f)
        {
            MovePieces();
        }

    }

    void ChangeNames()
    {
        string temp = otherGem.name;
        otherGem.name = this.name;
        this.name = temp;
    }
    

    private void MovePieces()
    {
        previousPos = posIndex;

        if(swipeAngle < 45 && swipeAngle > -45 && posIndex.x < board.getWidth() - 1) // RIGHT
        {
            otherGem = board.allGems[posIndex.x + 1, posIndex.y];
            posIndex.x++;
            otherGem.posIndex.x--;
        }
        else if (swipeAngle < 135 && swipeAngle > 45 && posIndex.y < board.getHeight() - 1) // UP
        {
            otherGem = board.allGems[posIndex.x, posIndex.y + 1];
            posIndex.y++;
            otherGem.posIndex.y--;
        }
        else if (swipeAngle < -45 && swipeAngle > -135 && posIndex.y > 0) // DOWN
        {
            otherGem = board.allGems[posIndex.x, posIndex.y - 1];
            posIndex.y--;
            otherGem.posIndex.y++;
        }
        else if (swipeAngle < -135 || swipeAngle > 135 && posIndex.x > 0) // LEFT
        {
            otherGem = board.allGems[posIndex.x - 1, posIndex.y];
            posIndex.x--;
            otherGem.posIndex.x++;
        }
        board.allGems[posIndex.x, posIndex.y] = this;
        board.allGems[otherGem.posIndex.x, otherGem.posIndex.y] = otherGem;
        ChangeNames();

        StartCoroutine(SwapBack());
    }


    IEnumerator SwapBack()
    {
        board.currentState = Board.BoardState.processing;

        yield return new WaitForSeconds(0.5f);

        board.matchFinder.FindMatches();

        if(otherGem != null)
        {
            if(!isMatched && !otherGem.isMatched)
            {
                otherGem.posIndex = posIndex;
                posIndex = previousPos;

                board.allGems[posIndex.x, posIndex.y] = this;
                board.allGems[otherGem.posIndex.x, otherGem.posIndex.y] = otherGem;
                ChangeNames();

                yield return new WaitForSeconds(0.5f);
                board.currentState = Board.BoardState.notProcessing;

            }
            else
            {
                board.DestoyMatches();
            }
        }

    }

    public void CallDestroyEffect()
    {
        Instantiate(destroyEffect, new Vector2(posIndex.x, posIndex.y), Quaternion.identity);
    }


}
