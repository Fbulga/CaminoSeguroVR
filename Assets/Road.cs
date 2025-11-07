using UnityEngine;

public class Road : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        LevelManager.instance.HandleTouchRoad();
    }
}