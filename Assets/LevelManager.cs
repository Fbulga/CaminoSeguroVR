using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance { get; private set; }

    [SerializeField] private bool lastLevel;
    [SerializeField] private Collider winCollider;
    [SerializeField] private int startTime;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private CanvasManager canvasManager;
    
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
        UpdateTimerTMP();
        if (currentTimer > 0)
        {
            StartCoroutine(Timer());
        }
        else
        {
            StopCoroutine(Timer());
            Debug.Log("Game Over");
        }
    }

    private void UpdateTimerTMP()
    {
        timerText.text = currentTimer.ToString();
    }
    
    private void FinishLevel()
    {
        var finalScore = currentTimer;
    }
    private void LoseLevel()
    {
        
    }
    
    private void LoseTime(int time)
    {
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
        currentTimer += time;
        UpdateTimerTMP();
    }

    public void HandleTouchRoad()
    {
        LoseTime(touchRoad);
        canvasManager.HandleRoadSignFade();
    }

    public void HandlePedestrianPath()
    {
        GainTime(crossPedestrianPath);
        canvasManager.HandlePedestrianPath();
    }
    
    public void HandleLookBothSidesGood()
    {
        GainTime(lookBothSides);
    }
    
    public void HandleLookBothSidesBad()
    {
        LoseTime(lookBothSides);
    }    
    public void HandleLookBothSidesWarning()
    {
        canvasManager.HandleLookSidesSignFade();
    }


    public void HandleFinishLevel()
    {
        FinishLevel();
    }
}