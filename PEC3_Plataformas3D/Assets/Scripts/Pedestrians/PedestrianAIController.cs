using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianAIController : MonoBehaviour
{
    [SerializeField] private Transform[] possibleDestinations;

    public WalkState WalkState { get; private set; }
    public RunAwayState RunAwayState { get; private set; }

    private IAIState currentState;

    private void Start()
    {
        WalkState = new WalkState(this);
        RunAwayState = new RunAwayState(this);
    }

    public void ChangeToState(IAIState state)
    {
        currentState?.ExitState();
        currentState = state;
        currentState?.EnterState();
    }

    public void SetDestination()
    {

    }

    public bool ReachedDestination()
    {
        return true;
    }

    public void ChooseNextAction()
    {

    }

    public void TurnIntoZombie()
    {

    }

    public void RunToSafePoint()
    {

    }

    public void Disappear()
    {

    }

    public bool ShouldRecalculateSafePoint()
    {
        return true;
    }
}
