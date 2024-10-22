using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Health : MonoBehaviour
{
    [SerializeField] float maxLife = 100;
    [SerializeField] private GameObject hitParticles;
    [SerializeField] private Transform head;
    [SerializeField] private AudioClip getHurtClip;

    private float life;
    private Animator animator;
    private AudioSource audioSource;

    public static Action<float> OnHealthChange;
    public static Action OnDeath;

    /// <summary>
    /// Start method to instantiate variables and listen for events
    /// </summary>
    private void Start()
    {
        life = maxLife;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Method to get damaged
    /// If life is 0 or lower, we die
    /// </summary>
    /// <param name="damage">Amount of damage received</param>
    public void GetHurt(float damage)
    {
        animator.SetTrigger("GetHit");

        Instantiate(hitParticles, head.position, Quaternion.identity);

        life -= damage;

        audioSource.PlayOneShot(getHurtClip);

        OnHealthChange?.Invoke(life);

        if (life <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Method to increase health
    /// </summary>
    /// <param name="healingValue">Amount of health increase</param>
    public void Heal(float healingValue)
    {
        life += healingValue;
        if (life > 100)
            life = 100;
        OnHealthChange?.Invoke(life);
    }

    /// <summary>
    /// Die method: We fire the death event and animation
    /// </summary>
    private void Die()
    {
        animator.SetBool("Dead", true);
        StartCoroutine(FixDeathAnimation());
        OnDeath?.Invoke();
    }

    /// <summary>
    /// We fix the animation's positioning
    /// </summary>
    IEnumerator FixDeathAnimation()
    {
        GetComponent<Collider>().enabled = false;
        GetComponent<ThirdPersonCharacter>().enabled = false;
        GetComponent<ThirdPersonUserControl>().enabled = false;
        GetComponent<Rigidbody>().useGravity = false;
        yield return new WaitForSeconds(2.5f);
        for (float i = 0; i >= -0.7f; i -= 0.01f)
        {
            yield return new WaitForSeconds(0.01f);
            transform.Translate(0, -0.01f, 0);
        }
    }
}
