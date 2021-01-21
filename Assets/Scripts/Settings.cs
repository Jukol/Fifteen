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
    private string soundFXVarName = "SoundFX";
    private string showTimerVarName = "ShowTimer";
    private string showMovesVarName = "ShowMoves";

    [SerializeField] private GameObject timerBox, movesBox;

    [SerializeField] private Toggle musicToggle, soundFXToggle, showTimerToglle, showMovesToggle;

    [SerializeField] private Text topTimeScoreText;


    private void Start()
    {
        if (!PlayerPrefs.HasKey(topTimeVarName))
        {
            PlayerPrefs.SetFloat(topTimeVarName, float.MaxValue);
        }

        if (!PlayerPrefs.HasKey(topMovesVarName))
        {
            PlayerPrefs.SetInt(topMovesVarName, Int32.MaxValue);
        }
            

        if (!PlayerPrefs.HasKey(musicVarName))
        {
            PlayerPrefs.SetInt(musicVarName, 1);
        }
            

        if (!PlayerPrefs.HasKey(soundFXVarName))
        {
            PlayerPrefs.SetInt(soundFXVarName, 1);
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
            //GameManager2.Instance.Music.mute = true;
        }
        else if (PlayerPrefs.GetInt(musicVarName) == 1)
        {
            musicToggle.isOn = true;
            //GameManager2.Instance.Music.mute = true;
        }
            

        if (PlayerPrefs.GetInt(soundFXVarName) == 0)
        {
            soundFXToggle.isOn = false;
            GameManager2.Instance.soundFX = false;
        }
        else if (PlayerPrefs.GetInt(soundFXVarName) == 1)
        {
            soundFXToggle.isOn = true;
            GameManager2.Instance.soundFX = true;
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
            GameManager2.Instance.moves < PlayerPrefs.GetInt(topMovesVarName))
        {
            PlayerPrefs.SetFloat(topTimeVarName, UIManager.Instance.elapsedTime);
            PlayerPrefs.SetInt(topMovesVarName, GameManager2.Instance.moves);
            UIManager.Instance.ShowBestTimeAndMovesPanel();
        }
        else if (UIManager.Instance.elapsedTime < PlayerPrefs.GetFloat(topTimeVarName))
        {
            PlayerPrefs.SetFloat(topTimeVarName, UIManager.Instance.elapsedTime);
            UIManager.Instance.ShowBestTimePanel();
        }
        else if (GameManager2.Instance.moves < PlayerPrefs.GetInt(topMovesVarName))
        {
            PlayerPrefs.SetInt(topMovesVarName, GameManager2.Instance.moves);
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
        TimeSpan elapsedTime = TimeSpan.FromSeconds(UIManager.Instance.elapsedTime);


        if (PlayerPrefs.GetFloat(topTimeVarName) == Int32.MaxValue)
            timeString = "";
        else
            timeString = elapsedTime.ToString("hh':'mm':'ss':'ff");

        if (PlayerPrefs.GetInt(topMovesVarName) == Int32.MaxValue)
            movesString = "";
        else
            movesString = PlayerPrefs.GetInt("TopMoves").ToString();

        topTimeScoreText.text = "Best time: " + timeString + "\n" + "Best moves: " + movesString;
    }

    public void MusicMuteToggle()
    {
        if (GameManager2.Instance.Music.mute)
        {
            GameManager2.Instance.Music.mute = false;
            PlayerPrefs.SetInt(musicVarName, 1);
        }
        else if (!GameManager2.Instance.Music.mute)
        {
            GameManager2.Instance.Music.mute = true;
            PlayerPrefs.SetInt(musicVarName, 0);
        }
    }

    public void SoundFXToglle()
    {
        if (GameManager2.Instance.soundFX)
        {
            GameManager2.Instance.soundFX = false;
            PlayerPrefs.SetInt(soundFXVarName, 0);
        }
        else if (!GameManager2.Instance.soundFX)
        {
            GameManager2.Instance.soundFX = true;
            PlayerPrefs.SetInt(soundFXVarName, 1);
        }
    }


}
