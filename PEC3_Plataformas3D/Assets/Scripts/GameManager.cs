using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject endGameCanvas;
    [SerializeField] private UnityStandardAssets.Cameras.AutoCam cam;

    private void Start()
    {
        endGameCanvas.SetActive(false);

        Health.OnDeath += EndGame;
    }

    private void EndGame()
    {
        StartCoroutine(WaitForAnimation());
    }

    IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(3);

        cam.DisableMovement(false);
        endGameCanvas.SetActive(true);
    }

    private void OnDestroy()
    {
        Health.OnDeath -= EndGame;
    }
}
