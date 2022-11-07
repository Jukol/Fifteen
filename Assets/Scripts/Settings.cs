using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    private static Settings _instance;
    public static Settings Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Settings is NULL.");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    private string topTimeVarName = "TopTime";
    private string topMovesVarName = "TopMoves";
    private string musicVarName = "Music";
    private string SoundFXVarName = "SoundFX";
    private string showTimerVarName = "ShowTimer";
    private string showMovesVarName = "ShowMoves";

    [SerializeField] private GameObject timerBox, movesBox;

    [SerializeField] private Toggle musicToggle, SoundFXToggle, showTimerToglle, showMovesToggle;

    [SerializeField] private Text topTimeScoreText;


    private void Start()
    {
        if (!PlayerPrefs.HasKey(topTimeVarName))
        {
            PlayerPrefs.SetFloat(topTimeVarName, 1000000f);
        }

        if (!PlayerPrefs.HasKey(topMovesVarName))
        {
            PlayerPrefs.SetInt(topMovesVarName, Int32.MaxValue);
        }
            

        if (!PlayerPrefs.HasKey(musicVarName))
        {
            PlayerPrefs.SetInt(musicVarName, 1);
        }
            

        if (!PlayerPrefs.HasKey(SoundFXVarName))
        {
            PlayerPrefs.SetInt(SoundFXVarName, 1);
        }
            

        if (!PlayerPrefs.HasKey(showTimerVarName))
        {
            PlayerPrefs.SetInt(showTimerVarName, 1);
        }
            

        if (!PlayerPrefs.HasKey(showMovesVarName))
        {
            PlayerPrefs.SetInt(showMovesVarName, 1);
        }
            

        if (PlayerPrefs.GetInt(musicVarName) == 0)
        {
            musicToggle.isOn = false;
        }
        else if (PlayerPrefs.GetInt(musicVarName) == 1)
        {
            musicToggle.isOn = true;
        }
            

        if (PlayerPrefs.GetInt(SoundFXVarName) == 0)
        {
            SoundFXToggle.isOn = false;
            GameManager.Instance.SoundFX = false;
        }
        else if (PlayerPrefs.GetInt(SoundFXVarName) == 1)
        {
            SoundFXToggle.isOn = true;
            GameManager.Instance.SoundFX = true;
        }

        if (PlayerPrefs.GetInt(showTimerVarName) == 0)
        {
            showTimerToglle.isOn = false;
            timerBox.SetActive(false);
        }
        else if (PlayerPrefs.GetInt(showTimerVarName) == 1)
        {
            showTimerToglle.isOn = true;
            timerBox.SetActive(true);
        }

        if (PlayerPrefs.GetInt(showMovesVarName) == 0)
        {
            showMovesToggle.isOn = false;
            movesBox.SetActive(false);
        }
        else if (PlayerPrefs.GetInt(showMovesVarName) == 1)
        {
            showMovesToggle.isOn = true;
            movesBox.SetActive(true);
        }
    }


    public void HighScore()
    {
        if (UIManager.Instance.elapsedTime < PlayerPrefs.GetFloat(topTimeVarName) && 
            GameManager.Instance.Moves < PlayerPrefs.GetInt(topMovesVarName))
        {
            PlayerPrefs.SetFloat(topTimeVarName, UIManager.Instance.elapsedTime);
            PlayerPrefs.SetInt(topMovesVarName, GameManager.Instance.Moves);
            UIManager.Instance.ShowBestTimeAndMovesPanel();
        }
        else if (UIManager.Instance.elapsedTime < PlayerPrefs.GetFloat(topTimeVarName))
        {
            PlayerPrefs.SetFloat(topTimeVarName, UIManager.Instance.elapsedTime);
            UIManager.Instance.ShowBestTimePanel();
        }
        else if (GameManager.Instance.Moves < PlayerPrefs.GetInt(topMovesVarName))
        {
            PlayerPrefs.SetInt(topMovesVarName, GameManager.Instance.Moves);
            UIManager.Instance.ShowBestMovesPanel();
        }
        else
        {
            UIManager.Instance.ShowGoodJobPanel();
        }
    }


    public void ShowScoreText()
    {
        string timeString;
        string movesString;

        TimeSpan highScoreTime = TimeSpan.FromSeconds(PlayerPrefs.GetFloat(topTimeVarName));

        if (PlayerPrefs.GetFloat(topTimeVarName) == 1000000f)
            timeString = "";
        else
            timeString = highScoreTime.ToString("hh':'mm':'ss':'ff");

        if (PlayerPrefs.GetInt(topMovesVarName) == Int32.MaxValue)
            movesString = "";
        else
            movesString = PlayerPrefs.GetInt("TopMoves").ToString();

        topTimeScoreText.text = "Best time: " + timeString + "\n" + "Best moves: " + movesString;
    }

    public void MusicMuteToggle()
    {
        if (GameManager.Instance.Music.mute)
        {
            GameManager.Instance.Music.mute = false;
            PlayerPrefs.SetInt(musicVarName, 1);
        }
        else if (!GameManager.Instance.Music.mute)
        {
            GameManager.Instance.Music.mute = true;
            PlayerPrefs.SetInt(musicVarName, 0);
        }
    }

    


}
