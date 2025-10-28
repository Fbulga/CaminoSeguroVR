using UnityEngine;
using NaughtyAttributes;

public class SimpleLights : MonoBehaviour
{
    [System.Serializable]
    public class TrafficPair
    {
        [Header("Autos")]
        public Renderer autoRed;
        public Renderer autoGreen;

        [Header("Peatones (perpendicular)")]
        public Renderer pedRed;
        public Renderer pedGreen;
    }

    [Header("Eje A (autos) y peatonal perpendicular (B)")]
    public TrafficPair axisA;

    [Header("Eje B (autos) y peatonal perpendicular (A)")]
    public TrafficPair axisB;

    [Header("Colores principales")]
    public Color greenColor = new Color(0f, 1f, 0f); // Verde brillante
    public Color redColor = new Color(1f, 0f, 0f);   // Rojo brillante

    [Header("Intensidades de emisión")]
    [Range(0f, 5f)] public float onEmissionIntensity = 2.5f;
    [Range(0f, 2f)] public float offEmissionIntensity = 0.15f;

    [ReadOnly] public bool isAActive = true;

    void Start()
    {
        EnableEmission(axisA);
        EnableEmission(axisB);
        UpdateLights();
    }

    [Button("🔁 Toggle Luces")]
    public void ToggleLights()
    {
        isAActive = !isAActive;
        UpdateLights();
    }

    private void UpdateLights()
    {
        if (isAActive)
        {
            // A en verde, B en rojo
            SetPairState(axisA, true);
            SetPairState(axisB, false);
        }
        else
        {
            // B en verde, A en rojo
            SetPairState(axisA, false);
            SetPairState(axisB, true);
        }
    }

    private void SetPairState(TrafficPair pair, bool active)
    {
        // Autos
        SetEmissive(pair.autoGreen, active, greenColor);
        SetEmissive(pair.autoRed, !active, redColor);

        // Peatones (perpendicular)
        SetEmissive(pair.pedGreen, !active, greenColor);
        SetEmissive(pair.pedRed, active, redColor);
    }

    private void SetEmissive(Renderer rend, bool active, Color color)
    {
        if (rend == null) return;

        var mat = rend.material;
        float intensity = active ? onEmissionIntensity : offEmissionIntensity;

        mat.EnableKeyword("_EMISSION");
        mat.SetColor("_BaseColor", color);
        mat.SetColor("_EmissionColor", color * intensity);

        DynamicGI.SetEmissive(rend, color * intensity);
    }

    private void EnableEmission(TrafficPair pair)
    {
        EnableEmissionFor(pair.autoRed);
        EnableEmissionFor(pair.autoGreen);
        EnableEmissionFor(pair.pedRed);
        EnableEmissionFor(pair.pedGreen);
    }

    private void EnableEmissionFor(Renderer rend)
    {
        if (rend == null) return;
        rend.material.EnableKeyword("_EMISSION");
    }
}