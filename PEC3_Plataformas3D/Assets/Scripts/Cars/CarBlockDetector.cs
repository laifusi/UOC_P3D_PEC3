using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBlockDetector : MonoBehaviour
{
    [SerializeField] GameObject car;
    [SerializeField] float maxTimeBlocking = 30;
    [SerializeField] bool isPlayerControlled;

    float timeBlocking;

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

    public void GetNotifiedOfBlockingEnd()
    {
        timeBlocking = 0;
    }
}
