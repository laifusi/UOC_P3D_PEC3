using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowState : IZombieState
{
    private ZombieAIController controller;

    public FollowState(ZombieAIController zombie)
    {
        controller = zombie;
    }

    public void EnterState()
    {
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

    public void OnTriggerEnter(Collider col) { }

    public void OnTriggerStay(Collider col) { }

    public void OnTriggerExit(Collider col)
    {
        controller.ChangeToState(controller.WanderState);
    }
}
