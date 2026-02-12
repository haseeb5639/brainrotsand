using UnityEngine;

public class AppNotification : MonoBehaviour
{
    public AppNotifications messeges;
    string messege;
    int index;
    public int NotificationTime = 1;


    private void OnApplicationFocus(bool focus)
    {
        if (focus == false)
        {

            if (messeges.Notifications.Length <= 0)
            {
                return;
            }
            index = Random.Range(0, messeges.Notifications.Length);
            messege = messeges.Notifications[index];
            GleyNotifications.SendNotification(Application.productName, messege, new System.TimeSpan(0, NotificationTime, 0));
        }
        else
        {
            //call initialize when user returns to your app to cancel all pending notifications
            GleyNotifications.Initialize();
        }
    }
}
