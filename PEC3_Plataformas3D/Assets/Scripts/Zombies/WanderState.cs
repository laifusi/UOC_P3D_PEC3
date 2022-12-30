using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderState : IAIState
{
    private ZombieAIController controller;

    public WanderState(ZombieAIController zombie)
    {
        controller = zombie;
    }

    public void EnterState()
    {
        controller.StartMove();
        controller.RandomizeDestination();
    }

    public void UpdateState()
    {
        if(controller.ReachedDestination(true))
        {
            controller.RandomizeDestination();
        }
    }

    public void ExitState(){ }

    public void GetHit()
    {
        controller.ChangeToState(controller.FollowState);
    }

    public void OnTriggerEnter()
    {
        controller.ChangeToState(controller.FollowState);
    }

    public void OnTriggerStay()
    {
        controller.ChangeToState(controller.FollowState);
    }

    public void OnTriggerExit(){ }
}
