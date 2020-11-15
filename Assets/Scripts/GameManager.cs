using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    Transform[] cells = new Transform[16];

    [SerializeField]
    GameObject[] pieces = new GameObject[15];
    GameObject piece01, piece02, piece03, piece04, piece05, piece06, piece07, piece08, piece09, piece10, piece11, piece12, piece13, piece14, piece15;
    [SerializeField]
    Text winText, hoursText, minutesText, secondsText, tenthsOfASecondText;
    [SerializeField]
    GameObject timer;

    int hours, minutes, seconds, tenthsOfASecond;
    float startTime;
    bool startTimer;


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
    }

    private void Update()
    {
        if (startTimer)
            Timer();
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

        for (int i = 0; i < pieces.Length; i++)
        {
            pieces[i].transform.position = cells[i].transform.position;
        }

        winText.gameObject.SetActive(false);
        startTime = Time.time;
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
            winText.gameObject.SetActive(true);
        }
    }

    public void Timer()
    {
        hours = (int)((Time.time - startTime)/3600);
        minutes = (int)((Time.time - startTime) / 60 - (Time.time - startTime) / 3600);
        if (minutes >= 60)
            minutes = minutes % 60;

        seconds = (int)((Time.time - startTime) - (Time.time - startTime) / 60 - (Time.time - startTime) / 3600);
        if (seconds >= 60)
            seconds = seconds % 60;

        tenthsOfASecond = (int)((Time.time - startTime) * 10 - (Time.time - startTime) - (Time.time - startTime) / 60 - (Time.time - startTime) / 3600);
        if (tenthsOfASecond >= 10)
            tenthsOfASecond = tenthsOfASecond % 10;

        
        if (hours < 10)
            hoursText.text = "0" + hours.ToString();
        else
            hoursText.text = hours.ToString();

        if (minutes < 10)
            minutesText.text = "0" + minutes.ToString();
        else
            minutesText.text = minutes.ToString();
        
        if (seconds < 10)
            secondsText.text = "0" + seconds.ToString();
        else
            secondsText.text = seconds.ToString();
        
        if (tenthsOfASecond < 10)
            tenthsOfASecondText.text = "0" + tenthsOfASecond.ToString();
        else
            tenthsOfASecondText.text = tenthsOfASecond.ToString();
    }

}
