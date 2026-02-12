





//using UnityEngine;
//using UnityEngine.AI;
//using System.Collections;

//[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
//public class NavMeshWander : MonoBehaviour
//{
//    [Header("Wander Settings")]
//    public bool useGlobalNavMeshArea = true;
//    public float wanderRadius = 6f;
//    public float pickInterval = 2.0f;
//    public float reachedDistance = 0.5f;
//    public bool useRandomWait = false;
//    public Vector2 randomWaitRange = new Vector2(0.2f, 1.5f);

//    [Header("Agent Settings")]
//    public float agentSpeed = 3.5f;
//    public float agentAcceleration = 8f;

//    [Header("Animation Settings")]
//    public AnimationClip walkClip;     // 👈 assign different clip per character
//    public AnimationClip idleClip;     // optional idle animation

//    private NavMeshAgent agent;
//    private Animator animator;
//    private Coroutine wanderRoutine;

//    private static Bounds globalNavMeshBounds;
//    private static bool boundsInitialized = false;

//    void Awake()
//    {
//        agent = GetComponent<NavMeshAgent>();
//        animator = GetComponent<Animator>();
//    }

//    void Start()
//    {
//        if (useGlobalNavMeshArea && !boundsInitialized)
//        {
//            globalNavMeshBounds = CalculateNavMeshBounds();
//            boundsInitialized = true;
//        }

//        // apply agent settings
//        agent.speed = agentSpeed;
//        agent.acceleration = agentAcceleration;

//        // 🔹 Override clips dynamically
//        if (walkClip != null)
//        {
//            var overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
//            if (idleClip != null)
//                overrideController["Idle"] = idleClip;
//            overrideController["Walk"] = walkClip;
//            animator.runtimeAnimatorController = overrideController;
//        }

//        wanderRoutine = StartCoroutine(WanderCoroutine());
//    }

//    void Update()
//    {
//        // 🔹 control animation parameter
//        if (animator != null)
//        {
//            bool isWalking = agent.velocity.magnitude > 0.1f;
//            animator.SetBool("isWalking", isWalking);
//        }
//    }

//    IEnumerator WanderCoroutine()
//    {
//        while (true)
//        {
//            if (!agent.pathPending && (agent.remainingDistance <= reachedDistance || agent.pathStatus != NavMeshPathStatus.PathComplete))
//            {
//                Vector3 randomPos;
//                if (useGlobalNavMeshArea)
//                    randomPos = GetRandomGlobalPoint();
//                else
//                    GetRandomPointOnNavMesh(transform.position, wanderRadius, out randomPos);

//                agent.SetDestination(randomPos);
//            }

//            yield return new WaitForSeconds(useRandomWait
//                ? Random.Range(randomWaitRange.x, randomWaitRange.y)
//                : pickInterval);
//        }
//    }

//    Vector3 GetRandomGlobalPoint()
//    {
//        for (int i = 0; i < 40; i++)
//        {
//            Vector3 randomPos = new Vector3(
//                Random.Range(globalNavMeshBounds.min.x, globalNavMeshBounds.max.x),
//                globalNavMeshBounds.center.y,
//                Random.Range(globalNavMeshBounds.min.z, globalNavMeshBounds.max.z)
//            );
//            NavMeshHit hit;
//            if (NavMesh.SamplePosition(randomPos, out hit, 2f, NavMesh.AllAreas))
//                return hit.position;
//        }
//        return transform.position;
//    }

//    Bounds CalculateNavMeshBounds()
//    {
//        Vector3 size = new Vector3(500, 50, 500);
//        NavMeshHit hit;
//        NavMesh.SamplePosition(Vector3.zero, out hit, 1000f, NavMesh.AllAreas);
//        return new Bounds(hit.position, size);
//    }

//    bool GetRandomPointOnNavMesh(Vector3 center, float radius, out Vector3 result)
//    {
//        for (int i = 0; i < 30; i++)
//        {
//            Vector3 randomDir = Random.insideUnitSphere * radius + center;
//            NavMeshHit hit;
//            if (NavMesh.SamplePosition(randomDir, out hit, 1.5f, NavMesh.AllAreas))
//            {
//                result = hit.position;
//                return true;
//            }
//        }
//        result = center;
//        return false;
//    }

//    void OnDisable()
//    {
//        if (wanderRoutine != null)
//            StopCoroutine(wanderRoutine);
//    }
//}












///////////////////////////////////////////  idle and walk animation 


//using UnityEngine;
//using UnityEngine.AI;
//using System.Collections;

//[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
//public class NavMeshWander : MonoBehaviour
//{
//    [Header("Wander Settings")]
//    public bool useGlobalNavMeshArea = true;
//    public float wanderRadius = 6f;
//    public float pickInterval = 2.0f;
//    public float reachedDistance = 0.5f;
//    public bool useRandomWait = false;
//    public Vector2 randomWaitRange = new Vector2(0.5f, 2f);

//    [Header("Agent Settings")]
//    public float agentSpeed = 3.5f;
//    public float agentAcceleration = 8f;

//    [Header("Animation Settings")]
//    [Tooltip("Assign unique walk animation clip for this character")]
//    public AnimationClip walkClip;
//    [Tooltip("Assign idle animation clip for this character")]
//    public AnimationClip idleClip;

//    private NavMeshAgent agent;
//    private Animator animator;
//    private Coroutine wanderRoutine;
//    private bool wasMoving = false;

//    private static Bounds globalNavMeshBounds;
//    private static bool boundsInitialized = false;

//    void Awake()
//    {
//        agent = GetComponent<NavMeshAgent>();
//        animator = GetComponent<Animator>();
//    }

//    void Start()
//    {
//        // Cache navmesh bounds if using global wander
//        if (useGlobalNavMeshArea && !boundsInitialized)
//        {
//            globalNavMeshBounds = CalculateNavMeshBounds();
//            boundsInitialized = true;
//        }

//        // apply agent settings
//        agent.speed = agentSpeed;
//        agent.acceleration = agentAcceleration;

//        // override animator clips dynamically
//        if (walkClip != null || idleClip != null)
//        {
//            var overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
//            if (walkClip != null) overrideController["Walk"] = walkClip;
//            if (idleClip != null) overrideController["Idle"] = idleClip;
//            animator.runtimeAnimatorController = overrideController;
//        }

//        // start wandering behavior
//        wanderRoutine = StartCoroutine(WanderCoroutine());
//    }

//    void Update()
//    {
//        bool isMoving = agent.velocity.magnitude > 0.1f;

//        if (animator != null)
//        {
//            animator.SetBool("isWalking", isMoving);
//        }

//        // detect change (optional debug)
//        if (isMoving != wasMoving)
//        {
//            wasMoving = isMoving;
//        }
//    }

//    IEnumerator WanderCoroutine()
//    {
//        while (true)
//        {
//            if (!agent.pathPending && (agent.remainingDistance <= reachedDistance || agent.pathStatus != NavMeshPathStatus.PathComplete))
//            {
//                Vector3 randomPos;
//                if (useGlobalNavMeshArea)
//                    randomPos = GetRandomGlobalPoint();
//                else
//                    GetRandomPointOnNavMesh(transform.position, wanderRadius, out randomPos);

//                agent.SetDestination(randomPos);
//            }

//            // Wait between moves
//            yield return new WaitForSeconds(useRandomWait
//                ? Random.Range(randomWaitRange.x, randomWaitRange.y)
//                : pickInterval);
//        }
//    }

//    Vector3 GetRandomGlobalPoint()
//    {
//        for (int i = 0; i < 40; i++)
//        {
//            Vector3 randomPos = new Vector3(
//                Random.Range(globalNavMeshBounds.min.x, globalNavMeshBounds.max.x),
//                globalNavMeshBounds.center.y,
//                Random.Range(globalNavMeshBounds.min.z, globalNavMeshBounds.max.z)
//            );
//            if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, 2f, NavMesh.AllAreas))
//                return hit.position;
//        }
//        return transform.position;
//    }

//    Bounds CalculateNavMeshBounds()
//    {
//        Vector3 size = new Vector3(500, 50, 500);
//        NavMesh.SamplePosition(Vector3.zero, out NavMeshHit hit, 1000f, NavMesh.AllAreas);
//        return new Bounds(hit.position, size);
//    }

//    bool GetRandomPointOnNavMesh(Vector3 center, float radius, out Vector3 result)
//    {
//        for (int i = 0; i < 30; i++)
//        {
//            Vector3 randomDir = Random.insideUnitSphere * radius + center;
//            if (NavMesh.SamplePosition(randomDir, out NavMeshHit hit, 1.5f, NavMesh.AllAreas))
//            {
//                result = hit.position;
//                return true;
//            }
//        }
//        result = center;
//        return false;
//    }

//    void OnDisable()
//    {
//        if (wanderRoutine != null)
//            StopCoroutine(wanderRoutine);
//    }
//}



using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class NavMeshWanderFinal : MonoBehaviour
{
    public enum WanderMode { Local, Global }

    [Header("Wander Mode")]
    public WanderMode mode = WanderMode.Local;

    [Tooltip("Local mode: agent sirf spawn point ke gird is radius me chalega")]
    public float localRadius = 8f;

    [Tooltip("New destination pick karne ka base interval")]
    public float pickInterval = 2f;

    [Tooltip("Itne paas aane par destination reached samjho")]
    public float reachedDistance = 0.5f;

    [Tooltip("Har dafa thora random wait bhi ho")]
    public bool useRandomWait = true;

    [Tooltip("Random wait ka range")]
    public Vector2 randomWaitRange = new Vector2(0.4f, 1.6f);

    [Header("Agent Settings")]
    public float agentSpeed = 3.5f;
    public float agentAcceleration = 10f;
    public float agentAngularSpeed = 240f;
    public float stoppingDistance = 0.1f;

    [Header("Animation (optional override)")]
    public AnimationClip walkClip;  // Animator me "Walk" state hona chahiye
    public AnimationClip idleClip;  // Animator me "Idle" state hona chahiye

    [Header("Advanced Safety")]
    [Tooltip("Agar velocity bohat der 0 ke qareeb rahe to naya point choose karo")]
    public float stuckSeconds = 1.8f;
    [Tooltip("isWalking ko set/clear karne ka threshold")]
    public float walkSpeedThreshold = 0.15f;

    private NavMeshAgent agent;
    private Animator animator;
    private Coroutine loop;
    private Vector3 spawnPoint;
    private float stuckTimer;

    // Global bounds cache
    private static bool globalBoundsReady = false;
    private static Bounds globalBounds;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        spawnPoint = transform.position;

        // Safe defaults
        agent.updatePosition = true;
        agent.updateRotation = true;
        agent.speed = agentSpeed;
        agent.acceleration = agentAcceleration;
        agent.angularSpeed = agentAngularSpeed;
        agent.stoppingDistance = stoppingDistance;
        agent.autoBraking = true;

        // Animator override (optional)
        if (walkClip != null)
        {
            var ovr = new AnimatorOverrideController(animator.runtimeAnimatorController);
            if (idleClip != null) ovr["Idle"] = idleClip;
            ovr["Walk"] = walkClip;
            animator.runtimeAnimatorController = ovr;
        }
    }

    void Start()
    {
        if (mode == WanderMode.Global && !globalBoundsReady)
        {
            // Precise bounds from baked navmesh
            var tri = NavMesh.CalculateTriangulation();
            if (tri.vertices != null && tri.vertices.Length > 0)
            {
                var b = new Bounds(tri.vertices[0], Vector3.zero);
                for (int i = 1; i < tri.vertices.Length; i++) b.Encapsulate(tri.vertices[i]);
                globalBounds = b;
                globalBoundsReady = true;
            }
            else
            {
                // Fallback if triangulation not available
                globalBounds = new Bounds(spawnPoint, new Vector3(500, 50, 500));
                globalBoundsReady = true;
            }
        }

        // Ensure we are on a NavMesh; if not, snap to nearest
        SnapToNearestNavMesh(transform.position, out Vector3 snapPos);
        if ((snapPos - transform.position).sqrMagnitude > 0.0001f)
            agent.Warp(snapPos);

        loop = StartCoroutine(WanderLoop());
    }

    void Update()
    {
        // Animator sync
        float speed = agent.velocity.magnitude;
        bool isWalking = speed > walkSpeedThreshold && !agent.isStopped;
        animator.SetBool("isWalking", isWalking);

        // Stop sliding
        if (!isWalking && !agent.pathPending && agent.remainingDistance <= reachedDistance)
            agent.velocity = Vector3.zero;

        // STUCK detector (path pending false, distance > reached, velocity ~0)
        if (!agent.pathPending && agent.remainingDistance > reachedDistance)
        {
            if (speed < 0.05f)
            {
                stuckTimer += Time.deltaTime;
                if (stuckTimer >= stuckSeconds)
                {
                    // Hard reset
                    ForceNewDestination();
                    stuckTimer = 0f;
                }
            }
            else stuckTimer = 0f;
        }
        else stuckTimer = 0f;
    }

    IEnumerator WanderLoop()
    {
        while (true)
        {
            if (!agent.pathPending && agent.remainingDistance <= reachedDistance)
            {
                ForceNewDestination();
            }

            float wait = useRandomWait ? Random.Range(randomWaitRange.x, randomWaitRange.y) : pickInterval;
            yield return new WaitForSeconds(wait);
        }
    }

    void ForceNewDestination()
    {
        Vector3 target;
        bool got = (mode == WanderMode.Global)
            ? GetRandomPointGlobal(out target)
            : GetRandomPointLocal(spawnPoint, localRadius, out target);

        if (!got) return;

        // FINAL clamp: never set destination unless it’s on baked navmesh
        if (NavMesh.SamplePosition(target, out NavMeshHit hit, 1.5f, NavMesh.AllAreas))
        {
            agent.isStopped = false;
            agent.ResetPath();
            agent.SetDestination(hit.position);
#if UNITY_EDITOR
            Debug.DrawLine(transform.position, hit.position, Color.green, 2f);
#endif
        }
    }

    bool GetRandomPointLocal(Vector3 center, float radius, out Vector3 pos)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 rnd = center + Random.insideUnitSphere * radius;
            if (NavMesh.SamplePosition(rnd, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                pos = hit.position;
                return true;
            }
        }
        pos = center;
        return false;
    }

    bool GetRandomPointGlobal(out Vector3 pos)
    {
        if (!globalBoundsReady)
        {
            pos = transform.position;
            return false;
        }

        for (int i = 0; i < 40; i++)
        {
            Vector3 rnd = new Vector3(
                Random.Range(globalBounds.min.x, globalBounds.max.x),
                globalBounds.center.y,
                Random.Range(globalBounds.min.z, globalBounds.max.z)
            );
            if (NavMesh.SamplePosition(rnd, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                pos = hit.position;
                return true;
            }
        }
        pos = transform.position;
        return false;
    }

    bool SnapToNearestNavMesh(Vector3 from, out Vector3 snapped)
    {
        if (NavMesh.SamplePosition(from, out NavMeshHit hit, 5f, NavMesh.AllAreas))
        {
            snapped = hit.position;
            return true;
        }
        snapped = from;
        return false;
    }

    void OnDisable()
    {
        if (loop != null) StopCoroutine(loop);
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (mode == WanderMode.Local)
        {
            Gizmos.color = new Color(0f, 0.6f, 1f, 0.25f);
            Gizmos.DrawSphere(Application.isPlaying ? spawnPoint : transform.position, localRadius);
        }
    }
#endif
}
