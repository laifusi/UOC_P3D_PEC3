using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingPointUI : MonoBehaviour
{
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private Camera cam;

    private void Update()
    {
        transform.position = Input.mousePosition;
    }
}
