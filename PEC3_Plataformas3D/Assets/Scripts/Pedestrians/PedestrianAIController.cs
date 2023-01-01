using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PedestrianAIController : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform[] cityDestinations;
    [SerializeField] private Transform[] buildingDestinations;
    [SerializeField] private Transform[] carDestinations;
    [SerializeField] private GameObject zombifiedParticles;
    [SerializeField] private Transform[] safePoints;

    public WalkState WalkState { get; private set; }
    public RunAwayState RunAwayState { get; private set; }
    public bool ShouldGetNewAction => shouldGetNewAction;

    public static System.Action<GameObject, Transform> OnGotInCar;
    public static System.Action<Transform> OnZombified;

    private IAIState currentState;
    private List<Transform> emptyCars = new List<Transform>();
    private List<Transform> occupiedCars = new List<Transform>();
    private Transform chosenDestination;
    private NavMeshAgent agent;
    private bool shouldGetNewAction = true;
    private bool carAction;

    private void Start()
    {
        WalkState = new WalkState(this);
        RunAwayState = new RunAwayState(this);

        agent = GetComponent<NavMeshAgent>();

        foreach(Transform car in carDestinations)
        {
            emptyCars.Add(car);
        }

        ChangeToState(WalkState);
    }

    private void Update()
    {
        currentState?.UpdateState();
    }

    public void ChangeToState(IAIState state)
    {
        currentState?.ExitState();
        currentState = state;
        currentState?.EnterState();
    }

    public void SetDestination()
    {
        agent.isStopped = false;
        agent.SetDestination(chosenDestination.position);
    }

    public bool ReachedDestination()
    {
        return agent.remainingDistance <= agent.stoppingDistance;
    }

    public void ChooseNextAction()
    {
        int randomId;

        int actionPercentage = Random.Range(0, 100);

        if (actionPercentage < 20)
        {
            if(emptyCars.Count > 0)
            {
                randomId = Random.Range(0, emptyCars.Count);
                chosenDestination = emptyCars[randomId];
                emptyCars.Remove(chosenDestination);
                occupiedCars.Add(chosenDestination);
                carAction = true;
                shouldGetNewAction = false;
            }
        }
        else if(actionPercentage < 40)
        {
            randomId = Random.Range(0, buildingDestinations.Length);
            chosenDestination = buildingDestinations[randomId];
            shouldGetNewAction = false;
        }
        else
        {
            randomId = Random.Range(0, cityDestinations.Length);
            chosenDestination = cityDestinations[randomId];
            shouldGetNewAction = true;
        }

        SetDestination();
    }

    public void TurnIntoZombie()
    {
        OnZombified?.Invoke(transform);
        Instantiate(zombifiedParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void RunToSafePoint()
    {
        GetClosestSafePoint();
        SetDestination();
    }

    private void GetClosestSafePoint()
    {
        float distanceToClosestPoint = 0;

        foreach(Transform safePoint in safePoints)
        {
            agent.SetDestination(safePoint.position);
            float distanceToPoint = agent.remainingDistance;
            if(distanceToClosestPoint == 0 || distanceToPoint < distanceToClosestPoint)
            {
                distanceToClosestPoint = distanceToPoint;
                chosenDestination = safePoint;
            }
        }
    }

    public void Disappear()
    {
        if(carAction)
        {
            OnGotInCar?.Invoke(prefab, chosenDestination);
        }

        Destroy(gameObject);
    }

    public bool ShouldRecalculateSafePoint()
    {
        return false;
    }

    public void OnZombieTriggerEnter()
    {
        currentState?.OnTriggerEnter();
    }

    public void OnZombieTriggerStay()
    {
        currentState?.OnTriggerStay();
    }

    public void OnZombieTriggerExit()
    {
        currentState?.OnTriggerExit();
    }
}
