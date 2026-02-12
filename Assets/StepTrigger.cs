
//////using UnityEngine;

//////public class StepTrigger : MonoBehaviour
//////{
//////    public int stepNumber = 1;

//////    private void OnTriggerEnter(Collider other)
//////    {
//////        if (!other.CompareTag("Player")) return;

//////        TutorialManager.Instance.TriggerStep(stepNumber);
//////    }

//////    private void OnTriggerExit(Collider other)
//////    {
//////        if (!other.CompareTag("Player")) return;

//////        TutorialManager.Instance.ExitStep(stepNumber);
//////    }
//////}


////using UnityEngine;

////public class StepTrigger : MonoBehaviour
////{
////    public int stepNumber;  // 1, 2, or 3

////    private void OnTriggerEnter(Collider other)
////    {
////        if (!other.CompareTag("Player")) return;

////        // Step order check
////        if (stepNumber == 1)
////        {
////            TutorialManager.Instance.TriggerStep(1);
////        }
////        else if (stepNumber == 2 && TutorialManager.Instance.step1Done)
////        {
////            TutorialManager.Instance.TriggerStep(2);
////        }
////        else if (stepNumber == 3 && TutorialManager.Instance.step2Done)
////        {
////            TutorialManager.Instance.TriggerStep(3);
////        }
////    }

////    private void OnTriggerExit(Collider other)
////    {
////        if (!other.CompareTag("Player")) return;

////        TutorialManager.Instance.ExitStep(stepNumber);
////    }
////}



//using UnityEngine;

//public class StepTrigger : MonoBehaviour
//{
//    public int stepNumber; // 1, 2, 3

//    private void OnTriggerEnter(Collider other)
//    {
//        if (!other.CompareTag("Player")) return;

//        // 🔒 Tutorial already completed? DO NOTHING
//        if (TutorialManager.Instance == null) return;
//        if (TutorialManager.Instance.IsTutorialCompleted())
//            return;

//        if (stepNumber == 1)
//        {
//            TutorialManager.Instance.TriggerStep(1);
//        }
//        else if (stepNumber == 2 && TutorialManager.Instance.step1Done)
//        {
//            TutorialManager.Instance.TriggerStep(2);
//        }
//        else if (stepNumber == 3 && TutorialManager.Instance.step2Done)
//        {
//            TutorialManager.Instance.TriggerStep(3);
//        }
//    }

//    private void OnTriggerExit(Collider other)
//    {
//        if (!other.CompareTag("Player")) return;

//        // 🔒 Tutorial completed → exit logic bhi skip
//        if (TutorialManager.Instance == null) return;
//        if (TutorialManager.Instance.IsTutorialCompleted())
//            return;

//        TutorialManager.Instance.ExitStep(stepNumber);
//    }
//}



using UnityEngine;

public class StepTrigger : MonoBehaviour
{
    [Tooltip("Use 1 or 2 only")]
    public int stepNumber; // 1 or 2

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (TutorialManager.Instance == null) return;

        // 🔒 Tutorial already finished → do nothing
        if (TutorialManager.Instance.IsTutorialCompleted())
            return;

        // ✅ Let TutorialManager decide if step should show
        TutorialManager.Instance.TriggerStep(stepNumber);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (TutorialManager.Instance == null) return;

        if (TutorialManager.Instance.IsTutorialCompleted())
            return;

        TutorialManager.Instance.ExitStep(stepNumber);
    }
}
