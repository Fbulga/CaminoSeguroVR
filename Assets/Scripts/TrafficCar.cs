using UnityEngine;

[ExecuteAlways]
public class TrafficCar : MonoBehaviour
{
    [Header("Movimiento")]
    public Transform target;
    [Range(1f, 20f)] public float maxSpeed = 8f;
    [Range(0.5f, 10f)] public float acceleration = 3f;
    [Range(2f, 20f)] public float brakeDecel = 10f;
    [SerializeField, Range(0.5f, 10f)] private float stopDistance = 2f;

    [Header("Semáforo y convoy")]
    public TrafficCar carInFront;
    public TrafficLightController trafficLight;
    public bool isLeader = false;
    public bool started = false;

    [Header("Debug (solo lectura)")]
    [SerializeField] private float currentSpeed = 0f;
    [SerializeField] private bool greenForB = false;
    [SerializeField] private bool canMove = false;
    [SerializeField] private bool ignoreTraffic = false;
    [SerializeField] private bool stoppedByLight = false;

    // 📉 Velocidad de cambio suave (para SmoothDamp)
    private float smoothVelocity = 0f;

    void Start() => stopDistance = Random.Range(6f, 8f);

    void FixedUpdate()
    {
        if (!started) return;

        // Si ignora el semáforo, sigue derecho
        if (ignoreTraffic)
        {
            MoveStraight();
            return;
        }

        // Si está detenido por luz roja → no se mueve
        if (stoppedByLight)
        {
            ApplyMovement(0);
            return;
        }

        // 🚦 Líder revisa el semáforo
        if (isLeader && trafficLight)
        {
            bool currentGreen = trafficLight.IsRedForA();
            if (currentGreen != greenForB)
            {
                greenForB = currentGreen;
                if (greenForB)
                {
                    canMove = true;
                    stoppedByLight = false;
                }
            }
        }
        // Seguidores heredan el estado del coche delante
        else if (carInFront)
        {
            greenForB = carInFront.greenForB;
        }

        // 🟥 Sin verde → desacelera suavemente
        if (!greenForB)
        {
            SmoothStop();
            return;
        }

        // 🚗 Si puede moverse, avanza recto
        if (canMove) MoveStraight();
    }

    private void MoveStraight()
    {
        if (target)
        {
            float dist = Vector3.Distance(transform.position, target.position);
            if (dist < 0.5f)
            {
                SmoothStop();
                return;
            }
        }

        if (carInFront)
        {
            float distFront = Vector3.Distance(transform.position, carInFront.transform.position);
            if (distFront <= stopDistance)
            {
                SmoothStop();
                return;
            }
        }

        // Aceleración progresiva suave
        currentSpeed = Mathf.SmoothDamp(currentSpeed, maxSpeed, ref smoothVelocity, 0.8f);
        ApplyMovement(currentSpeed);
    }

    // 🧭 Frenado con amortiguación suave
    private void SmoothStop()
    {
        // SmoothDamp suaviza la transición sin cortes secos
        currentSpeed = Mathf.SmoothDamp(currentSpeed, 0, ref smoothVelocity, 0.6f);
        if (currentSpeed < 0.02f)
            currentSpeed = 0;

        ApplyMovement(currentSpeed);
    }

    // 🅿️ Freno semáforo
    public void SoftStop()
    {
        if (ignoreTraffic) return;
        stoppedByLight = true;

        // Frenado aún más suave que el normal
        currentSpeed = Mathf.SmoothDamp(currentSpeed, 0, ref smoothVelocity, 1.2f);
        if (currentSpeed < 0.02f)
            currentSpeed = 0;

        ApplyMovement(currentSpeed);
        Debug.Log($"{name}: 🅿️ Soft stop (SmoothDamp)");
    }

    private void ApplyMovement(float speed)
    {
        transform.position += transform.forward * speed * Time.fixedDeltaTime;
    }

    // --- API pública ---
    public void StartDriving()
    {
        started = true;
        canMove = false;
        stoppedByLight = false;
        currentSpeed = 0f;
        greenForB = trafficLight && trafficLight.IsRedForA();
        if (greenForB) canMove = true;
    }

    public void ForceStop()
    {
        if (ignoreTraffic) return;
        stoppedByLight = true;
        canMove = false;
        currentSpeed = 0;
        smoothVelocity = 0;
        Debug.Log($"{name}: ⛔ Force stop");
    }

    public void ForceGo()
    {
        greenForB = true;
        canMove = true;
        stoppedByLight = false;
        smoothVelocity = 0;
        Debug.Log($"{name}: 🚦 Force go");
    }

    public void IgnoreTraffic()
    {
        ignoreTraffic = true;
        stoppedByLight = false;
        Debug.Log($"{name}: 🚘 Ignoring future red lights");
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector3 start = transform.position;
        Vector3 end = start + transform.forward * 3f;
        Gizmos.DrawLine(start, end);
        Gizmos.DrawSphere(end, 0.1f);
        UnityEditor.Handles.Label(end + Vector3.up * 0.5f, "→ Local Forward (Z)");
    }
#endif
}
