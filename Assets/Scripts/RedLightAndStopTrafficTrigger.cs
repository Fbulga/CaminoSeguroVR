using UnityEngine;
using System.Collections.Generic;

public class RedLightAndStopTrafficTrigger : MonoBehaviour
{
    [Header("Referencias")]
    public TrafficLightController trafficLight;
    public Transform stopLine;
    [Tooltip("Distancia de tolerancia para decidir si el auto ya cruzó o no.")]
    public float stopTolerance = 1.5f;
    public TrafficCar[] trafficCars;

    private readonly Dictionary<TrafficCar, float> debugDotValues = new();
    private bool triggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (triggered || !other.CompareTag("Player")) return;

        triggered = true;
        Debug.Log($"{name}: 🛑 Player entered trigger — switching to red and evaluating cars.");
        ActivateSequence();
    }

    private void ActivateSequence()
    {
        // 🔴 Cambiar semáforo (A verde, B rojo)
        if (trafficLight)
        {
            trafficLight.SetGreenForA();
            Debug.Log($"{name}: 🔴 Traffic light set to red for axis B.");
        }

        if (!stopLine)
        {
            Debug.LogWarning($"{name}: ⚠️ No stopLine assigned — stopping all cars.");
            foreach (var car in trafficCars)
                car?.ForceStop();
            return;
        }

        Vector3 linePos = stopLine.position;
        Vector3 lineForward = stopLine.forward.normalized; // eje azul (Z)
        debugDotValues.Clear();

        foreach (var car in trafficCars)
        {
            if (!car) continue;

            Vector3 diff = car.transform.position - linePos;
            float dot = Vector3.Dot(diff, lineForward);
            debugDotValues[car] = dot;

            // 🧩 Si el auto está DETRÁS de la línea (dot < -tolerancia) → frena
            if (dot < -stopTolerance)
            {
                car.SoftStop();
                Debug.Log($"{car.name}: 🟥 Detenido (antes de la línea, dot={dot:F2})");
            }
            else
            {
                car.IgnoreTraffic();
                Debug.Log($"{car.name}: ✅ Ya cruzó (dot={dot:F2}), sigue normal.");
            }
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (!stopLine) return;

        Gizmos.color = Color.red;
        Vector3 p = stopLine.position;
        Vector3 f = stopLine.forward * 5f;
        Gizmos.DrawLine(p, p + f);
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.Label(p + Vector3.up * 1.5f, "🟥 Stop Line (forward →)");
        
        foreach (var kv in debugDotValues)
        {
            if (!kv.Key) continue;
            Gizmos.color = kv.Value < -stopTolerance ? Color.red : Color.green;
            Gizmos.DrawSphere(kv.Key.transform.position + Vector3.up * 2f, 0.3f);
        }
    }
#endif
}
