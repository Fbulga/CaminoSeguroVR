using System;
using UnityEngine;

namespace Tp_1.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class BulletController : MonoBehaviour
    {
        [SerializeField] private float forceImpulse;
        private Rigidbody m_rigidbody;

        public event Action<BulletController> OnDeactivate;

        private void Awake()
        {
            m_rigidbody = GetComponent<Rigidbody>();
        }

        public void Initialize(Transform p_spawnPoint)
        {
            transform.position = p_spawnPoint.position;
            gameObject.SetActive(true);
            m_rigidbody.AddForce(p_spawnPoint.forward * forceImpulse, ForceMode.Impulse);
        }

        private void OnCollisionEnter(Collision p_other)
        {
            if (OnDeactivate == null)
            {
                Destroy(gameObject);
                return;
            }
            gameObject.SetActive(false);
            OnDeactivate.Invoke(this);
        }
    }
}