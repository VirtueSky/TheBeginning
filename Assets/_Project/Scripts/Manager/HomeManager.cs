using UnityEngine;
using VirtueSky.Notifications;

public class HomeManager : MonoBehaviour
{
    [SerializeField] private LoadSceneEvent loadSceneEvent;
    [SerializeField] private NotificationVariable notificationVariable;

    private void Start()
    {
        notificationVariable.Schedule();
    }

    public void LoadGameScene()
    {
        loadSceneEvent.Raise(new LoadSceneData(false, Constant.GAMEPLAY_SCENE, .1f, null));
    }
}