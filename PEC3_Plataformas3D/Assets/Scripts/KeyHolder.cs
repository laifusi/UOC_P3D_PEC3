using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyHolder : MonoBehaviour
{
    private int keysCollected;

    public void PickUpKey()
    {
        keysCollected++;
    }

    public void UseKey()
    {
        keysCollected--;
    }

    public bool HasKey()
    {
        return keysCollected > 0;
    }
}
