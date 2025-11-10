using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;


public class LookSidesController : MonoBehaviour
{
    [SerializeField] private SideTrigger[] sideTriggers;
    [SerializeField] private bool toggleOnce;
    [ShowIf("toggleOnce")] [SerializeField] private bool alreadyToggled;
    
    [Header("Activate")]
    [SerializeField] private bool activateCarSpawner;
    [ShowIf("activateCarSpawner")] [SerializeField] private CarSpawner carSpawner;
    [SerializeField] private bool redCrossSign;
    
    private void OnTriggerEnter(Collider other)
    {
        if (alreadyToggled)
        {
            return;
        }
        if (toggleOnce)
        {
            alreadyToggled = true;
        }
        if (!sideTriggers[0].sideCheck && !sideTriggers[1].sideCheck)
        {
            Debug.Log("Side/s Unchecked found");
            LevelManager.instance.HandleLookBothSidesBad();
            if (activateCarSpawner)
            {
                carSpawner.autoSpawn = true;
                if (redCrossSign)
                {
                    LevelManager.instance.HandleRedCrossSign();
                }
            }

            return;
        }
        LevelManager.instance.HandleLookBothSidesGood();
        if (toggleOnce)
        {
            alreadyToggled = true;
        }
    }
}