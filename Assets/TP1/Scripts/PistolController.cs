using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace TP1.Scripts
{
    [RequireComponent(typeof(XRGrabInteractable))]
    [RequireComponent(typeof(AudioSource))]
    public class PistolController : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField] private InputActionProperty shootInput;

        [Header("Bullet Settings")]
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private BulletController bulletPrefab;
        [SerializeField] private float cooldownShoot = 0.2f;

        [Header("FX")]
        [SerializeField] private ParticleSystem muzzleFlash;
        [SerializeField] private AudioClip shootSound;

        [Header("Haptics")]
        [SerializeField] private float hapticIntensity = 0.5f;
        [SerializeField] private float hapticDuration = 0.1f;

        private XRGrabInteractable m_grabInteractable;
        private PoolGeneric<BulletController> m_bulletPool;
        private float m_timer;
        private AudioSource m_audioSource;

        private void Awake()
        {
            m_bulletPool = new PoolGeneric<BulletController>(bulletPrefab);
            m_grabInteractable = GetComponent<XRGrabInteractable>();
            m_audioSource = GetComponent<AudioSource>();

            m_grabInteractable.selectEntered.AddListener(SelectEnteredHandler);
            m_grabInteractable.selectExited.AddListener(SelectExitedHandler);
        }

        private void OnDestroy()
        {
            m_grabInteractable.selectEntered.RemoveListener(SelectEnteredHandler);
            m_grabInteractable.selectExited.RemoveListener(SelectExitedHandler);
        }

        private void SelectEnteredHandler(SelectEnterEventArgs args)
        {
            shootInput.action.performed += ShootInputHandler;
        }

        private void SelectExitedHandler(SelectExitEventArgs args)
        {
            shootInput.action.performed -= ShootInputHandler;
        }

        private void ShootInputHandler(InputAction.CallbackContext obj)
        {
            if (m_timer > Time.time)
                return;

            var l_bullet = m_bulletPool.GetorCreate();
            l_bullet.Initialize(spawnPoint);
            l_bullet.OnDeactivate += BulletOnDeactivateHandler;

            if (muzzleFlash != null)
                muzzleFlash.Play();

            if (shootSound != null)
                m_audioSource.PlayOneShot(shootSound);

            // Haptic feedback
            TriggerHapticFeedback();

            m_timer = Time.time + cooldownShoot;
        }

        private void BulletOnDeactivateHandler(BulletController p_obj)
        {
            m_bulletPool.AddPool(p_obj);
        }

        private void TriggerHapticFeedback()
        {
            // Obtener el interactor que está agarrando la pistola
            if (m_grabInteractable.isSelected && m_grabInteractable.interactorsSelecting.Count > 0)
            {
                var interactor = m_grabInteractable.interactorsSelecting[0];

                // Verificar si es un XRBaseControllerInteractor para acceso a haptics
                if (interactor is XRBaseControllerInteractor controllerInteractor)
                {
                    controllerInteractor.SendHapticImpulse(hapticIntensity, hapticDuration);
                }
            }
        }
    }
}
