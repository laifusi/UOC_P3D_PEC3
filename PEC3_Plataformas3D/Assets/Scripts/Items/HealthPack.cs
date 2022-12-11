using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : Item
{
    [SerializeField] private float healingValue;

    protected override void PickUp(GameObject character)
    {
        character.GetComponent<Health>().Heal(healingValue);
    }
}
