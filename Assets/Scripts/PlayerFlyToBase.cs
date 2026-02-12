//using UnityEngine;

//public class PlayerFlyToBase : MonoBehaviour
//{
//    [Header("Settings")]
//    public Transform baseTarget;
//    public float flyHeight = 40f;
//    public float flyDuration = 1.2f;

//    private bool isFlying = false;
//    private Vector3 startPos;
//    private float timer = 0f;

//    void Update()
//    {
//        if (isFlying)
//        {
//            timer += Time.deltaTime / flyDuration;
//            float t = Mathf.Clamp01(timer);

//            Vector3 horizontal = Vector3.Lerp(startPos, baseTarget.position, t);
//            float height = Mathf.Sin(t * Mathf.PI) * flyHeight;

//            transform.position = new Vector3(horizontal.x, baseTarget.position.y + height, horizontal.z);

//            if (t >= 1f)
//            {
//                isFlying = false;
//            }
//        }
//    }

//    // 👇 Make sure THIS function exists and name is EXACTLY same
//    public void LaunchToBase()
//    {
//        if (isFlying) return;

//        startPos = transform.position;
//        timer = 0f;
//        isFlying = true;
//    }
//}






using UnityEngine;

public class PlayerFlyToBase : MonoBehaviour
{
    [Header("Settings")]
    public Transform baseTarget;
    public float flyHeight = 40f;
    public float flyDuration = 1.2f;

    private bool isFlying = false;
    private Vector3 startPos;
    private float timer = 0f;

    void Update()
    {
        if (isFlying)
        {
            timer += Time.deltaTime / flyDuration;
            float t = Mathf.Clamp01(timer);

            Vector3 horizontal = Vector3.Lerp(startPos, baseTarget.position, t);
            float height = Mathf.Sin(t * Mathf.PI) * flyHeight;

            transform.position = new Vector3(horizontal.x, baseTarget.position.y + height, horizontal.z);

            if (t >= 1f)
            {
                isFlying = false;
            }
        }
    }

    public void LaunchToBase()
    {
        if (isFlying) return;

        startPos = transform.position;
        timer = 0f;
        isFlying = true;
    }

    // 🧩 Enemy collision trigger
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("EnemyCircle"))
        {

            if (TutorialBrainrotGuide.Instance != null)
            {
                TutorialBrainrotGuide.Instance.ResetTutorialToStart();
            }

            Debug.Log("💥 Player hit EnemyCircle — returning to base!");

            LaunchToBase();

            if (BrainrotManager.instance != null)
                BrainrotManager.instance.ReturnHeldBrainrotToOrigin();

        }
    }

}
