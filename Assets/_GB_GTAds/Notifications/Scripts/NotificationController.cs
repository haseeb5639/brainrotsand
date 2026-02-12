using System.Collections;
using UnityEngine;
#if UNITY_IOS
using Unity.Notifications.iOS;
#endif
public class NotificationController : MonoBehaviour
{

    public GameObject NotificationsPrefab;
    public float DelayInRequest=4f;
    void Start()
    {
#if UNITY_IOS
        StartCoroutine( RequestAuthorization());
#endif
#if UNITY_ANDROID
        Init();
#endif
    }

#if UNITY_IOS
    IEnumerator RequestAuthorization()
    {
        yield return new WaitForSeconds(DelayInRequest);

        var authorizationOption = AuthorizationOption.Alert | AuthorizationOption.Badge | AuthorizationOption.Sound;
        using (var req = new AuthorizationRequest(authorizationOption, true))
        {
            while (!req.IsFinished)
            {
                yield return null;
            };

           
        }

        Init();

    }
#endif

    void Init()
    {
        Instantiate(NotificationsPrefab,this.transform);
        GleyNotifications.Initialize();

    }
}
