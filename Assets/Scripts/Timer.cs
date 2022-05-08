using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Timer : MonoBehaviour
{

    [SerializeField] private Text elapsedTimerText;
    [SerializeField] private TMP_Text countDownTimerText;

    private bool elapsedTimerRunning = false;
    private float unscaledTime;
    private float currentElapsedTimerValue;
    private int countDownStartTime;
    private bool countDownTimerRunning = true;
    private GameObject startPanel;

    private bool giveOneMagicZap = false;

    // Start is called before the first frame update
    void Start()
    {
        BackGroundManager.bgInstance.Audio.Stop();
        startPanel = GameObject.FindGameObjectWithTag("StartPanel");
        elapsedTimerRunning = false;
        unscaledTime = Time.unscaledTime;

        countDownStartTime = 3;
        countDownTimerRunning = true;
        startPanel.SetActive(true);
        SfxManager.sfxInstance.Audio.PlayOneShot(SfxManager.sfxInstance.Bleep);
        Time.timeScale = 0; // Pause the Game

        giveOneMagicZap = true;
    }
     public void StopTimer()
    {
        elapsedTimerRunning = false;
    }

    public float GetElapsedTime()
    {
        return currentElapsedTimerValue;
    }

    // Update is called once per frame
    void Update()
    {
        if (giveOneMagicZap)
        {
            // This fixed the problem of the PlayerArmature not starting at the correct start location
            Physics.SyncTransforms();
            giveOneMagicZap = false;
        }

        var deltaTime = Time.unscaledTime - unscaledTime;
        if (countDownTimerRunning)
        {
            if (deltaTime >= 1.0)
            {
                countDownStartTime--;
                SfxManager.sfxInstance.Audio.PlayOneShot(SfxManager.sfxInstance.Bleep);
                countDownTimerText.text = countDownStartTime.ToString();
                unscaledTime = Time.unscaledTime;
            }
            if (countDownStartTime <= 0)
            {
                countDownTimerRunning = false;
                elapsedTimerRunning = true;

                Time.timeScale = 1; // Start the Game
                startPanel.SetActive(false);
                BackGroundManager.bgInstance.Audio.Play();
            }
        }
        else if (elapsedTimerRunning)
        {
            deltaTime += Time.deltaTime;
            currentElapsedTimerValue = deltaTime;
            elapsedTimerText.text = "Time: " + TimeSpan.FromSeconds(deltaTime).ToString("mm':'ss'.'ff");
        }        
    }
}
