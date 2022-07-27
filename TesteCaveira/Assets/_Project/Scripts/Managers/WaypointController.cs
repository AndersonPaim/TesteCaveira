using System.Collections.Generic;
using UnityEngine;

public class WaypointController : MonoBehaviour
{
    [SerializeField] private List<Transform> _archerWaypoints = new List<Transform>();
    public List<Transform> ArcherWaypoint => _archerWaypoints;
}
