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
        controller.RunToSafePoint();
    }

    public void UpdateState() { }

    public void ExitState() { }

    public void GetHit() { }

    public void OnTriggerEnter() { }

    public void OnTriggerStay()
    {
        if(controller.ReachedDestination())
        {
            controller.Disappear();
        }
        else if (controller.ShouldRecalculateSafePoint())
        {
            controller.RunToSafePoint();
        }
    }

    public void OnTriggerExit()
    {
        controller.ChangeToState(controller.WalkState);
    }
}
