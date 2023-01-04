using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject winGameCanvas;
    [SerializeField] private GameObject endGameCanvas;
    [SerializeField] private UnityStandardAssets.Cameras.AutoCam cam;
    [SerializeField] private GameObject player;

    private void Start()
    {
        endGameCanvas.SetActive(false);

        Health.OnDeath += EndGame;
        ZombieSpawner.OnNoActivePoints += WinGame;
        Car.OnOutOfCar += ReactivatePlayer;
    }

    private void WinGame()
    {
        cam.DisableMovement(false);
        winGameCanvas.SetActive(true);
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

    private void ReactivatePlayer()
    {
        player.SetActive(true);
        cam.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        Health.OnDeath -= EndGame;
        ZombieSpawner.OnNoActivePoints -= WinGame;
        Car.OnOutOfCar -= ReactivatePlayer;
    }
}
