using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Tp_1.Scripts
{
    [RequireComponent(typeof(XRSocketInteractor))]
    public class SocketController : MonoBehaviour
    {
        [Header("Socket Visual Feedback")]
        [SerializeField] private Material normalMaterial;
        [SerializeField] private Material occupiedMaterial; // Material verde cuando está ocupado

        private XRSocketInteractor m_socketInteractor;
        private MeshRenderer m_meshRenderer;

        private void Awake()
        {
            m_socketInteractor = GetComponent<XRSocketInteractor>();
            m_meshRenderer = GetComponent<MeshRenderer>();

            // Guardar el material original si no está asignado
            if (normalMaterial == null)
                normalMaterial = m_meshRenderer.material;
        }

        private void OnEnable()
        {
            m_socketInteractor.selectEntered.AddListener(OnObjectPlaced);
            m_socketInteractor.selectExited.AddListener(OnObjectRemoved);
        }

        private void OnDisable()
        {
            m_socketInteractor.selectEntered.RemoveListener(OnObjectPlaced);
            m_socketInteractor.selectExited.RemoveListener(OnObjectRemoved);
        }

        private void OnObjectPlaced(SelectEnterEventArgs args)
        {
            // Cambiar a material verde cuando se coloca un objeto
            if (occupiedMaterial != null)
            {
                m_meshRenderer.material = occupiedMaterial;
            }

            Debug.Log($"Objeto colocado en socket: {args.interactableObject.transform.name}");
        }

        private void OnObjectRemoved(SelectExitEventArgs args)
        {
            // Volver al material normal cuando se retira el objeto
            if (normalMaterial != null)
            {
                m_meshRenderer.material = normalMaterial;
            }

            Debug.Log($"Objeto retirado del socket: {args.interactableObject.transform.name}");
        }
    }
}