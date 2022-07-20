using System.Collections.Generic;
using UnityEngine;

public class WaypointController : MonoBehaviour
{
    public static WaypointController sInstance;

    [SerializeField] private List<Transform> _archerWaypoints = new List<Transform>();

    public List<Transform> ArcherWaypoint => _archerWaypoints;

    private void Awake()
    {
        if (sInstance != null)
        {
            Object.DestroyImmediate(gameObject);
            return;
        }
        else
        {
            sInstance = this;
        }
    }
}
