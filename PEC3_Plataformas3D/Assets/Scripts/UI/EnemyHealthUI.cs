using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthUI : MonoBehaviour
{
    [SerializeField] ZombieAIController controller;

    private RectTransform rectTransform;
    private float maxWidth;

    private void Start()
    {
        controller.OnLifeChange += UpdateIndicator;

        rectTransform = GetComponent<RectTransform>();
        maxWidth = rectTransform.sizeDelta.x;
    }

    private void UpdateIndicator(float life)
    {
        rectTransform.sizeDelta = new Vector2(life * maxWidth / 100, rectTransform.sizeDelta.y);
    }

    private void OnDestroy()
    {
        controller.OnLifeChange -= UpdateIndicator;
    }
}
