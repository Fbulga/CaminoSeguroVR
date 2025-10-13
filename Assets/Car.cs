using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class Car : MonoBehaviour, IActivated
{
  Animator animator;

  private void Awake()
  {
    animator = GetComponent<Animator>();
  }

  public void Activate()
  {
    this.gameObject.SetActive(true);
  }
}
