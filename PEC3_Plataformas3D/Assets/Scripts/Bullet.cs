using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float damage = 30;
    [SerializeField] private float force = 100;
    [SerializeField] private GameObject bloodParticles;

    private Vector3 instantiationPoint;

    private void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.up * force);
        instantiationPoint = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        ZombieAIController zombie = other.GetComponent<ZombieAIController>();
        if(zombie != null)
        {
            zombie.GetHurt(damage, instantiationPoint);
            Instantiate(bloodParticles, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
