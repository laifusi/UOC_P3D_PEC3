using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PedestrianAIController : MonoBehaviour
{
    //[SerializeField] private GameObject prefab;
    [SerializeField] private GameObject zombifiedParticles;
    [SerializeField] private float minWalkSpeed = 0.5f;
    [SerializeField] private float maxWalkSpeed = 1.5f;
    [SerializeField] private float minRunSpeed = 1.5f;
    [SerializeField] private float maxRunSpeed = 3f;
    [SerializeField] private float minReactionTime = 0.2f;
    [SerializeField] private float maxReactionTime = 3f;
    [SerializeField] private GameObject runOverParticles;
    [SerializeField] private AudioClip[] possibleRunOverClips;
    [SerializeField] private AudioClip[] possibleScaredClips;

    public WalkState WalkState { get; private set; }
    public RunAwayState RunAwayState { get; private set; }
    public bool ShouldGetNewAction => shouldGetNewAction;

    public static System.Action<Transform> OnZombified;

    private IAIState currentState;
    private Transform chosenDestination;
    private NavMeshAgent agent;
    private bool shouldGetNewAction = true;
    private Animator animator;
    private AudioSource audioSource;
    private float timeSinceLastDestinationSet;

    private Transform[] cityDestinations;
    private Transform[] buildingDestinations;
    private Transform[] safePoints;

    private void Start()
    {
        WalkState = new WalkState(this);
        RunAwayState = new RunAwayState(this);

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

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

    public void SetSpeed()
    {
        float speed = 0;

        if(currentState == WalkState)
        {
            speed = Random.Range(minWalkSpeed, maxWalkSpeed);
        }
        else if(currentState == RunAwayState)
        {
            speed = Random.Range(minRunSpeed, maxRunSpeed);
        }

        agent.speed = speed;
        animator.SetFloat("Forward", agent.speed/2);
    }

    public void SetDestination()
    {
        agent.isStopped = false;
        NavMeshHit hit;
        if(NavMesh.SamplePosition(chosenDestination.position, out hit, 100, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
        else
        {
            ChooseNextAction();
        }
    }

    public bool ReachedDestination()
    {
        return agent.remainingDistance <= agent.stoppingDistance;
    }

    public bool IsReachedPossible()
    {
        timeSinceLastDestinationSet += Time.deltaTime;
        return timeSinceLastDestinationSet >= 1;
    }

    public void ChooseNextAction()
    {
        int randomId;

        int actionPercentage = Random.Range(0, 100);

        timeSinceLastDestinationSet = 0;

        if(actionPercentage < 30)
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

    public void ReactToZombie()
    {
        StartCoroutine(RandomizeReactionTime());
    }

    IEnumerator RandomizeReactionTime()
    {
        float reactionTime = Random.Range(minReactionTime, maxReactionTime);
        yield return new WaitForSeconds(reactionTime);
        PlaySound(possibleScaredClips, false);
        ChangeToState(RunAwayState);
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
        GetComponent<Collider>().enabled = false;
        Destroy(gameObject, 0.5f);
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

    public void SetCityPoints(Transform[] cityPoints, Transform[] buildingPoints, Transform[] safePoints)
    {
        cityDestinations = cityPoints;
        buildingDestinations = buildingPoints;
        this.safePoints = safePoints;
    }

    private void PlaySound(AudioClip[] possibleSounds, bool shouldLoop)
    {
        audioSource.clip = possibleSounds[Random.Range(0, possibleSounds.Length)];
        audioSource.loop = shouldLoop;
        audioSource.Play();
    }

    public void GetRunOver()
    {
        Debug.Log("Got run over");
        PlaySound(possibleRunOverClips, false);
        Instantiate(runOverParticles, transform.position, Quaternion.identity);
        currentState = null;
        GetComponent<Collider>().enabled = false;
        agent.enabled = false;
        SkinnedMeshRenderer[] meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer mr in meshRenderers)
            mr.enabled = false;
        Destroy(gameObject, 2f);
    }
}
