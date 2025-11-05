using UnityEngine;
using UnityEngine.Events;

public class TrafficLightProxy : MonoBehaviour
{
    public enum LightState { Green, Yellow, Red }

    [Header("Estado actual (solo lectura)")]
    [SerializeField] private LightState current = LightState.Green;
    public LightState Current => current;
    public bool IsRed => current == LightState.Red;

    [Header("Conecta aquí tu script real del semáforo")]
    public UnityEvent OnTurnRed;
    public UnityEvent OnTurnGreen;

    public void TurnRed()
    {
        current = LightState.Red;
        OnTurnRed?.Invoke(); // aquí conectás el método de tu semáforo (p.ej. Toggle(), SetRed(), etc.)
    }

    public void TurnGreen()
    {
        current = LightState.Green;
        OnTurnGreen?.Invoke(); // idem para poner en verde
    }
}