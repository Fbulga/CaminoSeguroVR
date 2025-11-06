using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class GreenLightAndStartTrafficTrigger : MonoBehaviour
{
    [Header("Referencias")]
    public TrafficLightController trafficLight;
    public TrafficCar[] trafficCars;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered || !other.CompareTag("Player")) return;

        triggered = true;
        Debug.Log($"{name}: 🟢 Player triggered traffic flow.");
        StartCoroutine(ActivateSequence());
    }

    private IEnumerator ActivateSequence()
    {
        // Cambiar semáforo al eje B (A rojo)
        if (trafficLight != null)
        {
            trafficLight.SetRedForA();
            Debug.Log($"{name}: 🟢 Traffic light turned green for axis B (A is red)!");
        }

        // Espera mínima para actualización visual y lógica
        yield return new WaitForSeconds(0.2f);

        // Activar todos los autos
        foreach (var car in trafficCars)
        {
            if (car == null) continue;
            car.StartDriving();
        }
    }
}