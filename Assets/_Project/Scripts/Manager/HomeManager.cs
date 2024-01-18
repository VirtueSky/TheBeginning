using UnityEngine;
using VirtueSky.Events;
using VirtueSky.Notifications;

public class HomeManager : MonoBehaviour
{
    [SerializeField] private StringEvent changeSceneEvent;
    [SerializeField] private NotificationVariable notificationVariable;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        notificationVariable.Schedule();
    }

    public void LoadGameScene()
    {
        changeSceneEvent.Raise(Constant.GAME_SCENE);
    }
}