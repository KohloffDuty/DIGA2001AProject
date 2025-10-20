using System.Collections;
using UnityEngine;

public class UIFader : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void FadeIn(float duration = 1.5f)
    {
        StopAllCoroutines();
        StartCoroutine(FadeRoutine(0f, 1f, duration));
    }

    public void FadeOut(float duration = 1.5f)
    {
        StopAllCoroutines();
        StartCoroutine(FadeRoutine(1f, 0f, duration));
    }

    private IEnumerator FadeRoutine(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        canvasGroup.alpha = startAlpha;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
    }

    private void Start()
    {
        // Optional: start hidden
        // canvasGroup.alpha = 0f;
    }

    private void Update()
    {
        // Nothing needed here for now
    }
}
