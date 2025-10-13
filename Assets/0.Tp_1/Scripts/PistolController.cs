using TP1.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace Tp_1.Scripts
{
    [RequireComponent(typeof(XRGrabInteractable))]
    public class PistolController : MonoBehaviour
    {
        [SerializeField] private InputActionProperty shootInput;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private BulletController bulletPrefab;
        [SerializeField] private float cooldownShoot;
        
        private XRGrabInteractable m_grabInteractable;
        private PoolGeneric<BulletController> m_bulletPool;
        private bool m_isActivated;
        private float m_timer;

        private void Awake()
        {
            m_bulletPool =  new PoolGeneric<BulletController>(bulletPrefab);
            
            m_grabInteractable = GetComponent<XRGrabInteractable>();
            
            m_grabInteractable.hoverEntered.AddListener(HoverEnteredHandler);
            m_grabInteractable.hoverExited.AddListener(HoverExitedHandler);
        }

        private void HoverExitedHandler(HoverExitEventArgs p_arg0)
        {
            shootInput.action.performed -= ShootInputHandler;
        }

        private void HoverEnteredHandler(HoverEnterEventArgs p_arg0)
        {
            shootInput.action.performed += ShootInputHandler;
        }

        private void ShootInputHandler(InputAction.CallbackContext p_obj)
        {
            if (m_timer > Time.time)
                return;
            
            var l_bullet = m_bulletPool.GetorCreate();
            l_bullet.Initialize(spawnPoint);
            l_bullet.OnDeactivate += BulletOnDeactivateHandler;

            m_timer = Time.time + cooldownShoot;
        }

        private void BulletOnDeactivateHandler(BulletController p_obj)
        {
            m_bulletPool.AddPool(p_obj);
        }
    }
}