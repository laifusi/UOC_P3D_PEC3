using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Health : MonoBehaviour
{
    [SerializeField] float maxLife = 100;

    private float life;
    private Animator animator;

    public static Action<float> OnHealthChange;
    public static Action OnDeath;

    /// <summary>
    /// Start method to instantiate variables and listen for events
    /// </summary>
    private void Start()
    {
        life = maxLife;
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Method to get damaged
    /// If life is 0 or lower, we die
    /// </summary>
    /// <param name="damage">Amount of damage received</param>
    public void GetHurt(float damage)
    {
        animator.SetTrigger("GetHit");

        life -= damage;

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
    /// Die method: We fire the death event
    /// </summary>
    private void Die()
    {
        animator.SetBool("Dead", true);
        StartCoroutine(FixDeathAnimation());
        OnDeath?.Invoke();
    }

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
