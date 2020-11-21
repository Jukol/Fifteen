using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    Transform[] cells = new Transform[16];

    [SerializeField]
    GameObject[] pieces = new GameObject[15];
    GameObject piece01, piece02, piece03, piece04, piece05, piece06, piece07, piece08, piece09, piece10, piece11, piece12, piece13, piece14, piece15;
    [SerializeField]
    Text elapsedTimeText;

    [SerializeField]
    AudioClip woodSlide;

    [SerializeField]
    GameObject startPanel;

    [SerializeField]
    Text movesText;

    [SerializeField]
    Text topTimeScore;

    Animator startPanelAnim;
    string elapsedTime, hoursText, minutesText, secondsText, tenthsOfASecondText, topTimeScorePath, topMovesScorePath;
    int timeScore, previousTime, moves, previousMoves;

    [SerializeField]
    Text shuffleButtonText;

    [SerializeField]
    Text movesCounter, bestTimeScoreTimeAndMovesText, bestMovesScoreTimeAndMovesText, bestTimeScoreText, bestMovesScoreText;

    [SerializeField]
    float shuffleSpeed;


    int hours, minutes, seconds, tenthsOfASecond, hundredthOfASecond;
    float startTime;
    bool startTimer;
    bool shuffleButtonPressed;

    [SerializeField]
    GameObject bestTimePanel, goodJobPanel, bestMovesPanel, bestTimeAndMovesPanel;

    bool shuffle, afterShuffleRoutine = true;



    public int Moves 
    { 
        get
        {
            return moves;
        }
        set
        {
            moves = value;
        }
    }

    public bool ShuffleButtonPressed
    {
        get
        {
            return shuffleButtonPressed;
        }
    }


    private void Start()
    {
        piece01 = pieces[0];
        piece02 = pieces[1];
        piece03 = pieces[2];
        piece04 = pieces[3];
        piece05 = pieces[4];
        piece06 = pieces[5];
        piece07 = pieces[6];
        piece08 = pieces[7];
        piece09 = pieces[8];
        piece10 = pieces[9];
        piece11 = pieces[10];
        piece12 = pieces[11];
        piece13 = pieces[12];
        piece14 = pieces[13];
        piece15 = pieces[14];

        startPanelAnim = startPanel.GetComponent<Animator>();
        startPanel.SetActive(true);
        topTimeScorePath = Application.persistentDataPath + "/topTimeScore.txt";
        topMovesScorePath = Application.persistentDataPath + "/topMovesScore.txt";
    }

    private void Update()
    {
        if (startTimer)
            Timer();
        movesText.text = "Moves: " + moves.ToString();

        if (shuffle)
        {
            for (int i = 0; i < pieces.Length; i++)
            {
                pieces[i].transform.position = Vector3.MoveTowards(pieces[i].transform.position, cells[i].transform.position, shuffleSpeed);
            }
        }

        if (pieces[0].transform.position == cells[0].transform.position &&
                pieces[1].transform.position == cells[1].transform.position &&
                pieces[2].transform.position == cells[2].transform.position &&
                pieces[3].transform.position == cells[3].transform.position &&
                pieces[4].transform.position == cells[4].transform.position &&
                pieces[5].transform.position == cells[5].transform.position &&
                pieces[6].transform.position == cells[6].transform.position &&
                pieces[7].transform.position == cells[7].transform.position &&
                pieces[8].transform.position == cells[8].transform.position &&
                pieces[9].transform.position == cells[9].transform.position &&
                pieces[10].transform.position == cells[10].transform.position &&
                pieces[11].transform.position == cells[11].transform.position &&
                pieces[12].transform.position == cells[12].transform.position &&
                pieces[13].transform.position == cells[13].transform.position &&
                pieces[14].transform.position == cells[14].transform.position)
        {
            shuffle = false;
            if (afterShuffleRoutine == true)
            {
                AfterShuffleRoutine();
            }
        }



    }

    public void Randomize()
    {
        for (int positionOfArray = 0; positionOfArray < pieces.Length; positionOfArray++)
        {
            GameObject obj = pieces[positionOfArray];
            int randomizeArray = Random.Range(0, positionOfArray);
            pieces[positionOfArray] = pieces[randomizeArray];
            pieces[randomizeArray] = obj;
        }

        shuffle = true;
        afterShuffleRoutine = true;
    }

    private void AfterShuffleRoutine()
    {
        movesCounter.gameObject.SetActive(true);
        startTime = Time.time;
        startTimer = true;
        shuffleButtonPressed = true;
        moves = 0;
        shuffleButtonText.text = "Reshuffle";
        afterShuffleRoutine = false;
    }

    public void CheckForWin()
    {
        if (piece01.transform.position == cells[0].transform.position &&
            piece02.transform.position == cells[1].transform.position &&
            piece03.transform.position == cells[2].transform.position &&
            piece04.transform.position == cells[3].transform.position &&
            piece05.transform.position == cells[4].transform.position &&
            piece06.transform.position == cells[5].transform.position &&
            piece07.transform.position == cells[6].transform.position &&
            piece08.transform.position == cells[7].transform.position &&
            piece09.transform.position == cells[8].transform.position &&
            piece10.transform.position == cells[9].transform.position &&
            piece11.transform.position == cells[10].transform.position &&
            piece12.transform.position == cells[11].transform.position &&
            piece13.transform.position == cells[12].transform.position &&
            piece14.transform.position == cells[13].transform.position &&
            piece15.transform.position == cells[14].transform.position)
        {
            startTimer = false;
            shuffleButtonText.text = "Play again!";
            shuffleButtonPressed = false;
            TimeScoreText();
            MovesScoreText();

            if (timeScore < previousTime && moves < previousMoves)
            {
                bestTimeScoreTimeAndMovesText.text = IntToTime(timeScore);
                bestMovesScoreTimeAndMovesText.text = (moves + 1).ToString();
                bestTimeAndMovesPanel.SetActive(true);
            }   
            else if (timeScore < previousTime)
            {
                bestTimePanel.SetActive(true);
                bestTimeScoreText.text = IntToTime(timeScore);
            }   
            else if (moves < previousMoves)
            {
                bestMovesPanel.SetActive(true);
                bestMovesScoreText.text = (moves + 1).ToString();
            }   
            else
            {
                goodJobPanel.SetActive(true);
            }
        }
    }

    public void Timer()
    {
        timeScore = (int)((Time.time - startTime) * 100);
        elapsedTime = IntToTime(timeScore);
        elapsedTimeText.text = elapsedTime;
    }

    public void QuitGame()
    {
        startTimer = false;
        Application.Quit();
    }

    public void PlaySound()
    {
        AudioSource.PlayClipAtPoint(woodSlide, Camera.main.transform.position, 1f);
    }

    public void StartPanelAnimationOut()
    {
        startPanelAnim.SetTrigger("Out");
    }

    public void StartPanelAnimationIn()
    {
        startPanelAnim.SetTrigger("In");
    }

    void TimeScoreText()
    {
        if (!File.Exists(topTimeScorePath))
            File.WriteAllText(topTimeScorePath, "1000000");
        
        if (File.Exists(topTimeScorePath))
            previousTime = int.Parse(File.ReadAllText(topTimeScorePath));
        
        if (timeScore < previousTime)
        {
            string content = timeScore.ToString();
            File.WriteAllText(topTimeScorePath, content);
        }
    }

    void MovesScoreText()
    {
        if (!File.Exists(topMovesScorePath))
            File.WriteAllText(topMovesScorePath, "1000000");
        
        if (File.Exists(topMovesScorePath))
            previousMoves = int.Parse(File.ReadAllText(topMovesScorePath));

        if (moves < previousMoves)
        {
            string content = (moves + 1).ToString();
            File.WriteAllText(topMovesScorePath, content);
        }
    }

    public void ShowScoreText()
    {
        
        if (File.Exists(topTimeScorePath) && File.Exists(topMovesScorePath))
        {
            int getTimeValueFromFile = int.Parse(File.ReadAllText(topTimeScorePath));
            
            string timeString = IntToTime(getTimeValueFromFile);
            string movesString = File.ReadAllText(topMovesScorePath);
            

            topTimeScore.text = "Best time: " + timeString + "\n" + "Best moves: " + movesString;
        }
    }

    string IntToTime(int value)
    {
        int hundredths = value % 100;
        int seconds = value / 100 % 60;
        int minutes = value / 100 / 60 % 60;
        int hours = value / 100 / 60 / 60;

        string hundredthsString = hundredths.ToString();
        if (hundredths < 10)
            hundredthsString = "0" + hundredthsString;
        string secondsString = seconds.ToString();
        if (seconds < 10)
            secondsString = "0" + secondsString;
        string minutesString = minutes.ToString();
        if (minutes < 10)
            minutesString = "0" + minutesString;
        string hoursString = hours.ToString();
        if (hours < 10)
            hoursString = "0" + hoursString;

        string timeString = hoursString + ":" + minutesString + ":" + secondsString + ":" + hundredthsString;
        return timeString;
    }

 

}
