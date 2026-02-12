







//using UnityEngine;
//using UnityEngine.AI;

//public class AISimpleMover : MonoBehaviour
//{
//    [Header("NavMesh")]
//    public NavMeshAgent agent;

//    [Header("Patrol Points")]
//    public Transform[] waypoints;
//    private int currentIndex = 0;

//    [Header("Idle Behavior")]
//    public float minIdleTime = 1f;
//    public float maxIdleTime = 3f;
//    private bool isIdle = false;
//    private float idleTimer = 0f;
//    private float idleDuration = 0f;

//    private AIAnimationController animControl;

//    // ---------------------------------------
//    // DEFENSE MODE (Player in base)
//    // ---------------------------------------
//    public BaseOwner owner;
//    private bool defending = false;
//    private Transform playerTarget;

//    void Start()
//    {
//        if (agent == null)
//            agent = GetComponent<NavMeshAgent>();

//        animControl = GetComponent<AIAnimationController>();
//        GoToNextPoint();
//    }

//    void Update()
//    {
//        // FALLING = AI disabled temporarily
//        if (animControl.isFallen)
//            return;

//        // RUNNING ANIMATION
//        float spd = agent.desiredVelocity.magnitude;
//        animControl.SetRunning(spd > 0.1f);

//        // ---------------------------------------
//        // DEFENSE / ATTACK MODE
//        // ---------------------------------------
//        if (defending && playerTarget != null)
//        {
//            agent.isStopped = false;
//            agent.SetDestination(playerTarget.position);

//            float dist = Vector3.Distance(transform.position, playerTarget.position);

//            if (dist < 1.8f)
//            {
//                animControl.Attack();
//            }

//            return;
//        }

//        // ---------------------------------------
//        // IDLE SYSTEM
//        // ---------------------------------------
//        if (isIdle)
//        {
//            agent.isStopped = true;
//            idleTimer += Time.deltaTime;

//            if (idleTimer >= idleDuration)
//            {
//                isIdle = false;
//                agent.isStopped = false;
//                GoToNextPoint();
//            }
//            return;
//        }

//        // ---------------------------------------
//        // WAYPOINT REACHED
//        // ---------------------------------------
//        if (!agent.pathPending && agent.remainingDistance < 0.3f)
//        {
//            isIdle = true;
//            idleTimer = 0f;
//            idleDuration = Random.Range(minIdleTime, maxIdleTime);
//        }
//    }

//    // ---------------------------------------
//    // PATROL
//    // ---------------------------------------
//    private void GoToNextPoint()
//    {
//        if (waypoints.Length == 0) return;

//        currentIndex++;
//        if (currentIndex >= waypoints.Length)
//            currentIndex = 0;

//        agent.SetDestination(waypoints[currentIndex].position);
//    }

//    // ---------------------------------------
//    // DEFENSE MODE
//    // ---------------------------------------
//    public void StartDefense(Transform player)
//    {
//        defending = true;
//        playerTarget = player;

//        isIdle = false;
//        agent.isStopped = false;
//    }

//    public void StopDefense()
//    {
//        defending = false;
//        playerTarget = null;

//        GoToNextPoint();
//    }
//}



using UnityEngine;
using UnityEngine.AI;

public class AISimpleMover : MonoBehaviour
{
    [Header("NavMesh")]
    public NavMeshAgent agent;

    [Header("Patrol Points")]
    public Transform[] waypoints;
    private int currentIndex = 0;

    [Header("Idle Behavior")]
    public float minIdleTime = 1f;
    public float maxIdleTime = 3f;

    private bool isIdle = false;
    private float idleTimer = 0f;
    private float idleDuration = 0f;

    private AIAnimationController animControl;

    // DEFENSE MODE
    public BaseOwner owner;
    private bool defending = false;
    private Transform playerTarget;

    void Start()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        animControl = GetComponent<AIAnimationController>();
        GoToNextPoint();
    }

    void Update()
    {
        // -------------------------------------------------
        // 1) STOP EVERYTHING IF AGENT DISABLED
        // -------------------------------------------------
        if (agent == null || !agent.enabled)
        {
            animControl.SetRunning(false);
            return;
        }

        // -------------------------------------------------
        // 2) STOP EVERYTHING DURING ANIMATION LOCK
        // -------------------------------------------------
        if (animControl.animationLock || animControl.isFallen)
        {
            agent.isStopped = true;
            animControl.SetRunning(false);
            return;
        }

        // ENABLE MOVEMENT AGAIN
        agent.isStopped = false;

        // -------------------------------------------------
        // RUNNING ANIMATION
        // -------------------------------------------------
        float spd = agent.desiredVelocity.magnitude;
        animControl.SetRunning(spd > 0.1f);

        // -------------------------------------------------
        // DEFENSE MODE (CHASE PLAYER)
        // -------------------------------------------------
        if (defending && playerTarget != null)
        {
            agent.SetDestination(playerTarget.position);

            float dist = Vector3.Distance(transform.position, playerTarget.position);
            if (dist < 1.8f)
            {
                animControl.Attack();
            }

            return;
        }

        // -------------------------------------------------
        // IDLE BEHAVIOR
        // -------------------------------------------------
        if (isIdle)
        {
            agent.isStopped = true;
            idleTimer += Time.deltaTime;

            if (idleTimer >= idleDuration)
            {
                isIdle = false;
                agent.isStopped = false;
                GoToNextPoint();
            }
            return;
        }

        // -------------------------------------------------
        // WAYPOINT REACHED
        // -------------------------------------------------
        if (!agent.pathPending && agent.remainingDistance < 0.3f)
        {
            isIdle = true;
            idleTimer = 0f;
            idleDuration = Random.Range(minIdleTime, maxIdleTime);
        }
    }

    // -------------------------------------------------
    // PATROL SYSTEM
    // -------------------------------------------------
    private void GoToNextPoint()
    {
        if (waypoints.Length == 0)
            return;

        currentIndex++;
        if (currentIndex >= waypoints.Length)
            currentIndex = 0;

        agent.SetDestination(waypoints[currentIndex].position);
    }

    // -------------------------------------------------
    // DEFENSE MODE
    // -------------------------------------------------
    public void StartDefense(Transform player)
    {
        defending = true;
        playerTarget = player;
        isIdle = false;
        agent.isStopped = false;
    }

    public void StopDefense()
    {
        defending = false;
        playerTarget = null;
        GoToNextPoint();
    }
}
