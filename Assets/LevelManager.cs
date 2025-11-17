using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEditor;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance { get; private set; }

    [SerializeField] private bool lastLevel;
    [SerializeField] private int startTime;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private CanvasManager canvasManager;
    
    [Header("SFX")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClipBell;
    [SerializeField] private AudioClip audioClipWhistle;
    [SerializeField] private AudioClip audioClipLose;
    
    [Header("Point to Lose")]
    [SerializeField] private int touchRoad;
    [SerializeField] private int crossWithoutLookingSides;
    
    [Header("Point to Win")]
    [SerializeField] private int crossPedestrianPath;
    [SerializeField] private int lookBothSides;
    
    private int currentTimer;

    private void Awake()
    {
        if (instance != null && instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            instance = this; 
        } 
    }

    private void Start()
    {
        currentTimer = startTime;
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(1);
        currentTimer--;
        if (currentTimer > 0)
        {
            StartCoroutine(Timer());
        }
        else
        {
            currentTimer = 0;
            StopCoroutine(Timer());
            LoseLevel();
        }
        UpdateTimerTMP();
    }

    private void UpdateTimerTMP()
    {
        timerText.text = currentTimer.ToString();
    }
    
    private void FinishLevel()
    {
        GameManager.Instance.totalScore += currentTimer;
        GameManager.Instance.HandleNextLevel();
    }

    private void FinishGame()
    {
        Application.Quit();
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
    }
    private void LoseLevel()
    {
        audioSource.PlayOneShot(audioClipLose);
        GameManager.Instance.ReloadScene();
    }
    
    private void LoseTime(int time)
    {
        audioSource.PlayOneShot(audioClipWhistle);
        currentTimer -= time;
        if (currentTimer <= 0)
        {
            currentTimer = 0;
            LoseLevel();
        }
        UpdateTimerTMP();
    }
    
    private void GainTime(int time)
    {
        audioSource.PlayOneShot(audioClipBell);
        currentTimer += time;
        UpdateTimerTMP();
    }

    public void HandleTouchRoad()
    {
        LoseTime(touchRoad);
        canvasManager.HandleRoadSignFade();
        GameManager.Instance.touchStreetCount++;
    }

    public void HandlePedestrianPath(bool showUI)
    {
        GainTime(crossPedestrianPath);
        if (showUI)
        {
            canvasManager.HandlePedestrianPath();
        }
        GameManager.Instance.crossPedestrianCount++;
    }
    
    public void HandleLookBothSidesGood()
    {
        GainTime(lookBothSides);
    }
    
    public void HandleLookBothSidesBad()
    {
        canvasManager.HandleLookSidesSignFade();
        LoseTime(lookBothSides);
        GameManager.Instance.dontLookSidesCount++;
    }    
    public void HandleLookBothSidesWarning()
    {
        canvasManager.HandleLookSidesWarningSignFade();
    }
    
    public void HandleFinishLevel()
    {
        if (!lastLevel)
        {
            FinishLevel();
            return;
        }
        FinishGame();
    }

    public void HandleRedCrossSign()
    {
        canvasManager.HandleRedCrossSignFade();
    }
}