using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;


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

    private CanvasRenderer imageRenderer;
    private CanvasRenderer textRenderer;

    private void GetRenderers(GameObject sign)
    {
        imageRenderer = sign.GetComponent<CanvasRenderer>();
        textRenderer = sign.GetComponentInChildren<TextMeshProUGUI>().GetComponent<CanvasRenderer>();
        SetAlpha(0f);
    }

    private void PlayFadeSequence(GameObject sign)
    {
        StopAllCoroutines();
        GetRenderers(sign);
        StartCoroutine(FadeInOutRoutine(sign));
    }

    private IEnumerator FadeInOutRoutine(GameObject sign)
    {
        sign.SetActive(true);
        yield return Fade(0f, 1f);
        yield return new WaitForSeconds(visibleTime);
        yield return Fade(1f, 0f);
        sign.SetActive(false);
    }

    private IEnumerator Fade(float start, float end)
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);

            float alpha = Mathf.Lerp(start, end, t);
            imageRenderer.SetAlpha(alpha);
            textRenderer.SetAlpha(alpha);

            yield return null;
        }
        
        imageRenderer.SetAlpha(end);
        textRenderer.SetAlpha(end);
    }

    private void SetAlpha(float alpha)
    {
        imageRenderer.SetAlpha(alpha);
        textRenderer.SetAlpha(alpha);
    }

    public void HandleRoadSignFade()
    {
        PlayFadeSequence(roadSign);
    }

    public void HandlePedestrianPath()
    {
        PlayFadeSequence(pedestrianSign);
    }

    public void HandleRedCrossSignFade()
    {
        PlayFadeSequence(redCrossSign);
    }
    
    public void HandleLookSidesWarningSignFade()
    {
        PlayFadeSequence(lookSidesWarningSign);
    }
    
    public void HandleLookSidesSignFade()
    {
        PlayFadeSequence(lookSidesSign);
    }
}
