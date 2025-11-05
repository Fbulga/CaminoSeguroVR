using UnityEngine;

[RequireComponent(typeof(SimpleLights))]
public class TrafficLightController : MonoBehaviour
{
    private SimpleLights lights;

    void Awake()
    {
        lights = GetComponent<SimpleLights>();
    }

    /// <summary>
    /// Pone el eje A (autos) en rojo, eje B en verde.
    /// </summary>
    public void SetRedForA()
    {
        if (lights.isAActive) lights.ToggleLights(); // Solo toggle si está verde
    }

    /// <summary>
    /// Pone el eje A (autos) en verde, eje B en rojo.
    /// </summary>
    public void SetGreenForA()
    {
        if (!lights.isAActive) lights.ToggleLights(); // Solo toggle si está rojo
    }

    /// <summary>
    /// Devuelve true si el eje A (autos) está en rojo.
    /// </summary>
    public bool IsRedForA()
    {
        return !lights.isAActive;
    }
}