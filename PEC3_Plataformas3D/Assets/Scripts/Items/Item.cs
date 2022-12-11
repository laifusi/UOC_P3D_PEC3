using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    private AudioSource audioSource;
    private Collider myCollider;

    protected void Start()
    {
        audioSource = GetComponent<AudioSource>();
        myCollider = GetComponent<Collider>();
    }

    /// <summary>
    /// If the player picks an item: play the sound, do the actions defined in the child classes and destroy the object
    /// </summary>
    /// <param name="other"></param>
    protected void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            audioSource.Play();
            PickUp(other.gameObject);
            myCollider.enabled = false;
            Destroy(gameObject, 1f);
        }
    }

    protected abstract void PickUp(GameObject character);
}
