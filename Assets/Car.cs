using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class Car : MonoBehaviour, IActivated
{
  public void Activate()
  {
    this.gameObject.SetActive(true);
  }
}
