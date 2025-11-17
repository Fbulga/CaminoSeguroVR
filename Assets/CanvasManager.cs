using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [Header("Signs")]
    [SerializeField] private GameObject roadSign;
    [SerializeField] private GameObject pedestrianSign;
    [SerializeField] private GameObject redCrossSign;
    [SerializeField] private GameObject lookSidesWarningSign;
    [SerializeField] private GameObject lookSidesSign;

    [Header("Fade Control")]
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float visibleTime = 1.5f;

    // Cada cartel guarda su propia corrutina activa
    private Dictionary<GameObject, Coroutine> runningCoroutines = new Dictionary<GameObject, Coroutine>();

    private void PlayFadeSequence(GameObject sign)
    {
        // Si el cartel ya estaba fadeando, cancelar SOLO esa rutina
        if (runningCoroutines.ContainsKey(sign) && runningCoroutines[sign] != null)
            StopCoroutine(runningCoroutines[sign]);

        // Iniciar su propia fade routine
        runningCoroutines[sign] = StartCoroutine(FadeInOutRoutine(sign));
    }

    private IEnumerator FadeInOutRoutine(GameObject sign)
    {
        var img = sign.GetComponent<CanvasRenderer>();
        var txt = sign.GetComponentInChildren<TextMeshProUGUI>().GetComponent<CanvasRenderer>();

        // Arranca invisible
        img.SetAlpha(0f);
        txt.SetAlpha(0f);
        sign.SetActive(true);

        // Fade In
        yield return Fade(img, txt, 0f, 1f);

        // Tiempo visible
        yield return new WaitForSeconds(visibleTime);

        // Fade Out
        yield return Fade(img, txt, 1f, 0f);

        sign.SetActive(false);
    }

    private IEnumerator Fade(CanvasRenderer img, CanvasRenderer txt, float start, float end)
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);
            float alpha = Mathf.Lerp(start, end, t);

            img.SetAlpha(alpha);
            txt.SetAlpha(alpha);

            yield return null;
        }

        img.SetAlpha(end);
        txt.SetAlpha(end);
    }

    public void HandleRoadSignFade() => PlayFadeSequence(roadSign);
    public void HandlePedestrianPath() => PlayFadeSequence(pedestrianSign);
    public void HandleRedCrossSignFade() => PlayFadeSequence(redCrossSign);
    public void HandleLookSidesWarningSignFade() => PlayFadeSequence(lookSidesWarningSign);
    public void HandleLookSidesSignFade() => PlayFadeSequence(lookSidesSign);
}
