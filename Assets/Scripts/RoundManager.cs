using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public float roundTime = 60f;
    public int shuffleLeft = 15;
    [HideInInspector] public int currentScore = 0;

    private UIManager uiManager;
    private Board board;
    


    void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
        board = FindObjectOfType<Board>();
    }

    void Update()
    {
        CalculateTimeLeft();
    }


    public void CalculateTimeLeft()
    {
        if(roundTime > Mathf.Epsilon )
        {
            roundTime -= Time.deltaTime;
        }
        else        
        {
            roundTime = 0f;            
        }      
                
    }    


    

}
