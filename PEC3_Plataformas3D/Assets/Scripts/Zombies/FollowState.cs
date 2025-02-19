using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowState : IAIState
{
    private ZombieAIController controller;

    public FollowState(ZombieAIController zombie)
    {
        controller = zombie;
    }

    public void EnterState()
    {
        controller.StartRun();
        controller.SetFollowDestination();
    }

    public void UpdateState()
    {
        if(controller.ReachedDestination(false))
        {
            controller.ChangeToState(controller.AttackState);
        }
        else if(controller.ShouldRecalculateDestination())
        {
            controller.SetFollowDestination();
        }
    }

    public void ExitState() { }

    public void GetHit() { }

    public void OnTriggerEnter() { }

    public void OnTriggerStay() { }

    public void OnTriggerExit()
    {
        controller.ChangeToState(controller.WanderState);
    }
}
