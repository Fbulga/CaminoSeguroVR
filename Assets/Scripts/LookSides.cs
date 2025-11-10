using NaughtyAttributes;
using UnityEngine;



public class LookSidesController : MonoBehaviour
{
    [SerializeField] private SideTrigger[] sideTriggers;
    [SerializeField] private bool toggleOnce;
    [ShowIf("toggleOnce")] [SerializeField] private bool alreadyToggled;
    private void OnTriggerEnter(Collider other)
    {
        if (alreadyToggled)
        {
            return;
        }
        if (!sideTriggers[0].sideCheck && !sideTriggers[1].sideCheck)
        {
            Debug.Log("Side/s Unchecked found");
            LevelManager.instance.HandleLookBothSidesBad();
            return;
        }
        LevelManager.instance.HandleLookBothSidesGood();
        if (toggleOnce)
        {
            alreadyToggled = true;
        }
    }
}