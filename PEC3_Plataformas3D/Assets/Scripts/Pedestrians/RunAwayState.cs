using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAwayState : IAIState
{
    private PedestrianAIController controller;

    public RunAwayState(PedestrianAIController pedestrian)
    {
        controller = pedestrian;
    }

    public void EnterState()
    {
        controller.SetSpeed();
        controller.RunToSafePoint();
    }

    public void UpdateState()
    {
        if (controller.ReachedDestination())
        {
            controller.Disappear();
        }
    }

    public void ExitState() { }

    public void GetHit() { }

    public void OnTriggerEnter()
    {
        controller.RunToSafePoint();
    }

    public void OnTriggerStay()
    {
    }

    public void OnTriggerExit()
    {
    }
}
