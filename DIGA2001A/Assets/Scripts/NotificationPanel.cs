using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NotificationManager : MonoBehaviour
{
    [Header("References")]
    public TMP_Text notificationTemplate;   // Drag your disabled TMP_Text template here
    public float displayDuration = 2.5f;
    public float fadeDuration = 0.5f;
    public float moveUpDistance = 40f;

    private List<GameObject> activeNotifications = new List<GameObject>();

    public void ShowNotification(string message, Color? color = null)
    {
        // Create a new instance
        TMP_Text newNotif = Instantiate(notificationTemplate, transform);
        newNotif.gameObject.SetActive(true);
        newNotif.text = message;
        newNotif.color = color ?? Color.white;

        CanvasGroup cg = newNotif.GetComponent<CanvasGroup>();
        cg.alpha = 0f;

        activeNotifications.Add(newNotif.gameObject);

        StartCoroutine(AnimateNotification(newNotif, cg));
    }

    private IEnumerator AnimateNotification(TMP_Text notif, CanvasGroup cg)
    {
        // Fade in
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            cg.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }
        cg.alpha = 1;

        // Hold
        yield return new WaitForSeconds(displayDuration);

        // Fade out & move up
        RectTransform rt = notif.GetComponent<RectTransform>();
        Vector3 startPos = rt.localPosition;
        Vector3 endPos = startPos + Vector3.up * moveUpDistance;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            cg.alpha = Mathf.Lerp(1, 0, t / fadeDuration);
            rt.localPosition = Vector3.Lerp(startPos, endPos, t / fadeDuration);
            yield return null;
        }

        Destroy(notif.gameObject);
    }
}
