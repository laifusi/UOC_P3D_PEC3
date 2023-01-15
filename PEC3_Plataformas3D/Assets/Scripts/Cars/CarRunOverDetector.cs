using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarRunOverDetector : MonoBehaviour
{
    [SerializeField] float runOverDamage = 10;
    private void OnTriggerEnter(Collider other)
    {
        ZombieAIController zombie = other.GetComponent<ZombieAIController>();
        PedestrianAIController pedestrian = other.GetComponent<PedestrianAIController>();
        Health player = other.GetComponent<Health>();

        if (zombie != null)
        {
            zombie.GetRunOver();
            GetComponentInParent<UnityStandardAssets.Vehicles.Car.CarAIControl>().SetDrivingBool(true);
        }
        else if (pedestrian != null)
        {
            pedestrian.GetRunOver();
            GetComponentInParent<UnityStandardAssets.Vehicles.Car.CarAIControl>().SetDrivingBool(true);
        }
        else if (player != null)
        {
            player.GetHurt(runOverDamage);
        }
    }
}
