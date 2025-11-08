using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawnerTrigger : MonoBehaviour
{
    [SerializeField] private CarSpawner carSpawner;
    private void ToggleCarSpawner()
    {
        carSpawner.autoSpawn = !carSpawner.autoSpawn;
    }
    private void OnTriggerEnter(Collider other)
    {
        ToggleCarSpawner();
    }
}