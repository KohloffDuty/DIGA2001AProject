using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class UIFader : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    void Awake() => canvasGroup = GetComponent<CanvasGroup>();

    public void FadeIn(float duration = 1f)
    {
        StopAllCoroutines();
        StartCoroutine(FadeRoutine(0f, 1f, duration));
    }

    IEnumerator FadeRoutine(float start, float end, float duration)
    {
        float t = 0;
        canvasGroup.alpha = start;
        while (t < duration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, t / duration);
            yield return null;
        }
        canvasGroup.alpha = end;
    }
}
