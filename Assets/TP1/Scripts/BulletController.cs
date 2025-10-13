using System;
using System.Collections;
using UnityEngine;

namespace TP1.Scripts
{
    [RequireComponent(typeof(LineRenderer))]
    public class BulletController : MonoBehaviour
    {
        [SerializeField] private float laserDistance = 50f;
        [SerializeField] private float speed = 30f;          
        [SerializeField] private float tailLength = 2f;      
        [SerializeField] private LayerMask hitLayers;
        [SerializeField] private ParticleSystem hitEffect;

        private LineRenderer m_lineRenderer;
        public event Action<BulletController> OnDeactivate;

        private void Awake()
        {
            m_lineRenderer = GetComponent<LineRenderer>();
            m_lineRenderer.enabled = false;
        }

        public void Initialize(Transform spawnPoint)
        {
            StartCoroutine(FireLaser(spawnPoint));
        }

        private IEnumerator FireLaser(Transform spawnPoint)
        {
            Vector3 origin = spawnPoint.position;
            Vector3 direction = spawnPoint.forward;

            // Detectar si golpea algo
            bool hitSomething = Physics.Raycast(origin, direction, out RaycastHit hit, laserDistance, hitLayers);
            Vector3 targetPoint = hitSomething ? hit.point : origin + direction * laserDistance;

            m_lineRenderer.enabled = true;
            m_lineRenderer.positionCount = 2;

            Vector3 currentEnd = origin;

            while ((currentEnd - origin).sqrMagnitude < (targetPoint - origin).sqrMagnitude)
            {
                // Avanzar la punta del láser
                currentEnd += direction * speed * Time.deltaTime;

                // Limitar al objetivo
                if ((currentEnd - origin).sqrMagnitude > (targetPoint - origin).sqrMagnitude)
                    currentEnd = targetPoint;

                // Determinar posición de la cola
                Vector3 tailPos = currentEnd - direction * tailLength;

                // Evitar que la cola retroceda más que el origen
                if ((tailPos - origin).sqrMagnitude < 0f)
                    tailPos = origin;

                // Actualizar LineRenderer
                m_lineRenderer.SetPosition(0, tailPos);
                m_lineRenderer.SetPosition(1, currentEnd);

                yield return null;
            }

            // Instanciar efecto de impacto
            if (hitSomething && hitEffect != null)
                Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));

            // Ejemplo: cambiar color del objeto impactado
            if (hitSomething)
            {
                Renderer rend = hit.collider.GetComponent<Renderer>();
                if (rend != null)
                    rend.material.color = Color.red;
            }

            // Esperar un momento antes de desaparecer
            yield return new WaitForSeconds(0.05f);

            m_lineRenderer.enabled = false;

            if (OnDeactivate != null)
                OnDeactivate.Invoke(this);
            else
                Destroy(gameObject);
        }
    }
}

