using UnityEngine;

public class PedestrianPath : MonoBehaviour
{
    private bool triggered = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!triggered)
        {
            triggered = true;
            LevelManager.instance.HandlePedestrianPath();
        }
    }
}