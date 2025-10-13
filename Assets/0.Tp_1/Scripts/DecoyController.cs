using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Tp_1.Scripts
{
    [RequireComponent(typeof(XRSimpleInteractable))]
    [RequireComponent(typeof(MeshRenderer))]
    public class DecoyController : MonoBehaviour
    {
        [Header("Visual Feedback")]
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color hoverColor = Color.yellow;

        [Header("Message")]
        [SerializeField] private string decoyMessage = "Este objeto no sirve para la misión";

        private XRSimpleInteractable m_simpleInteractable;
        private MeshRenderer m_meshRenderer;
        private Material m_material;

        private void Awake()
        {
            m_simpleInteractable = GetComponent<XRSimpleInteractable>();
            m_meshRenderer = GetComponent<MeshRenderer>();

            // Crear una instancia del material para no afectar el original
            m_material = m_meshRenderer.material;
            m_material.color = normalColor;
        }

        private void OnEnable()
        {
            m_simpleInteractable.hoverEntered.AddListener(OnHoverEntered);
            m_simpleInteractable.hoverExited.AddListener(OnHoverExited);
            m_simpleInteractable.selectEntered.AddListener(OnSelectEntered);
        }

        private void OnDisable()
        {
            m_simpleInteractable.hoverEntered.RemoveListener(OnHoverEntered);
            m_simpleInteractable.hoverExited.RemoveListener(OnHoverExited);
            m_simpleInteractable.selectEntered.RemoveListener(OnSelectEntered);
        }

        private void OnHoverEntered(HoverEnterEventArgs args)
        {
            // Cambiar color al hacer hover
            m_material.color = hoverColor;
        }

        private void OnHoverExited(HoverExitEventArgs args)
        {
            // Volver al color normal al salir del hover
            m_material.color = normalColor;
        }

        private void OnSelectEntered(SelectEnterEventArgs args)
        {
            // SOLO aquí aparece el mensaje importante según la consigna
            Debug.Log(decoyMessage);
        }
    }
}