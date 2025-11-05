using UnityEngine;

public class RedLightAndStartCarOnTrigger : MonoBehaviour
{
    [Header("Referencias")]
    public TrafficLightController trafficLight;
    public CarMover car;

    private bool triggered = false;

    /// <summary>
    /// Llamable desde UnityEvent o directamente desde otro script.
    /// </summary>
    public void Activate()
    {
        if (triggered)
        {
            Debug.Log($"{name}: Ya fue activado una vez, ignorando.");
            return;
        }

        triggered = true;

        // 🚦 Cambiar semáforo a rojo
        if (trafficLight != null)
        {
            trafficLight.SetRedForA();
            Debug.Log($"{name}: Semáforo puesto en rojo (SetRedForA).");
        }
        else
        {
            Debug.LogWarning($"{name}: No se asignó ningún TrafficLightController.");
        }

        // 🚗 Iniciar auto que obedece semáforo
        if (car != null)
        {
            car.obeyTrafficLight = true;
            car.trafficLight = trafficLight;
            car.StartDriving();
            Debug.Log($"{name}: Auto '{car.name}' activado y configurado para obedecer semáforo.");
        }
        else
        {
            Debug.LogWarning($"{name}: No se asignó ningún CarMover.");
        }
    }

    /// <summary>
    /// Opcional: también funciona como trigger físico directo.
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"{name}: Jugador entró al trigger. Ejecutando Activate().");
            Activate();
        }
    }
}