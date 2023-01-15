using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarLine : MonoBehaviour
{
    [SerializeField] private Transform entryPoint; // Tranform of the point where the line starts
    [SerializeField] private UnityStandardAssets.Utility.WaypointCircuit waypointCircuit; // WaypointCircuit associated to the CarLine

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
