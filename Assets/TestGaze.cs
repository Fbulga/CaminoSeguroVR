using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGaze : MonoBehaviour
{
    [SerializeField] private ParticleSystem m_psSelected;
    [SerializeField] private ParticleSystem m_psDeselected;
    public void Selected()
    {
        m_psSelected.Play();
    }

    public void Deselected()
    {
        m_psDeselected.Play();
    }
}
