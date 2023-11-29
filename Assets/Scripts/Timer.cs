using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float timeLeft;
    public bool timerOn = false;
    public TextMeshProUGUI timerText;
    public LevelLoaderScript levelLoader;

    // Start is called before the first frame update
    void Start()
    {
        timerOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerOn == true)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                updateTimer(timeLeft);
            } else
            {
                Debug.Log("End");
                timeLeft = 0;
                timerOn = false;
                levelLoader.loadRunnerEndScreen();
            }
        }
    }

    void updateTimer(float currentTime)
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        timerText.text = "Timer: " + minutes + ":" + seconds;
    }
}
