using System.Collections;
using UnityEngine;

public class SideTrigger : MonoBehaviour
{
    [SerializeField] public bool sideCheck;
    
    [SerializeField] private float uncheckDelayTime;
    
    public void SideChecked()
    {
        sideCheck = true;
        StartCoroutine(UncheckDelay());
    }

    public void UncheckSide()
    {
        sideCheck = false;
    }

    IEnumerator UncheckDelay()
    {
        yield return new WaitForSeconds(uncheckDelayTime);
        UncheckSide();
    }
}