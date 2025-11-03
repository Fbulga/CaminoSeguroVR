using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class LookSidesController : MonoBehaviour
{
    [SerializeField] private SideTrigger[] sideTriggers;
    
    [SerializeField] private float loseDelayTime;
    
    private Coroutine coroutine;

    private void OnTriggerEnter(Collider other)
    {
        coroutine = StartCoroutine(CrossCheck());
    }

    private void OnTriggerExit(Collider other)
    {
        StopCoroutine(coroutine);
    }
    
    IEnumerator CrossCheck()
    {
        if (!sideTriggers[0].sideCheck && !sideTriggers[1].sideCheck)
        {
            Debug.Log("Side/s Unchecked found");
            yield return new WaitForSeconds(loseDelayTime);
            Debug.Log("Lose");
        }
        else
        {
            yield return null; 
        }
    }
    
}
