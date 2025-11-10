using UnityEngine;
using TMPro;
using System.Collections;

public class UITooltip : MonoBehaviour
{
    private TMP_Text tooltipText;
    private CanvasGroup canvasGroup;
    private Coroutine currentRoutine;

    [Header("Tooltip Settings")]
    public float fadeDuration = 0.5f;
    public float displayTime = 2f;

    void Awake()
    {
        tooltipText = GetComponent<TMP_Text>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
    }

    public void ShowMessage(string message, Color? color = null)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        tooltipText.text = message;
        tooltipText.color = color ?? Color.white;
        currentRoutine = StartCoroutine(FadeMessage());
    }

    private IEnumerator FadeMessage()
    {
        // Fade In
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1;

        yield return new WaitForSeconds(displayTime);

        // Fade Out
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            canvasGroup.alpha = Mathf.Lerp(1, 0, t / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0;
    }
}
