using UnityEngine;

public class PedestrianPath : MonoBehaviour
{
    [SerializeField] private bool showUI = true;
    private bool triggered = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!triggered)
        {
            triggered = true;
            LevelManager.instance.HandlePedestrianPath(showUI);
        }
    }
}