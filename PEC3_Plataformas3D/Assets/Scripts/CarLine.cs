using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarLine : MonoBehaviour
{
    [SerializeField] private Transform entryPoint;
    [SerializeField] private UnityStandardAssets.Utility.WaypointCircuit waypointCircuit;

    public Vector3 getEntryPoint()
    {
        return entryPoint.position;
    }

    public Quaternion getEntryRotation()
    {
        return entryPoint.rotation;
    }

    public UnityStandardAssets.Utility.WaypointCircuit GetWayPoints()
    {
        return waypointCircuit;
    }
}
