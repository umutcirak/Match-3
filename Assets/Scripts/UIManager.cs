using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI time_value;
    public TextMeshProUGUI score_value;
    public TextMeshProUGUI shuffle_value;    


    Board board;
    RoundManager roundManager;

    private float displayScore;
    [SerializeField ]float scoreCatchSpeed;

    void Awake()
    {
        board = FindObjectOfType<Board>();
        roundManager = FindObjectOfType<RoundManager>();
    }


    void Update()
    {
        DisplayShuffleLeft();
        DisplayTimeLeft();
        DisplayScore();
        FinishCheck();
    }

    void DisplayShuffleLeft()
    {
        shuffle_value.text = roundManager.shuffleLeft.ToString();
    }

    void DisplayTimeLeft()
    {     
        time_value.text = ((int)roundManager.roundTime).ToString() + " s";
    }
    
    void DisplayScore()
    {      
        if(displayScore - roundManager.currentScore < 0.05f)
        {
            displayScore = Mathf.Lerp(displayScore, roundManager.currentScore, scoreCatchSpeed * Time.deltaTime);
        }
        else
        {            
            displayScore = roundManager.currentScore;
        }
        
        score_value.text = displayScore.ToString("0");
    }



    void FinishCheck()
    {
        if(roundManager.roundTime == 0f && board.currentState == Board.BoardState.notProcessing)
        {
            StartCoroutine(OpenEndSceneCo());
            SFXManager.instance.PlayLevelComplete();
        }        
    }

    IEnumerator OpenEndSceneCo()
    {
        yield return new WaitForSeconds(1f);

        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }










}
