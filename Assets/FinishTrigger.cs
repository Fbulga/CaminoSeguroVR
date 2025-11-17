using TMPro;
using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    private void OnTriggerEnter(Collider other)
    {
        audioSource.Play();
        LevelManager.instance.HandleFinishLevel();
    }
}
