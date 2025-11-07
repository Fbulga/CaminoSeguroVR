using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        LevelManager.instance.HandleFinishLevel();
    }
}
