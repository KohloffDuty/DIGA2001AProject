using UnityEngine;
using UnityEngine.UI;

public class EndButtonAnimator : MonoBehaviour
{
    [SerializeField] private RectTransform button;
    [SerializeField] private Vector2 startOffset = new Vector2(0, -200f);
    [SerializeField] private float slideTime = 0.8f;

    private Vector2 targetPosition;

    void Start()
    {
        targetPosition = button.anchoredPosition;
        button.anchoredPosition += startOffset;
        StartCoroutine(SlideIn());
    }

    private System.Collections.IEnumerator SlideIn()
    {
        float t = 0f;
        while (t < slideTime)
        {
            t += Time.deltaTime;
            button.anchoredPosition = Vector2.Lerp(targetPosition + startOffset, targetPosition, t / slideTime);
            yield return null;
        }
    }
}