using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    private RectTransform rectTransform;
    private float maxWidth;

    /// <summary>
    /// Initialize variables and listen to the listener
    /// </summary>
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        maxWidth = rectTransform.sizeDelta.x;

        Health.OnHealthChange += UpdateIndicator;
    }

    /// <summary>
    /// Update the health indicator: change the sizeDelta of the rectTransform according to the current value and the maxWidth
    /// </summary>
    /// <param name="currentNumber"></param>
    private void UpdateIndicator(float currentNumber)
    {
        rectTransform.sizeDelta = new Vector2(currentNumber * maxWidth / 100, rectTransform.sizeDelta.y);
    }

    /// <summary>
    /// Stop listening to events when destroyed
    /// </summary>
    private void OnDestroy()
    {
        Health.OnHealthChange -= UpdateIndicator;
    }
}
