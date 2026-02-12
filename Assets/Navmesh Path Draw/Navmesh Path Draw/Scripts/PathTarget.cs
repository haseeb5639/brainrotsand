using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTarget : MonoBehaviour
{
    private void OnEnable()
    {
        ShowGoldenPath.goldenPath.SetPathTarget(this.gameObject);


    }
}


