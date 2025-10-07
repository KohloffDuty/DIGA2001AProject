using UnityEngine;

[ExecuteAlways] // So it runs in edit mode
public class ZoneVisualizer : MonoBehaviour
{
    [Header("Zone Radii (in meters)")]
    public float innermostRadius = 10f;
    public float innerRadius = 25f;
    public float outerRadius = 45f;

    [Header("Colors")]
    public Color innermostColor = new Color(0f, 1f, 1f, 0.3f);  // Cyan (safe zone)
    public Color innerColor = new Color(1f, 1f, 0f, 0.3f);      // Yellow (food zone)
    public Color outerColor = new Color(1f, 0f, 0f, 0.3f);      // Red (danger zone)

    private void OnDrawGizmos()
    {
        Vector3 center = transform.position;

        // OUTER ZONE
        Gizmos.color = outerColor;
        Gizmos.DrawWireSphere(center, outerRadius);
        DrawLabel(center + Vector3.forward * outerRadius, "Outer Zone (Danger)");

        // INNER ZONE
        Gizmos.color = innerColor;
        Gizmos.DrawWireSphere(center, innerRadius);
        DrawLabel(center + Vector3.forward * innerRadius, "Inner Zone (Food)");

        // INNERMOST ZONE
        Gizmos.color = innermostColor;
        Gizmos.DrawWireSphere(center, innermostRadius);
        DrawLabel(center + Vector3.forward * innermostRadius, "Innermost Zone (Safe)");
    }

    private void DrawLabel(Vector3 position, string text)
    {
#if UNITY_EDITOR
        UnityEditor.Handles.color = Color.white;
        UnityEditor.Handles.Label(position + Vector3.up * 0.5f, text);
#endif
    }
}
