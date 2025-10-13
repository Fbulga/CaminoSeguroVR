using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class Semaforo : MonoBehaviour, IActivated
{
    [SerializeField] private ParticleSystem particles; 
    private Material instanceMaterial;
    public void Activate()
    {
        var renderer = GetComponent<Renderer>();
        var originalMaterial = renderer.sharedMaterial;
        instanceMaterial = new Material(originalMaterial);
        renderer.material = instanceMaterial;
        instanceMaterial.color = Color.green;
        
        particles.gameObject.SetActive(true);
        particles.Play();
        
        
    }
}
