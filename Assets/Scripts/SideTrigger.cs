using System.Collections;
using UnityEngine;

public class SideTrigger : MonoBehaviour
{
    public bool sideCheck {get; private set; }
    
    [SerializeField] private float uncheckDelayTime;
    
    public void SideChecked()
    {
        Debug.Log("Side check triggered");
        sideCheck = true;
        StartCoroutine(UncheckDelay());
    }

    public void UncheckSide()
    {
        Debug.Log("Uncheck side triggered");
        sideCheck = false;
    }

    IEnumerator UncheckDelay()
    {
        yield return new WaitForSeconds(uncheckDelayTime);
        UncheckSide();
    }
}