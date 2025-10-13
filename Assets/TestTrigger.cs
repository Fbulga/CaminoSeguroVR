using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Events;

public class TestTrigger : MonoBehaviour
{
    [SerializeField] UnityEvent onPlayerEnter;
    [SerializeField] public GameObject activateGO;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered");
            onPlayerEnter?.Invoke();
        }

        if (activateGO != null)
        {
            if(activateGO.TryGetComponent<IActivated>(out IActivated activated))
            {
                activated.Activate();
            }
        }
    }
    
}
