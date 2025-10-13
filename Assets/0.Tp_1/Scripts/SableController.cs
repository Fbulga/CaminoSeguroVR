using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace Tp_1.Scripts
{
    [RequireComponent(typeof(XRGrabInteractable))]
    public class SableController : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField] private InputActionProperty activateInput;

        [Header("Saber Settings")]
        [SerializeField] private Transform sableTransform;
        [SerializeField] private float sableLarge;
        [SerializeField] private ParticleSystem sableParticles;

        
        
        private XRGrabInteractable m_grabInteractable;
        private bool m_isActivated;
        private float m_oldScale = 0.08f;

        private AudioSource m_audioSource;
        private void Awake()
        {
            m_grabInteractable = GetComponent<XRGrabInteractable>();
            
            m_grabInteractable.selectEntered.AddListener(SelectEnteredHandler);
            m_grabInteractable.selectExited.AddListener(SelectExitedHandler);
            m_oldScale = sableTransform.localScale.y;
            
            m_audioSource = GetComponent<AudioSource>();
        }
        
        private void SelectExitedHandler(SelectExitEventArgs p_arg0)
        {
            activateInput.action.performed -= ActiveSableInputHandler;
            //DeactivateSable();
        }

        private void SelectEnteredHandler(SelectEnterEventArgs p_arg0)
        {
            activateInput.action.performed += ActiveSableInputHandler;
        }

        private void ActiveSableInputHandler(InputAction.CallbackContext p_obj)
        {
            if (m_isActivated)
                DeactivateSable();
            else 
                ActiveSable();
        }

        private void ActiveSable()
        {
            m_isActivated = true;
            m_audioSource.Play();
            sableParticles.Play();
            sableTransform.localScale = new Vector3(sableTransform.localScale.x, sableLarge, sableTransform.localScale.z);
            sableTransform.localPosition = Vector3.up * sableLarge;
        }

        private void DeactivateSable()
        {
            m_isActivated = false;
            sableTransform.localScale = new Vector3(sableTransform.localScale.x, m_oldScale, sableTransform.localScale.z);
            sableTransform.localPosition = Vector3.zero;
        }

    }
}