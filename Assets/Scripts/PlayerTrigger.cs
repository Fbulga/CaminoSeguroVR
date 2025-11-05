using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class PlayerTrigger : MonoBehaviour
{
    [Tooltip("Tag que debe tener el jugador para disparar el trigger")]
    public string playerTag = "Player";

    [Header("Acciones al entrar el jugador")]
    public UnityEvent OnPlayerEnter;

    Collider col;

    void Reset()
    {
        col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            Debug.Log($"{name}: Jugador con tag '{playerTag}' detectado. Ejecutando UnityEvent.");
            OnPlayerEnter?.Invoke();
        }
        else
        {
            Debug.Log($"{name}: Otro objeto entró ({other.name}), tag = '{other.tag}', ignorado.");
        }
    }
}