








////using SUPERCharacter;
////using UnityEngine;
////using UnityEngine.AI;

////public class AIAnimationController : MonoBehaviour
////{
////    public Animator anim;
////    public GameObject weapon;

////    public bool isFallen = false;

////    private Rigidbody rb;
////    private NavMeshAgent agent;

////    private void Start()
////    {
////        rb = GetComponent<Rigidbody>();
////        agent = GetComponent<NavMeshAgent>();

////        if (rb != null)
////        {
////            rb.isKinematic = true;   // normal state me no physics push
////        }
////    }

////    // -------------------------------------------------------
////    // FALL WITH FORCE
////    // -------------------------------------------------------
////    public void Fall(Vector3 hitDirection)
////    {
////        if (isFallen) return;

////        isFallen = true;

////        // Stop AI movement
////        if (agent != null) agent.enabled = false;

////        // Enable physics push
////        rb.isKinematic = false;

////        // Add push-back force
////        Vector3 push = (hitDirection.normalized * 6f) + (Vector3.up * 3f);
////        rb.AddForce(push, ForceMode.Impulse);

////        anim.SetBool("Run", false);
////        anim.SetTrigger("Fall");

////        // Stand up
////        Invoke(nameof(StandUp), 1.2f);
////    }

////    // Normal Fall call (if no push needed)
////    public void Fall()
////    {
////        // default direction: backward from AI forward
////        Fall(-transform.forward);
////    }

////    // -------------------------------------------------------
////    // STAND UP
////    // -------------------------------------------------------
////    private void StandUp()
////    {
////        anim.SetTrigger("StandUp");

////        // Lock AI back to non-physics
////        rb.isKinematic = true;
////        if (agent != null) agent.enabled = true;

////        Invoke(nameof(AllowMove), 0.1f);
////    }

////    private void AllowMove()
////    {
////        isFallen = false;
////    }

////    // -------------------------------------------------------
////    // RUNNING
////    // -------------------------------------------------------
////    public void SetRunning(bool value)
////    {
////        if (isFallen) return;
////        anim.SetBool("Run", value);
////    }

////    // -------------------------------------------------------
////    // ATTACK
////    // -------------------------------------------------------
////    public void Attack()
////    {
////        if (isFallen) return;
////        anim.SetTrigger("Attack");
////    }

////    public void ShowWeapon() => weapon?.SetActive(true);
////    public void HideWeapon() => weapon?.SetActive(false);

////    // -------------------------------------------------------
////    // DEAL DAMAGE (to player)
////    // -------------------------------------------------------
////    public void DealDamage()
////    {
////        float radius = 1.6f;
////        Collider[] hits = Physics.OverlapSphere(transform.position, radius);

////        foreach (Collider hit in hits)
////        {
////            SUPERCharacterAIO player = hit.GetComponent<SUPERCharacterAIO>();
////            if (player != null)
////            {
////                player.GetHitByAI();
////                return;
////            }
////        }
////    }
////}



//using SUPERCharacter;
//using UnityEngine;
//using UnityEngine.AI;

//public class AIAnimationController : MonoBehaviour
//{
//    public Animator anim;
//    public GameObject weapon;

//    public bool isFallen = false;
//    public bool animationLock = false;

//    private Rigidbody rb;
//    private NavMeshAgent agent;

//    private void Start()
//    {
//        rb = GetComponent<Rigidbody>();
//        agent = GetComponent<NavMeshAgent>();

//        if (rb != null)
//            rb.isKinematic = true;
//    }

//    // -------------------------------------------------------
//    // FALL WITH FORCE
//    // -------------------------------------------------------
//    public void Fall(Vector3 hitDirection)
//    {
//        if (isFallen)
//            return;

//        isFallen = true;
//        animationLock = true;

//        // disable AI movement
//        if (agent != null)
//            agent.enabled = false;

//        rb.isKinematic = false;

//        // push back effect
//        Vector3 push = hitDirection.normalized * 6f + Vector3.up * 3f;
//        rb.AddForce(push, ForceMode.Impulse);

//        anim.SetBool("Run", false);
//        anim.SetTrigger("Fall");

//        // go to stand up animation
//        Invoke(nameof(StandUp), 1.2f);
//    }

//    // fallback if Fall() called without direction
//    public void Fall()
//    {
//        Fall(-transform.forward);
//    }

//    // -------------------------------------------------------
//    // STAND UP
//    // -------------------------------------------------------
//    private void StandUp()
//    {
//        anim.SetTrigger("StandUp");

//        rb.isKinematic = true;

//        // enable NavMeshAgent AFTER animation begins to stand
//        Invoke(nameof(EnableAgentAfterStand), 0.6f);

//        // finish movement lock after animation ends
//        Invoke(nameof(AllowMove), 0.7f);
//    }

//    private void EnableAgentAfterStand()
//    {
//        if (agent != null)
//            agent.enabled = true;
//    }

//    private void AllowMove()
//    {
//        isFallen = false;
//        animationLock = false;
//    }

//    // -------------------------------------------------------
//    // RUNNING
//    // -------------------------------------------------------
//    public void SetRunning(bool value)
//    {
//        if (isFallen || animationLock)
//            return;

//        anim.SetBool("Run", value);
//    }

//    // -------------------------------------------------------
//    // ATTACK
//    // -------------------------------------------------------
//    public void Attack()
//    {
//        if (isFallen || animationLock)
//            return;

//        animationLock = true;
//        anim.SetTrigger("Attack");

//        Invoke(nameof(AllowMove), 0.7f);
//    }

//    public void ShowWeapon() => weapon?.SetActive(true);
//    public void HideWeapon() => weapon?.SetActive(false);

//    // -------------------------------------------------------
//    // DAMAGE PLAYER
//    // -------------------------------------------------------
//    public void DealDamage()
//    {
//        float radius = 1.6f;
//        Collider[] hits = Physics.OverlapSphere(transform.position, radius);

//        foreach (Collider hit in hits)
//        {
//            SUPERCharacterAIO player = hit.GetComponent<SUPERCharacterAIO>();
//            if (player != null)
//            {
//                player.GetHitByAI();
//                return;
//            }
//        }
//    }
//}


using SUPERCharacter;
using UnityEngine;
using UnityEngine.AI;

public class AIAnimationController : MonoBehaviour
{
    public Animator anim;
    public GameObject weapon;

    public bool isFallen = false;
    public bool animationLock = false;

    private Rigidbody rb;
    private NavMeshAgent agent;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();

        if (rb != null)
            rb.isKinematic = true;
    }

    // -------------------------------------------------------
    // FALL WITH FORCE
    // -------------------------------------------------------
    public void Fall(Vector3 hitDirection)
    {
        if (isFallen)
            return;

        isFallen = true;
        animationLock = true;

        // Disable AI movement while falling
        if (agent != null)
            agent.enabled = false;

        rb.isKinematic = false;

        // Push force effect
        Vector3 push = hitDirection.normalized * 6f + Vector3.up * 3f;
        rb.AddForce(push, ForceMode.Impulse);

        anim.SetBool("Run", false);
        anim.SetTrigger("Fall");
        AudioManager.PlayAISound();
        // Stand up after delay
        Invoke(nameof(StandUp), 1.2f);
    }

    // If no direction provided
    public void Fall()
    {
        Fall(-transform.forward);
    }

    // -------------------------------------------------------
    // STAND UP
    // -------------------------------------------------------
    private void StandUp()
    {
        anim.SetTrigger("StandUp");

        rb.isKinematic = true;

        Invoke(nameof(EnableAgentAfterStand), 0.6f);
        Invoke(nameof(AllowMove), 0.7f);
    }

    private void EnableAgentAfterStand()
    {
        if (agent != null)
            agent.enabled = true;
    }

    private void AllowMove()
    {
        isFallen = false;
        animationLock = false;
    }

    // -------------------------------------------------------
    // RUN ANIMATION
    // -------------------------------------------------------
    public void SetRunning(bool value)
    {
        if (isFallen || animationLock)
            return;

        anim.SetBool("Run", value);
    }

    // -------------------------------------------------------
    // ATTACK
    // -------------------------------------------------------
    public void Attack()
    {
        
        if (isFallen || animationLock)
            return;

        animationLock = true;
        anim.SetTrigger("Attack");

        Invoke(nameof(AllowMove), 0.7f);
    }

    public void ShowWeapon() => weapon?.SetActive(true);
    public void HideWeapon() => weapon?.SetActive(false);

    // -------------------------------------------------------
    // DAMAGE PLAYER
    // -------------------------------------------------------
    public void DealDamage()
    {
        
        float radius = 1.6f;
        Collider[] hits = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider hit in hits)
        {
            SUPERCharacterAIO player = hit.GetComponent<SUPERCharacterAIO>();
            if (player != null)
            {
                Debug.Log("AI HIT PLAYER → RETURNING STOLEN BRAINROT");

                // 👇 first return stolen brainrot
                TryReturnPlayerBrainrot();

                // then apply player hit / respawn
                player.GetHitByAI();
                return;
            }
        }
    }

    // -------------------------------------------------------
    // RETURN PLAYER'S BRAINROT
    // -------------------------------------------------------
    private void TryReturnPlayerBrainrot()
    {
        if (BrainrotManager.instance == null)
            return;

        BrainrotInteraction held = BrainrotManager.instance.heldBrainrot;

        if (held != null)
        {
            Debug.Log("⚠️ Stolen Brainrot returned to origin!");

            held.ReturnToOrigin();

            BrainrotManager.instance.heldBrainrot = null;

            BrainrotUIManager.instance.UpdateActionButton(false);
            BrainrotUIManager.instance.HideMessage();
        }
    }
}
