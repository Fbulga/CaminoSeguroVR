using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [Header("🚗 Prefabs y Configuración")]
    [Tooltip("Prefabs de autos con el componente CarMover.")]
    public List<GameObject> carPrefabs = new List<GameObject>();

    [Tooltip("Waypoints que seguirán los autos.")]
    public List<Transform> waypoints = new List<Transform>();

    [Tooltip("Control de semáforo (opcional).")]
    public TrafficLightController trafficLight;

    [Tooltip("Transform de línea de detención (opcional).")]
    public Transform stopLine;

    [Header("⚙️ Spawning")]
    [Tooltip("Tiempo entre apariciones (segundos).")]
    public float spawnInterval = 4f;

    [Tooltip("Máximo número de autos activos.")]
    public int maxCars = 10;

    [Tooltip("Desviación aleatoria al generar autos.")]
    public Vector3 randomOffset = new Vector3(1.5f, 0, 1.5f);

    [Tooltip("Si está activo, los autos se generan automáticamente.")]
    public bool autoSpawn = true;

    private readonly List<GameObject> activeCars = new List<GameObject>();
    private float nextSpawnTime = 0f;

    void Update()
    {
        if (!autoSpawn) return;

        // Esperar al próximo spawn
        if (Time.time < nextSpawnTime) return;

        // Limpiar referencias nulas
        activeCars.RemoveAll(c => c == null);

        // Límite de autos activos
        if (activeCars.Count >= maxCars) return;

        SpawnCar();
        nextSpawnTime = Time.time + spawnInterval;
    }

    public void SpawnCar()
    {
        if (carPrefabs.Count == 0)
        {
            Debug.LogWarning($"{name}: ⚠️ No hay prefabs asignados.");
            return;
        }
        if (waypoints.Count == 0)
        {
            Debug.LogWarning($"{name}: ⚠️ No hay waypoints asignados.");
            return;
        }

        // Prefab aleatorio
        GameObject prefab = carPrefabs[Random.Range(0, carPrefabs.Count)];
        Vector3 spawnPos = transform.position + new Vector3(
            Random.Range(-randomOffset.x, randomOffset.x),
            Random.Range(-randomOffset.y, randomOffset.y),
            Random.Range(-randomOffset.z, randomOffset.z)
        );

        Quaternion spawnRot = waypoints[0].rotation; // orientación inicial del primer waypoint
        GameObject newCar = Instantiate(prefab, spawnPos, spawnRot);

        CarMover mover = newCar.GetComponent<CarMover>();
        if (mover != null)
        {
            mover.waypoints = new List<Transform>(waypoints);
            mover.speed = Random.Range(20f, 40f);
            mover.StartDriving();

            if (trafficLight)
            {
                mover.obeyTrafficLight = true;
                mover.trafficLight = trafficLight;
                mover.stopLine = stopLine;
            }
        }
        else
        {
            Debug.LogWarning($"{name}: El prefab '{prefab.name}' no tiene componente CarMover.");
        }

        activeCars.Add(newCar);
        Debug.Log($"{name}: 🚘 Auto instanciado ({newCar.name}) en {spawnPos}");
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 0.6f);
        UnityEditor.Handles.Label(transform.position + Vector3.up * 1f, "🚗 Car Spawner");

        // Dibujar los waypoints conectados
        if (waypoints != null && waypoints.Count > 1)
        {
            Gizmos.color = Color.yellow;
            for (int i = 0; i < waypoints.Count - 1; i++)
            {
                if (waypoints[i] && waypoints[i + 1])
                    Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
            }
        }
    }
#endif
}
