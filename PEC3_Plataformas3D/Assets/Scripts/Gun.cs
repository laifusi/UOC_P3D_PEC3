using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private int initialAmountOfBullets = 10;

    private int amountOfMunition;

    //public static Action<MunitionType, int> OnAmmoChange;

    //public MunitionType MunitionType => typeOfGun;

    /// <summary>
    /// Start Method to initialize variables and listen for events
    /// </summary>
    private void Start()
    {
        //audioSource = GetComponent<AudioSource>();

        amountOfMunition = initialAmountOfBullets;
        //OnAmmoChange?.Invoke(typeOfGun, amountOfMunition);
    }

    /// <summary>
    /// Update method
    /// We send a ray out from the center, if it hits something, we paint the aimingIndicator and check if the ammo amount, the time is right and the player pressed the button.
    /// If the player can and does shoot, we check if it hit an enemy and hurt him.
    /// If it didn't hit an enemy, we instantiate a bullet hole where we shot.
    /// </summary>
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, transform.rotation);
            Destroy(bullet, 2);
        }
    }

    /// <summary>
    /// Method to add ammo to out count
    /// </summary>
    /// <param name="typeOfAmmo">Type of munition to add</param>
    /// <param name="amount">Amount of bullets to add</param>
    /*public void AddAmmo(MunitionType typeOfAmmo, int amount)
    {
        if (typeOfAmmo != typeOfGun)
            return;

        amountOfMunition += amount;
        if (amountOfMunition > maxNumberOfBullets)
            amountOfMunition = maxNumberOfBullets;
        OnAmmoChange?.Invoke(typeOfGun, amountOfMunition);
    }*/

    /// <summary>
    /// Method to activate or deactivate the gun
    /// </summary>
    /// <param name="isActive"></param>
    /*public void ActivateGun(bool isActive)
    {
        activeGun = isActive;
    }*/
}
