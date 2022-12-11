using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePlayerDetection : MonoBehaviour
{
    private ZombieAIController controller;

    private void Start()
    {
        controller = GetComponentInParent<ZombieAIController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        controller.TriggerEnter(other);
    }

    private void OnTriggerStay(Collider other)
    {
        controller.TriggerStay(other);
    }

    private void OnTriggerExit(Collider other)
    {
        controller.TriggerExit(other);
    }
}
