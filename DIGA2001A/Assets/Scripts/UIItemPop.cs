using UnityEngine;

public class UIItemPop : MonoBehaviour
{
    [Header("Pop Settings")]
    public float popScale = 1.3f;        // how big it grows
    public float popDuration = 0.15f;    // how fast it pops
    public float returnDuration = 0.2f;  // how fast it shrinks back

    private Vector3 originalScale;
    private bool isPopping = false;

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void PlayPop()
    {
        if (!isPopping)
            StartCoroutine(PopRoutine());
    }

    private System.Collections.IEnumerator PopRoutine()
    {
        isPopping = true;

        // Scale up
        float t = 0;
        while (t < popDuration)
        {
            t += Time.deltaTime;
            float progress = t / popDuration;
            transform.localScale = Vector3.Lerp(originalScale, originalScale * popScale, progress);
            yield return null;
        }

        // Scale down
        t = 0;
        while (t < returnDuration)
        {
            t += Time.deltaTime;
            float progress = t / returnDuration;
            transform.localScale = Vector3.Lerp(originalScale * popScale, originalScale, progress);
            yield return null;
        }

        transform.localScale = originalScale;
        isPopping = false;
    }
}
