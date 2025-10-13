using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Tp_1.Scripts
{
    [RequireComponent(typeof(XRSimpleInteractable))]
    public class ButtonActivator : MonoBehaviour
    {
        [Header("Objects to Activate/Toggle")]
        [SerializeField] private GameObject[] objectsToToggle;

        [Header("Settings")]
        [SerializeField] private bool toggleMode = true; // true = toggle on/off, false = solo activar

        private XRSimpleInteractable m_simpleInteractable;
        private bool m_isOn = false;

        private void Awake()
        {
            m_simpleInteractable = GetComponent<XRSimpleInteractable>();
        }

        private void Start()
        {
            InitializeObjectsAsOff();
        }

        private void OnEnable()
        {
            m_simpleInteractable.selectEntered.AddListener(OnButtonPressed);
        }

        private void OnDisable()
        {
            m_simpleInteractable.selectEntered.RemoveListener(OnButtonPressed);
        }

        private void OnButtonPressed(SelectEnterEventArgs args)
        {
            Debug.Log($"Button {gameObject.name} activated!");
            ToggleObjects();
        }

        private void ToggleObjects()
        {
            if (toggleMode)
            {
                m_isOn = !m_isOn;
            }
            else
            {
                m_isOn = true;
            }

            foreach (GameObject obj in objectsToToggle)
            {
                if (obj != null)
                {
                    obj.SetActive(m_isOn);
                }
            }

            if (objectsToToggle.Length > 0)
            {
                Debug.Log($"Objects turned {(m_isOn ? "ON" : "OFF")}");
            }
        } 
        private void InitializeObjectsAsOff()
        {
            m_isOn = false;

            // Apagar todos los objetos
            foreach (GameObject obj in objectsToToggle)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                }
            }

            Debug.Log($"Button {gameObject.name}: Objects initialized as OFF");
        }
    }
}