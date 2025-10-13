using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Tp_1.Scripts
{
    [RequireComponent(typeof(XRSimpleInteractable))]
    public class PokeButton : MonoBehaviour
    {
        [Header("Button Settings")]
        [SerializeField] private Transform buttonTransform;
        [SerializeField] private float pressDepth = 0.02f;
        [SerializeField] private float returnSpeed = 5f;

        [Header("Visual Feedback")]
        [SerializeField] private Color normalColor = Color.green;
        [SerializeField] private Color pressedColor = Color.red;
        [SerializeField] private Light[] lightsToToggle;

        [Header("Effects")]
        [SerializeField] private GameObject[] enemiesToSpawn;
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private ParticleSystem activationEffect;
        [SerializeField] private AudioClip buttonSound;

        private XRSimpleInteractable m_simpleInteractable;
        private Vector3 m_originalPosition;
        private Vector3 m_pressedPosition;
        private bool m_isPressed = false;
        private bool m_lightsOn = false;
        private Renderer m_buttonRenderer;
        private Material m_buttonMaterial;
        private AudioSource m_audioSource;

        private void Awake()
        {
            m_simpleInteractable = GetComponent<XRSimpleInteractable>();

            // Configurar posiciones del botón
            if (buttonTransform == null)
                buttonTransform = transform;

            m_originalPosition = buttonTransform.localPosition;
            m_pressedPosition = m_originalPosition + Vector3.down * pressDepth;

            // Configurar material del botón
            m_buttonRenderer = GetComponent<Renderer>();
            if (m_buttonRenderer != null)
            {
                m_buttonMaterial = m_buttonRenderer.material;
                m_buttonMaterial.color = normalColor;
            }

            // Audio source
            m_audioSource = GetComponent<AudioSource>();
            if (m_audioSource == null)
                m_audioSource = gameObject.AddComponent<AudioSource>();
        }

        private void OnEnable()
        {
            m_simpleInteractable.selectEntered.AddListener(OnButtonPressed);
            m_simpleInteractable.selectExited.AddListener(OnButtonReleased);
        }

        private void OnDisable()
        {
            m_simpleInteractable.selectEntered.RemoveListener(OnButtonPressed);
            m_simpleInteractable.selectExited.RemoveListener(OnButtonReleased);
        }

        private void Update()
        {
            // Animar el retorno del botón
            if (!m_isPressed)
            {
                buttonTransform.localPosition = Vector3.Lerp(
                    buttonTransform.localPosition,
                    m_originalPosition,
                    Time.deltaTime * returnSpeed
                );
            }
        }

        private void OnButtonPressed(SelectEnterEventArgs args)
        {
            // Solo activar con Poke Interactor
            if (IsPokeInteractor(args.interactorObject))
            {
                m_isPressed = true;
                buttonTransform.localPosition = m_pressedPosition;

                // Cambiar color del botón
                if (m_buttonMaterial != null)
                    m_buttonMaterial.color = pressedColor;

                // Reproducir sonido
                if (buttonSound != null && m_audioSource != null)
                    m_audioSource.PlayOneShot(buttonSound);

                // Ejecutar efectos
                ExecuteButtonEffects();

                Debug.Log("Poke button activated!");
            }
        }

        private void OnButtonReleased(SelectExitEventArgs args)
        {
            if (IsPokeInteractor(args.interactorObject))
            {
                m_isPressed = false;

                // Restaurar color del botón
                if (m_buttonMaterial != null)
                    m_buttonMaterial.color = normalColor;

                Debug.Log("Poke button released");
            }
        }

        private bool IsPokeInteractor(IXRInteractor interactor)
        {
            // Verificar si es un XR Poke Interactor oficial
            return interactor is XRPokeInteractor;
        }

        private void ExecuteButtonEffects()
        {
            // Efecto 1: Toggle luces
            ToggleLights();

            // Efecto 2: Spawn enemigos
            SpawnEnemies();

            // Efecto 3: Particle effect
            if (activationEffect != null)
                activationEffect.Play();
        }

        private void ToggleLights()
        {
            m_lightsOn = !m_lightsOn;

            foreach (Light light in lightsToToggle)
            {
                if (light != null)
                {
                    light.enabled = m_lightsOn;
                }
            }

            Debug.Log($"Lights turned {(m_lightsOn ? "ON" : "OFF")}");
        }

        private void SpawnEnemies()
        {
            if (enemiesToSpawn.Length == 0 || spawnPoints.Length == 0)
                return;

            for (int i = 0; i < spawnPoints.Length && i < enemiesToSpawn.Length; i++)
            {
                if (spawnPoints[i] != null && enemiesToSpawn[i] != null)
                {
                    GameObject enemy = Instantiate(
                        enemiesToSpawn[i],
                        spawnPoints[i].position,
                        spawnPoints[i].rotation
                    );

                    Debug.Log($"Enemy spawned at {spawnPoints[i].name}");

                    // Opcional: destruir después de un tiempo
                    Destroy(enemy, 10f);
                }
            }
        }
    }
}