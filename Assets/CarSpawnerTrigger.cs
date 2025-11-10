using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class CarSpawnerTrigger : MonoBehaviour
{
    [SerializeField] private CarSpawner carSpawner;
    [SerializeField] private bool toggleOnce;
    [ShowIf("toggleOnce")] [SerializeField] private bool alreadyToggled;
    private void ToggleCarSpawner()
    {
        carSpawner.autoSpawn = !carSpawner.autoSpawn;
        if (toggleOnce)
        {
            alreadyToggled = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (alreadyToggled)
        {
            return;
        }
        ToggleCarSpawner();
    }
}