using UnityEngine;

public class StartCarOnTrigger : MonoBehaviour
{
    [Header("Auto a activar")]
    public CarMover car;

    private bool triggered = false;

    // 🔹 Llamable desde un UnityEvent o directamente desde otro script
    public void Activate()
    {
        if (triggered)
        {
            Debug.Log($"{name} ya fue activado una vez. Ignorando.");
            return;
        }

        triggered = true;

        if (car != null)
        {
            Debug.Log($"{name}: Activando auto '{car.name}'");
            car.StartDriving();
        }
        else
        {
            Debug.LogWarning($"{name}: No se asignó ningún CarMover.");
        }
    }

    // 🔹 (opcional) si querés que también funcione como trigger directo
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"{name}: El jugador entró al trigger (OnTriggerEnter).");
            Activate();
        }
    }
}