using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : IAIState
{
    private PedestrianAIController controller;

    public WalkState(PedestrianAIController pedestrian)
    {
        controller = pedestrian;
    }

    public void EnterState()
    {
        controller.ChooseNextAction();
    }

    public void UpdateState()
    {
        if(controller.ReachedDestination())
        {
            if(controller.ShouldGetNewAction)
            {
                controller.ChooseNextAction();
            }
            else
            {
                controller.Disappear();
            }
        }
    }

    public void ExitState(){ }

    public void GetHit()
    {
        controller.TurnIntoZombie();
    }

    public void OnTriggerEnter()
    {
        controller.ChangeToState(controller.RunAwayState);
    }

    public void OnTriggerStay()
    {
        controller.ChangeToState(controller.RunAwayState);
    }

    public void OnTriggerExit() { }
}
