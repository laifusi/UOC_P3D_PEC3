using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Item
{
    protected override void PickUp(GameObject character)
    {
        character.GetComponent<KeyHolder>().PickUpKey();
    }
}
