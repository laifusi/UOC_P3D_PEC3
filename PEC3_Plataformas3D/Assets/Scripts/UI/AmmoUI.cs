using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoUI : MonoBehaviour
{
    private TMP_Text text;
    private void Start()
    {
        text = GetComponent<TMP_Text>();
        Gun.OnAmmoChange += UpdateText;
    }

    private void UpdateText(int value)
    {
        text.SetText(value.ToString());
    }

    private void OnDestroy()
    {
        Gun.OnAmmoChange -= UpdateText;
    }
}
