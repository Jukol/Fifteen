using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("UI Managers is NULL!");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    [SerializeField] private GameObject _startPanel;
    [SerializeField] private GameObject _unsolvablePanel;
    private Vector3 _beyondScreen;
    private Animator _startPanelAnim;
    public float elapsedTime { get; set; }
    private TimeSpan _timeSpan;
    [SerializeField] private Text _timerText, _bestTimeScoreText;
    private bool _timerGoing;
    [SerializeField] private Text _movesText, _bestMovesText, _bestTimeForDoublePanelText, _bestMovesForDoublePanelText;
    [SerializeField] private GameObject _timerBox, _movesBox;
    [SerializeField] private GameObject _bestTimePanel, _bestMovesPanel, _bestTimeAndMovesPanel;
    [SerializeField] private GameObject _goodJobPanel;


    private void Start()
    {
        _startPanel.SetActive(true);
        _startPanelAnim = _startPanel.GetComponent<Animator>();
        _beyondScreen = new Vector3(-1094, 0, 0);
    }

    public void StartPanelMoveOut ()
    {
        
        _startPanelAnim.SetTrigger("Out");
    }

    public void StartPanelMoveIn()
    {
        _startPanelAnim.SetTrigger("In");
    }

    public void StartTimer ()
    {
        _timerGoing = true;
        elapsedTime = 0f;
        StartCoroutine(UpdateTimerProcedure());
    }

    public void StopTimer()
    {
        _timerGoing = false;
    }

    IEnumerator UpdateTimerProcedure ()
    {
        while (_timerGoing)
        {
            elapsedTime += Time.deltaTime;
            _timeSpan = TimeSpan.FromSeconds(elapsedTime);
            _timerText.text = "Time: " + _timeSpan.ToString("hh':'mm':'ss':'ff");
            yield return null;
        }
    }

    public void UpdateMovesText(int moves)
    {
        _movesText.text = "Moves: " + moves;
    }

    public void ShowUnsolvablePanel()
    {
        _unsolvablePanel.SetActive(true);
    }

    public void TimerToggle()
    {
        if (_timerBox.activeSelf == true)
        {
            _timerBox.SetActive(false);
            PlayerPrefs.SetInt("ShowTimer", 0);
        }
        else if (_timerBox.activeSelf == false)
        {
            _timerBox.SetActive(true);
            PlayerPrefs.SetInt("ShowTimer", 1);
        }
    }

    public void MovesToggle()
    {
        if (_movesBox.activeSelf == true)
        {
            _movesBox.SetActive(false);
            PlayerPrefs.SetInt("ShowMoves", 0);
        }
        else if (_movesBox.activeSelf == false)
        {
            _movesBox.SetActive(true);
            PlayerPrefs.SetInt("ShowMoves", 1);
        }
    }

    public void SoundFXToggle()
    {
        if (GameManager.Instance.SoundFX)
        {
            GameManager.Instance.SoundFX = false;
            PlayerPrefs.SetInt("SoundFX", 0);
        }
        else if (!GameManager.Instance.SoundFX)
        {
            GameManager.Instance.SoundFX = true;
            PlayerPrefs.SetInt("SoundFX", 1);
        }
    }

    public void ShowBestTimePanel()
    {
        _bestTimePanel.SetActive(true);
        _bestTimeScoreText.text = _timeSpan.ToString("hh':'mm':'ss':'ff");
    }

    public void ShowBestMovesPanel()
    {
        _bestMovesPanel.SetActive(true);
        _bestMovesText.text = GameManager.Instance.Moves.ToString();
    }

    public void ShowBestTimeAndMovesPanel()
    {
        _bestTimeAndMovesPanel.SetActive(true);
        _bestTimeForDoublePanelText.text = "Time: " + _timeSpan.ToString("hh':'mm':'ss':'ff");
        _bestMovesForDoublePanelText.text = "Moves: " + GameManager.Instance.Moves.ToString();
    }

    public void ShowGoodJobPanel()
    {
        _goodJobPanel.SetActive(true);
    }

}
