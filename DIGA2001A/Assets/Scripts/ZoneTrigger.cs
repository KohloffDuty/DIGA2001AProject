using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ZoneTrigger : MonoBehaviour
{
    public enum ZoneType { Outer, Inner, Innermost }
    public ZoneType zoneType;

    [Header("Reference to Zone Visualizer")]
    public ZoneVisualizer visualizer;

    private SphereCollider col;

    private void Reset()
    {
        col = GetComponent<SphereCollider>();
        col.isTrigger = true;
    }

    private void OnValidate()
    {
        if (visualizer == null) return;

        col = GetComponent<SphereCollider>();
        col.isTrigger = true;

        switch (zoneType)
        {
            case ZoneType.Outer:
                col.radius = visualizer.outerRadius;
                break;
            case ZoneType.Inner:
                col.radius = visualizer.innerRadius;
                break;
            case ZoneType.Innermost:
                col.radius = visualizer.innermostRadius;
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            Debug.Log("Player entered " + zoneType + " zone.");
    }
}
