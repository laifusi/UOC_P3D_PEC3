using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : Item
{
    [SerializeField] private int amountOfBullets;

    public static Action<int> OnPickAmmo;

    protected override void PickUp(GameObject character)
    {
        OnPickAmmo?.Invoke(amountOfBullets);
    }
}
