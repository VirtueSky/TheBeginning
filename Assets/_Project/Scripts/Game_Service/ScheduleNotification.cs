using System.Collections.Generic;
using UnityEngine;
using VirtueSky.Inspector;
using VirtueSky.Notifications;

[HideMonoScript]
public class ScheduleNotification : MonoBehaviour
{
    [SerializeField] private List<NotificationVariable> listNotificationVariable;

    void Start()
    {
        foreach (var notification in listNotificationVariable)
        {
            notification.Schedule();
        }
    }

    public void SendNotification()
    {
        foreach (var notification in listNotificationVariable)
        {
            notification.Send();
        }
    }
}