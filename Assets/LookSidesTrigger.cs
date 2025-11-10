using UnityEngine;

public class LookSidesTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        LevelManager.instance.HandleLookBothSidesWarning();
    }
}