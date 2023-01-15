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
        controller.SetSpeed();
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
            else if(controller.IsReachedPossible())
            {
                controller.Disappear();
            }
        }
    }

    public void ExitState(){ }

    public void GetHit()
    {
    }

    public void OnTriggerEnter()
    {
        controller.ReactToZombie();
        //controller.ChangeToState(controller.RunAwayState);
    }

    public void OnTriggerStay()
    {
        controller.ReactToZombie();
        //controller.ChangeToState(controller.RunAwayState);
    }

    public void OnTriggerExit() { }
}
