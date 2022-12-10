using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float damage = 30;
    [SerializeField] private float force = 100;

    private void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.right * force);
    }

    private void OnTriggerEnter(Collider other)
    {
        ZombieAIController zombie = other.GetComponent<ZombieAIController>();
        if(zombie != null)
        {
            zombie.GetHurt(damage);
        }
    }
}
