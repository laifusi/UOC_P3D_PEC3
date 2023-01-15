using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBlockDetector : MonoBehaviour
{
    [SerializeField] GameObject car;
    [SerializeField] float maxTimeBlocking = 30;
    [SerializeField] bool isPlayerControlled;

    float timeBlocking;

    /// <summary>
    /// Method to know the car is blocking a road and check if it should be destroyed
    /// </summary>
    public void GetNotifiedOfRoadBlocking()
    {
        if (isPlayerControlled)
            return;

        timeBlocking += Time.deltaTime;

        if(timeBlocking > maxTimeBlocking)
        {
            Destroy(car);
        }
    }

    /// <summary>
    /// Method to reset the blocking timer
    /// </summary>
    public void GetNotifiedOfBlockingEnd()
    {
        timeBlocking = 0;
    }
}
