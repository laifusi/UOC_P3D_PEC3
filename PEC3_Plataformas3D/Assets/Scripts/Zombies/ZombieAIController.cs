using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAIController : MonoBehaviour
{
    [SerializeField] private float attackDistance;
    [SerializeField] private float zombifyDistance = 2;
    [SerializeField] private float attackDamage = 20;
    [SerializeField] private float timeBetweenFollowRecalculation = 1;
    [SerializeField] private float wanderRadius = 5;
    [SerializeField] private float moveSpeed = 0.2f;
    [SerializeField] private float runSpeed = 3f;
    [SerializeField] private float runAfterPedestrianSpeed = 5f;
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private GameObject dieParticles;
    [SerializeField] private float deathFixMultiplier = 1;
    [SerializeField] private Rigidbody[] deathDropPrefabs;
    [SerializeField] private float possibilityOfDrop = 40;
    [SerializeField] private Vector3 dropForce;
    [SerializeField] private AudioClip[] possibleWanderSounds;
    [SerializeField] private AudioClip[] possibleFollowSounds;
    [SerializeField] private AudioClip[] possibleHurtSounds;
    [SerializeField] private AudioClip[] possibleAttackSounds;
    [SerializeField] private GameObject runOverParticles;

    public WanderState WanderState { get; private set; }    // Wander state
    public FollowState FollowState { get; private set; }    // Follow state
    public AttackState AttackState { get; private set; }    // AttackState state

    public Action<float> OnLifeChange;
    public static Action<bool, bool, bool> OnItemDropped;

    private IAIState currentState;  // IZombieState for the current state
    private NavMeshAgent navMeshAgent;  // NavMeshAgent component
    private Vector3 playerPos;
    private float timeSinceFollowUpdate;
    private Animator animator;
    private bool canAttack = true;
    private Health player;
    private float health;
    private PedestrianAIController pedestrian;
    private Vector3 pedestrianPos;
    private AudioSource audioSource;
    private bool isFollowingPedestrian;

    private void Start()
    {
        health = maxHealth;

        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        WanderState = new WanderState(this);
        FollowState = new FollowState(this);
        AttackState = new AttackState(this);

        ChangeToState(WanderState);

        Health.OnDeath += DeactivateAI;
        ZombieSpawner.OnNoActivePoints += DeactivateAI;
    }

    private void Update()
    {
        timeSinceFollowUpdate += Time.deltaTime;

        currentState?.UpdateState();
    }

    public void ChangeToState(IAIState state)
    {
        currentState?.ExitState();
        currentState = state;
        currentState?.EnterState();
    }

    public void StartMove()
    {
        PlaySound(possibleWanderSounds, true);
        animator.SetBool("Running", false);
        animator.SetBool("Walking", true);
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = moveSpeed;
    }

    public void StartRun()
    {
        PlaySound(possibleFollowSounds, true);
        animator.SetBool("Running", true);
        navMeshAgent.isStopped = false;
        if(player != null || pedestrian == null)
        {
            navMeshAgent.speed = runSpeed;
        }
        else if(pedestrian != null)
        {
            navMeshAgent.speed = runAfterPedestrianSpeed;
        }
    }

    /// <summary>
    /// We set a new random Wander destination inside the wander radius
    /// </summary>
    public void RandomizeDestination()
    {
        float randomX = transform.position.x + UnityEngine.Random.Range(-wanderRadius, wanderRadius);
        float randomZ = transform.position.z + UnityEngine.Random.Range(-wanderRadius, wanderRadius);
        Vector3 randomPosition = new Vector3(randomX, wanderRadius, randomZ);

        NavMeshHit hit;
        if(NavMesh.SamplePosition(randomPosition, out hit, wanderRadius, NavMesh.AllAreas))
        {
            navMeshAgent.SetDestination(hit.position);
        }
    }

    /// <summary>
    /// We check if we reached the nav mesh destination
    /// If it's not the wander state we check if the player is in attack distance
    /// </summary>
    public bool ReachedDestination(bool isWander)
    {
        if (isWander)
        {
            return navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance;
        }
        else
        {
            if (!isFollowingPedestrian)
                return Vector3.Distance(playerPos, transform.position) <= attackDistance;
            else
                return Vector3.Distance(pedestrianPos, transform.position) <= zombifyDistance || pedestrian == null;
        }
    }

    /// <summary>
    /// We get the player's position translated to a nav mesh point
    /// </summary>
    public void SetFollowDestination()
    {
        timeSinceFollowUpdate = 0;
        NavMeshHit hit;
        if (player != null && NavMesh.SamplePosition(playerPos, out hit, wanderRadius, NavMesh.AllAreas))
        {
            isFollowingPedestrian = false;
            navMeshAgent.SetDestination(hit.position);
        }
        else if(pedestrian != null && NavMesh.SamplePosition(pedestrianPos, out hit, wanderRadius, NavMesh.AllAreas))
        {
            isFollowingPedestrian = true;
            navMeshAgent.SetDestination(hit.position);
        }
    }

    /// <summary>
    /// We check if we should recalculate the player's position in the follow state
    /// </summary>
    public bool ShouldRecalculateDestination()
    {
        return timeSinceFollowUpdate >= timeBetweenFollowRecalculation;
    }

    public void StopMovement()
    {
        animator.SetBool("Running", false);
        animator.SetBool("Walking", false);
        navMeshAgent.SetDestination(transform.position);
        navMeshAgent.isStopped = true;
    }

    public bool ShouldAttack => canAttack;

    /// <summary>
    /// Method used as an event of the attack animation to control attack timing
    /// </summary>
    public void EnableAttack(int canAttack)
    {
        this.canAttack = canAttack == 0 ? false : true;
    }

    /// <summary>
    /// Method to rotate the enemy to look at the player
    /// </summary>
    public void LookAtPlayer()
    {
        Vector3 lookDirection = playerPos - transform.position;

        transform.rotation = Quaternion.FromToRotation(Vector3.forward, new Vector3(lookDirection.x, 0, lookDirection.z));
    }

    public void Attack()
    {
        PlaySound(possibleAttackSounds, false);
        animator.SetTrigger("Attack");
    }

    /// <summary>
    /// Method used as an event of the animation to apply damage to the player if he is within distance
    /// </summary>
    public void HitPlayer()
    {
        if(Vector3.Distance(playerPos, transform.position) <= attackDistance)
        {
            player?.GetHurt(attackDamage);
        }
    }

    public void TriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerPos = other.transform.position;
            player = other.GetComponent<Health>();
            currentState?.OnTriggerEnter();
        }
        else if(other.CompareTag("Pedestrian"))
        {
            pedestrianPos = other.transform.position;
            pedestrian = other.GetComponent<PedestrianAIController>();
            pedestrian.OnZombieTriggerEnter();
            currentState?.OnTriggerEnter();
        }
    }

    public void TriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerPos = other.transform.position;
            currentState?.OnTriggerStay();
        }
        else if (other.CompareTag("Pedestrian"))
        {
            pedestrianPos = other.transform.position;
            pedestrian = other.GetComponent<PedestrianAIController>();
            pedestrian.OnZombieTriggerStay();
            currentState?.OnTriggerStay();
        }
    }

    public void TriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerPos = Vector3.zero;
            player = null;
            currentState?.OnTriggerExit();
        }
        else if (other.CompareTag("Pedestrian"))
        {
            pedestrianPos = Vector3.zero;
            pedestrian.OnZombieTriggerExit();
            pedestrian = null;
            currentState?.OnTriggerExit();
        }
    }

    /// <summary>
    /// Method to get damaged and cache the player's attacking position
    /// </summary>
    /// <param name="damage">Amount of damage received</param>
    /// <param name="attackedFrom">Position the player attacked from</param>
    public void GetHurt(float damage, Vector3 attackedFrom)
    {
        playerPos = attackedFrom;
        
        health -= damage;
        OnLifeChange?.Invoke(health);
        animator.SetTrigger("GetHurt");

        PlaySound(possibleHurtSounds, false);

        if(health <= 0)
        {
            Die();
        }

        currentState?.GetHit();
    }

    /// <summary>
    /// Die method: deactivate functionalities, instantiate particle systems and animations and drop an item, if it applies
    /// </summary>
    private void Die()
    {
        StopMovement();
        currentState = null;
        animator.SetBool("Dead", true);
        Instantiate(dieParticles, transform.position, Quaternion.identity);
        GetComponent<Collider>().enabled = false;
        navMeshAgent.enabled = false;
        StartCoroutine(FixDeathAnimation());
        DropItem();
        Destroy(gameObject, 10);
    }

    /// <summary>
    /// Drop an item on death
    /// </summary>
    private void DropItem()
    {
        if (UnityEngine.Random.Range(0, 100) < possibilityOfDrop)
        {
            int randomId = UnityEngine.Random.Range(0, deathDropPrefabs.Length);
            Rigidbody drop = Instantiate(deathDropPrefabs[randomId], transform.position, Quaternion.identity);
            drop.AddForce(dropForce);
            bool isHealthPack = drop.GetComponent<HealthPack>() != null;
            bool isAmmo = drop.GetComponent<Ammo>() != null;
            bool isKey = drop.GetComponent<Key>() != null;
            OnItemDropped?.Invoke(isHealthPack, isAmmo, isKey);
        }
    }

    /// <summary>
    /// Coroutine to fix the animation's positioning
    /// </summary>
    /// <returns></returns>
    IEnumerator FixDeathAnimation()
    {
        yield return new WaitForSeconds(1f);
        for (float i = 0; i >= -0.9f; i -= 0.01f)
        {
            yield return new WaitForSeconds(0.01f);
            transform.Translate(0, -0.01f*deathFixMultiplier, 0);
        }
    }

    /// <summary>
    /// Method to deactive AI on game ended
    /// </summary>
    private void DeactivateAI()
    {
        currentState = null;
    }

    /// <summary>
    /// Method to play a random sound from a list of sounds
    /// </summary>
    /// <param name="possibleSounds">list of sounds</param>
    /// <param name="shouldLoop">whether the sound should loop</param>
    private void PlaySound(AudioClip[] possibleSounds, bool shouldLoop)
    {
        audioSource.clip = possibleSounds[UnityEngine.Random.Range(0, possibleSounds.Length)];
        audioSource.loop = shouldLoop;
        audioSource.Play();
    }

    /// <summary>
    /// Method to react to being run over by a car
    /// We play a sound and some particles and destroy the zombie
    /// </summary>
    public void GetRunOver()
    {
        PlaySound(possibleHurtSounds, false);
        Instantiate(runOverParticles, transform.position, Quaternion.identity);
        Instantiate(dieParticles, transform.position, Quaternion.identity);
        currentState = null;
        GetComponent<Collider>().enabled = false;
        SkinnedMeshRenderer[] meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach(SkinnedMeshRenderer mr in meshRenderers)
            mr.enabled = false;
        navMeshAgent.enabled = false;
        Destroy(gameObject, 2f);
    }

    /// <summary>
    /// Method to define if a pedestrian disappeared and there's no one else to follow
    /// </summary>
    public bool NoOneToAttack()
    {
        return pedestrian == null && player == null;
    }

    /// <summary>
    /// Method that defines whether we are following a pedestrian or not
    /// </summary>
    public bool FollowingPedestrian()
    {
        bool wasFollowingPedestrian = isFollowingPedestrian;

        if(pedestrian == null)
        {
            isFollowingPedestrian = false;
        }

        return isFollowingPedestrian || wasFollowingPedestrian;
    }

    /// <summary>
    /// Method to notify the pedestrian it should turn into a zombie
    /// </summary>
    public void ZombifyPedestrian()
    {
        pedestrian.TurnIntoZombie();
    }

    private void OnDestroy()
    {
        Health.OnDeath -= DeactivateAI;
        ZombieSpawner.OnNoActivePoints -= DeactivateAI;
    }
}
