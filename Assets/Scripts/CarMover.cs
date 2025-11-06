using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarMover : MonoBehaviour
{
    [Header("Ruta")]
    public List<Transform> waypoints = new List<Transform>();
    public float speed = 8f;
    public float waypointReachThreshold = 0.5f;

    [Header("Detención por semáforo")]
    public bool obeyTrafficLight = false;
    public TrafficLightController trafficLight;
    public Transform stopLine;
    public float stopDistance = 2.0f;
    public float brakeDecel = 12f;

    private Rigidbody rb;
    private int currentIndex = 0;
    private bool driving = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    public void StartDriving()
    {
        if (waypoints.Count == 0)
        {
            Debug.LogError($"{name}: 🚫 No hay waypoints asignados. No puede conducir.");
            return;
        }

        // Reiniciar si el auto ya terminó su recorrido
        if (currentIndex >= waypoints.Count)
            currentIndex = 0;

        driving = true;
        Debug.Log($"{name}: 🚗 Comienza a conducir. Waypoints totales: {waypoints.Count}");
    }

    void FixedUpdate()
    {
        if (!driving) return;
        if (waypoints == null || waypoints.Count == 0)
        {
            Debug.LogWarning($"{name}: Sin waypoints, deteniendo el movimiento.");
            driving = false;
            return;
        }

        // Seguridad: evitar índices fuera de rango
        if (currentIndex < 0 || currentIndex >= waypoints.Count)
        {
            Debug.LogWarning($"{name}: Índice de waypoint fuera de rango ({currentIndex}). Deteniendo auto.");
            driving = false;
            rb.velocity = Vector3.zero;
            return;
        }

        // 🚦 Frenar si el semáforo está rojo
        if (obeyTrafficLight && trafficLight != null && trafficLight.IsRedForA() && stopLine != null)
        {
            float distToStop = Vector3.Distance(
                new Vector3(transform.position.x, 0, transform.position.z),
                new Vector3(stopLine.position.x, 0, stopLine.position.z)
            );

            if (distToStop <= stopDistance)
            {
                rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.fixedDeltaTime * brakeDecel);
                Debug.Log($"{name}: 🟥 Semáforo rojo. Deteniéndose a {distToStop:F2} m de la línea.");
                return;
            }
        }

        // 🛣️ Movimiento normal
        Transform target = waypoints[currentIndex];
        Vector3 to = target.position - transform.position;
        Vector3 dir = new Vector3(to.x, 0, to.z).normalized;

        if (dir.sqrMagnitude > 0.001f)
        {
            Quaternion look = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, look, Time.fixedDeltaTime * 6f);
        }

        rb.velocity = dir * speed;

        // ✅ Cambio de waypoint
        if (to.magnitude <= waypointReachThreshold)
        {
            Debug.Log($"{name}: Llegó al waypoint {currentIndex + 1}/{waypoints.Count}");
            currentIndex++;

            if (currentIndex >= waypoints.Count)
            {
                driving = false;
                rb.velocity = Vector3.zero;
                gameObject.SetActive(false);
                Debug.Log($"{name}: 🏁 Ruta completada, deteniéndose.");
            }
        }
    }
}
