using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManager : MonoBehaviour
{
    [SerializeField] private GameObject staticCar;
    [SerializeField] private GameObject AICar;
    [SerializeField] private GameObject playerControllerCar;

    private void Start()
    {
        StartStaticCar();
    }

    public void StartStaticCar()
    {
        staticCar.SetActive(true);
        AICar.SetActive(false);
        playerControllerCar.SetActive(false);
    }

    public void StartAICar()
    {
        staticCar.SetActive(false);
        AICar.SetActive(true);
        playerControllerCar.SetActive(false);
    }

    public void StartPlayerCar()
    {
        staticCar.SetActive(false);
        AICar.SetActive(false);
        playerControllerCar.SetActive(true);
    }
}