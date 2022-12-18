using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieDoor : MonoBehaviour
{
    [SerializeField] private GameObject destroyDoorParticles;

    public static Action<Transform> OnDoorDestroyed;

    private void OnTriggerEnter(Collider other)
    {
        KeyHolder player = other.GetComponent<KeyHolder>();
        if(player != null && player.HasKey())
        {
            player.UseKey();
            DestroyDoor();
        }
    }

    private void DestroyDoor()
    {
        OnDoorDestroyed?.Invoke(transform);
        Instantiate(destroyDoorParticles, transform.position, Quaternion.identity);
        Destroy(gameObject, 0.5f);
    }
}
