
//using UnityEngine;

//public class WeaponSpinner : MonoBehaviour
//{
//    public float rotateSpeed = 100f;

//    void Update()
//    {
//        transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f);
//    }
//}


using UnityEngine;

public class WeaponSpinner : MonoBehaviour
{
    public float rotateSpeed = 150f;

    void Update()
    {
        Vector3 rot = transform.eulerAngles;
        rot.y += rotateSpeed * Time.deltaTime;
        transform.eulerAngles = rot;
    }
}
