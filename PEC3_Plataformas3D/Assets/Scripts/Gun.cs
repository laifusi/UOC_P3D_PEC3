using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private int initialAmountOfBullets = 10;
    [SerializeField] private int maxBullets = 10;
    [SerializeField] private Camera cam;
    [SerializeField] private Transform characterTransform;
    [SerializeField] private Transform animationIKTarget;
    [SerializeField] private LayerMask aimingLayerMask;
    [SerializeField] private float minAimDistance = 2f;

    private int amountOfMunition;
    private bool activeGun = true;

    public static Action<int> OnAmmoChange;

    /// <summary>
    /// Start Method to initialize variables and listen for events
    /// </summary>
    private void Start()
    {
        Health.OnDeath += DeactivateGun;
        ZombieSpawner.OnNoActivePoints += DeactivateGun;
        Ammo.OnPickAmmo += AddAmmo;

        amountOfMunition = initialAmountOfBullets;
        OnAmmoChange?.Invoke(amountOfMunition);
    }

    /// <summary>
    /// Update method
    /// We send a raycast from the mouse position and orient the IK target to make the body look at the point we are aiming at
    /// If the player clicks the mouse button, we instantiate a bullet
    /// </summary>
    private void Update()
    {
        if(activeGun)
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit, 100, aimingLayerMask))
            {
                Vector3 hitPoint = hit.point;
                if(Vector3.Distance(hit.point, shootingPoint.position) > minAimDistance)
                {
                    shootingPoint.right = (hit.point - shootingPoint.position).normalized;
                    animationIKTarget.position = hit.point;
                    animationIKTarget.forward = Vector3.Lerp(animationIKTarget.forward, (hit.point - shootingPoint.position).normalized, Time.deltaTime * 20f);
                }
            }

            if (amountOfMunition > 0 && Input.GetMouseButtonDown(0))
            {
                amountOfMunition--;
                OnAmmoChange?.Invoke(amountOfMunition);

                GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);
                Destroy(bullet, 2);
            }
        }
    }

    /// <summary>
    /// Method to add ammo to out count
    /// </summary>
    /// <param name="amount">Amount of bullets to add</param>
    public void AddAmmo(int amount)
    {
        amountOfMunition += amount;
        if (amountOfMunition > maxBullets)
            amountOfMunition = maxBullets;
        OnAmmoChange?.Invoke(amountOfMunition);
    }

    /// <summary>
    /// Method to activate or deactivate the gun
    /// </summary>
    /// <param name="isActive"></param>
    public void ActivateGun(bool isActive)
    {
        activeGun = isActive;
    }

    /// <summary>
    /// Method to deactivate the gun on game end
    /// </summary>
    private void DeactivateGun()
    {
        ActivateGun(false);
    }

    /// <summary>
    /// OnDestroy method to stop listening to events
    /// </summary>
    private void OnDestroy()
    {
        Health.OnDeath -= DeactivateGun;
        ZombieSpawner.OnNoActivePoints -= DeactivateGun;
    }
}
