using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class ActivateTrigger : MonoBehaviour
{
    [Header("Referencias")] 
    [SerializeField] private bool activateCarMover;
    [ShowIf("activateCarMover")]public CarMover car;
    [SerializeField] private bool activateCarMoverRedLight;
    [ShowIf("activateCarMoverRedLight")]public TrafficLightController trafficLight;
    
    private bool triggered = false;
    
    private void Activate()
    {
        if (triggered)
        {
            Debug.Log($"{name}: Ya fue activado una vez, ignorando.");
            return;
        }

        triggered = true;

        // 🚦 Cambiar semáforo a rojo
        if (trafficLight != null)
        {
            trafficLight.SetRedForA();
            Debug.Log($"{name}: Semáforo puesto en rojo (SetRedForA).");
        }
        else
        {
            Debug.LogWarning($"{name}: No se asignó ningún TrafficLightController.");
        }

        // 🚗 Iniciar auto que obedece semáforo
        if (car != null)
        {
            car.obeyTrafficLight = true;
            car.trafficLight = trafficLight;
            car.StartDriving();
            Debug.Log($"{name}: Auto '{car.name}' activado y configurado para obedecer semáforo.");
        }
        else
        {
            Debug.LogWarning($"{name}: No se asignó ningún CarMover.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Activate();
    }
}