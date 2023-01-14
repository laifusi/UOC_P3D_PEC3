using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManager : MonoBehaviour
{
    [SerializeField] private GameObject staticCar;
    [SerializeField] private GameObject AICar;
    [SerializeField] private GameObject playerControllerCar;

    private int carType; // 0 = Static ; 1 = AI ; 2 = Player

    private void Awake()
    {
        StartStaticCar();
    }

    private void Start()
    {
        PedestrianAIController.OnGotInCar += StartCar;
    }

    private void StartCar(GameObject pedestrian, CarManager car)
    {
        if(car == this)
        {
            StartAICar();
        }
    }

    public void StartStaticCar()
    {
        ResetTransforms();
        carType = 0;
        staticCar.SetActive(true);
        AICar.SetActive(false);
        playerControllerCar.SetActive(false);
    }

    public void StartAICar()
    {
        ResetTransforms();
        carType = 1;
        staticCar.SetActive(false);
        AICar.SetActive(true);
        playerControllerCar.SetActive(false);
    }

    public void StartPlayerCar()
    {
        ResetTransforms();
        carType = 2;
        staticCar.SetActive(false);
        AICar.SetActive(false);
        playerControllerCar.SetActive(true);
    }

    private void ResetTransforms()
    {
        if (carType == 0)
        {
            AICar.transform.position = staticCar.transform.position;
            AICar.transform.rotation = staticCar.transform.rotation;
            playerControllerCar.transform.position = staticCar.transform.position;
            playerControllerCar.transform.rotation = staticCar.transform.rotation;
        }
        else if (carType == 1)
        {
            staticCar.transform.position = AICar.transform.position;
            staticCar.transform.rotation = AICar.transform.rotation;
            playerControllerCar.transform.position = AICar.transform.position;
            playerControllerCar.transform.rotation = AICar.transform.rotation;
        }
        else if (carType == 2)
        {
            staticCar.transform.position = playerControllerCar.transform.position;
            staticCar.transform.rotation = playerControllerCar.transform.rotation;
            AICar.transform.position = playerControllerCar.transform.position;
            AICar.transform.rotation = playerControllerCar.transform.rotation;
        }
    }

    public void SetWayPoints(UnityStandardAssets.Utility.WaypointCircuit circuit)
    {
        AICar.GetComponent<UnityStandardAssets.Utility.WaypointProgressTracker>().SetWaypointCircuit(circuit);
    }

    public bool IsAICar()
    {
        return carType == 1;
    }

    public void UnspawnCar()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        PedestrianAIController.OnGotInCar -= StartCar;
    }
}
