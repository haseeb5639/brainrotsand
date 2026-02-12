////using System.Collections;
////using UnityEngine;
////using UnityEngine.AI;

////public class ShowGoldenPath : MonoBehaviour
////{
////    public Transform target;
////    private NavMeshPath path;
////    public static ShowGoldenPath goldenPath;



////    [SerializeField]
////    private LineRenderer Path;
////    [SerializeField]
////    private float PathHeightOffset = 1.25f;

////    public bool IsPathOn;


////    [SerializeField] float scrollSpeed = 1f;
////    Renderer rend;

////    private void Awake()
////    {
////        goldenPath = this;
////    }
////    void Start()
////    {

////        IsPathOn = true;
////        Path = GetComponent<LineRenderer>();
////        path = new NavMeshPath();
////        rend = GetComponent<Renderer>();
////    }

////    public void SetPathTarget(GameObject other)
////    {
////        target = other.transform;
////    }

////    void Update()
////    {
////        if (IsPathOn && target != null)
////        {

////            if (NavMesh.CalculatePath(transform.position, target.transform.position, NavMesh.AllAreas, path))
////            {
////                Path.positionCount = path.corners.Length;

////                for (int i = 0; i < path.corners.Length; i++)
////                {
////                    Path.SetPosition(i, path.corners[i] + Vector3.up * PathHeightOffset);
////                }
////            }
////            else
////            {
////                // Debug.LogError($"Unable to calculate a path on the NavMesh between {transform.position} and {target.transform.position}!");
////            }

////            float offset = Time.time * scrollSpeed;

////            Path.material.SetTextureOffset("_MainTex", new Vector2(-offset, 0));
////        }

////    }
////}



//using System;
//using System.Collections;
//using UnityEngine;
//using UnityEngine.AI;

//public class ShowGoldenPath : MonoBehaviour
//{
//    public Transform target;
//    private NavMeshPath path;
//    public static ShowGoldenPath goldenPath;

//    [SerializeField] private LineRenderer Path;
//    [SerializeField] private float PathHeightOffset = 1.25f;

//    public bool IsPathOn = false;   // default OFF

//    [SerializeField] float scrollSpeed = 1f;
//    Renderer rend;

//    private void Awake()
//    {
//        goldenPath = this;
//    }

//    void Start()
//    {
//        Path = GetComponent<LineRenderer>();
//        path = new NavMeshPath();
//        rend = GetComponent<Renderer>();
//    }

//    public void SetPathTarget(GameObject other)
//    {
//        if (other == null)
//        {
//            target = null;
//            Path.positionCount = 0; // CLEAR PATH
//            return;
//        }

//        target = other.transform;
//    }

//    void Update()
//    {
//        if (!IsPathOn || target == null)
//        {
//            Path.positionCount = 0; // stop drawing
//            return;
//        }

//        if (NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path))
//        {
//            Path.positionCount = path.corners.Length;

//            for (int i = 0; i < path.corners.Length; i++)
//                Path.SetPosition(i, path.corners[i] + Vector3.up * PathHeightOffset);
//        }

//        float offset = Time.time * scrollSpeed;
//        Path.material.SetTextureOffset("_MainTex", new Vector2(-offset, 0));
//    }

//    internal void SetPathTarget(object baseTargetGameObject)
//    {
//        throw new NotImplementedException();
//    }



//}


using UnityEngine;
using UnityEngine.AI;

public class ShowGoldenPath : MonoBehaviour
{
    public static ShowGoldenPath goldenPath;

    public Transform target;
    private NavMeshPath path;

    [SerializeField] private LineRenderer Path;
    [SerializeField] private float PathHeightOffset = 1.25f;

    public bool IsPathOn = false;

    [SerializeField] private float scrollSpeed = 1f;
    Renderer rend;

    private void Awake()
    {
        goldenPath = this;
    }

    void Start()
    {
        Path = GetComponent<LineRenderer>();
        path = new NavMeshPath();
        rend = GetComponent<Renderer>();

        // 💥 If tutorial already done → disable path system completely
        if (PlayerPrefs.GetInt("TutorialDone", 0) == 1)
        {
            IsPathOn = false;
            target = null;
            Path.positionCount = 0;
            return;
        }
    }

    public void SetPathTarget(GameObject other)
    {
        if (other == null)
        {
            target = null;
            Path.positionCount = 0;
            return;
        }

        target = other.transform;
    }

    void Update()
    {
        if (!IsPathOn || target == null)
        {
            Path.positionCount = 0;
            return;
        }

        if (NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path))
        {
            Path.positionCount = path.corners.Length;

            for (int i = 0; i < path.corners.Length; i++)
            {
                Path.SetPosition(i, path.corners[i] + Vector3.up * PathHeightOffset);
            }
        }

        float offset = Time.time * scrollSpeed;
        Path.material.SetTextureOffset("_MainTex", new Vector2(-offset, 0));
    }
}
