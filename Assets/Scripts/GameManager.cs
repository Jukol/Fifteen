using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Michsky.UI.ModernUIPack;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    Transform[] cells = new Transform[16];

    [SerializeField]
    GameObject[] pieces = new GameObject[15];
    GameObject piece01, piece02, piece03, piece04, piece05, piece06, piece07, piece08, piece09, piece10, piece11, piece12, piece13, piece14, piece15; 
    [SerializeField]
    GameObject startPanel, bestTimePanel, goodJobPanel, bestMovesPanel, bestTimeAndMovesPanel, timerBox, movesBox;
    
    [SerializeField]
    Text elapsedTimeText, movesText, topTimeScore, movesCounter, bestTimeScoreTimeAndMovesText, bestMovesScoreTimeAndMovesText, bestTimeScoreText, bestMovesScoreText;

    [SerializeField]
    AudioClip woodSlide;

    Animator startPanelAnim;
    
    string elapsedTime;
    
    int timeScore, moves;

    [SerializeField]
    ButtonManager shuffleButton, startButtonOnStartPanel;

    [SerializeField]
    float shuffleSpeed;

    AudioSource music;

    [SerializeField]
    Toggle musicToggle, soundFXToggle, showTimerToglle, showMovesToggle;

    bool soundFX, startTimer, shuffleButtonPressed, shuffle, afterShuffleRoutine = true, startButtonPressedOnce;

    float startTime;

    public int Moves 
    { 
        get { return moves; }
        set { moves = value; }                             
    }

    public bool ShuffleButtonPressed
    {
        get { return shuffleButtonPressed; }
    }

    public void TimerOff()
    {
        startTimer = false;
    }

    public void TimerOn()
    {
        startTimer = true;
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
        music = GetComponent<AudioSource>();


        if (!PlayerPrefs.HasKey("TopTime"))
            PlayerPrefs.SetInt("TopTime", 1000000);

        if (!PlayerPrefs.HasKey("TopMoves"))
            PlayerPrefs.SetInt("TopMoves", 1000000);

        if (!PlayerPrefs.HasKey("Music"))
            PlayerPrefs.SetInt("Music", 1);

        if (!PlayerPrefs.HasKey("SoundFX"))
            PlayerPrefs.SetInt("SoundsFX", 1);

        if (!PlayerPrefs.HasKey("ShowTimer"))
            PlayerPrefs.SetInt("ShowTimer", 1);

        if (!PlayerPrefs.HasKey("ShowMoves"))
            PlayerPrefs.SetInt("ShowMoves", 1);

        if (PlayerPrefs.GetInt("Music") == 0)
            musicToggle.isOn = false;
        else if (PlayerPrefs.GetInt("Music") == 1)
            musicToggle.isOn = true;

        if (PlayerPrefs.GetInt("SoundFX") == 0)
        {
            soundFXToggle.isOn = false;
            soundFX = false;
        }
        else if (PlayerPrefs.GetInt("SoundFX") == 1)
        {
            soundFXToggle.isOn = true;
            soundFX = true;
        }

        if (PlayerPrefs.GetInt("ShowTimer") == 0)
        {
            showTimerToglle.isOn = false;
            timerBox.SetActive(false);
        }
        else if (PlayerPrefs.GetInt("ShowTimer") == 1)
        {
            showTimerToglle.isOn = true;
            timerBox.SetActive(true);
        }

        if (PlayerPrefs.GetInt("ShowMoves") == 0)
        {
            showMovesToggle.isOn = false;
            movesBox.SetActive(false);
        }
        else if (PlayerPrefs.GetInt("ShowMoves") == 1)
        {
            showMovesToggle.isOn = true;
            movesBox.SetActive(true);
        }
    }

    private void FixedUpdate()
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
        startTimer = false;
        
        for (int positionOfArray = 0; positionOfArray < pieces.Length; positionOfArray++)//looked this up on YouTube :-)
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
        shuffleButtonPressed = true;
        moves = 0;
        afterShuffleRoutine = false;
        startTimer = true;
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
            shuffleButton.buttonText = "Play again!";
            shuffleButtonPressed = false;
            TimeScore(); //put top time on PlayerPrefs
            MovesScore(); //put top moves on PlayerPrefs

            if (timeScore < PlayerPrefs.GetInt("TopTime") && moves < PlayerPrefs.GetInt("TopMoves"))
            {
                bestTimeScoreTimeAndMovesText.text = IntToTime(timeScore);
                bestMovesScoreTimeAndMovesText.text = (moves + 1).ToString();
                bestTimeAndMovesPanel.SetActive(true);
            }   
            else if (timeScore < PlayerPrefs.GetInt("TopTime"))
            {
                bestTimePanel.SetActive(true);
                bestTimeScoreText.text = IntToTime(timeScore);
            }   
            else if (moves < PlayerPrefs.GetInt("TopMoves"))
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
        PlayerPrefs.Save();
        Application.Quit();
    }

    public void PlaySound()
    {
        if (soundFX)
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

    void TimeScore()
    {   
        if (timeScore < PlayerPrefs.GetInt("TopTime"))
            PlayerPrefs.SetInt("TopTime", timeScore);
    }

    void MovesScore()
    {
        if (moves < PlayerPrefs.GetInt("TopMoves"))
            PlayerPrefs.SetInt("TopMoves", moves + 1);
    }

    public void ShowScoreText()
    {
        string timeString;
        string movesString;


        if (PlayerPrefs.GetInt("TopTime") == 1000000)
            timeString = "";
        else
            timeString = IntToTime(PlayerPrefs.GetInt("TopTime"));

        if (PlayerPrefs.GetInt("TopMoves") == 1000000)
            movesString = "";
        else
            movesString = PlayerPrefs.GetInt("TopMoves").ToString();

        topTimeScore.text = "Best time: " + timeString + "\n" + "Best moves: " + movesString;
    }

    string IntToTime(int value)//convert time in seconds to "03:36:08:49" format
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

   
    //SETTINGS - MOVE TO SETTINGS.CS (?)
    public void MusicMuteToggle()
    {
        if (music.mute == true)
        {
            music.mute = false;
            PlayerPrefs.SetInt("Music", 1);
        }
        else if (music.mute == false)
        {
            music.mute = true;
            PlayerPrefs.SetInt("Music", 0);
        }
    }

    public void SoundFXToglle()
    {
        if (soundFX)
        {
            soundFX = false;
            PlayerPrefs.SetInt("SoundFX", 0);
        }   
        else if (!soundFX)
        {
            soundFX = true;
            PlayerPrefs.SetInt("SoundFX", 1);
        }
    }

    public void ShowTimerToggle()
    {
        if (timerBox.activeSelf)
        {
            timerBox.SetActive(false);
            PlayerPrefs.SetInt("ShowTimer", 0);
        }
        else if (!timerBox.activeSelf)
        {
            timerBox.SetActive(true);
            PlayerPrefs.SetInt("ShowTimer", 1);
        }
    }

    public void ShowMovesToggle()
    {
        if (movesBox.activeSelf)
        {
            movesBox.SetActive(false);
            PlayerPrefs.SetInt("ShowMoves", 0);
        }
        else if (!movesBox.activeSelf)
        {
            movesBox.SetActive(true);
            PlayerPrefs.SetInt("ShowMoves", 1);
        }
    }
    //SETTINGS - MOVE TO SETTINGS.CS (?)

    public void StartScreenStartButton()//turn timer back on after pause
    {
        if (elapsedTimeText.text != "")
        {
            startTimer = true;
        }

        startButtonPressedOnce = true;
    }

    public void ChangeTextOnStartButton()
    {
        if (startButtonPressedOnce)
            startButtonOnStartPanel.buttonText = "Resume";
    }
}
