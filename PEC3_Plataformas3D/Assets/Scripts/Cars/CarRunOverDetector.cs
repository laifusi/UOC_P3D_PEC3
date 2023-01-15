using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarRunOverDetector : MonoBehaviour
{
    [SerializeField] float runOverDamage = 10;

    /// <summary>
    /// We check if we hit a zombie, a pedestrian or the player
    /// If we it a zombie or a pedestrian, we run them over and reset the driving
    /// If we hit the player, we apply damage to him
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        ZombieAIController zombie = other.GetComponent<ZombieAIController>();
        PedestrianAIController pedestrian = other.GetComponent<PedestrianAIController>();
        Health player = other.GetComponent<Health>();

        if (zombie != null)
        {
            zombie.GetRunOver();
            GetComponentInParent<UnityStandardAssets.Vehicles.Car.CarAIControl>()?.SetDrivingBool(true);
        }
        else if (pedestrian != null)
        {
            pedestrian.GetRunOver();
            GetComponentInParent<UnityStandardAssets.Vehicles.Car.CarAIControl>()?.SetDrivingBool(true);
        }
        else if (player != null)
        {
            player.GetHurt(runOverDamage);
        }
    }
}
