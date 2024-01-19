using UnityEngine;
using VirtueSky.Notifications;

public class HomeManager : MonoBehaviour
{
    [SerializeField] private NotificationVariable notificationVariable;

    private void Awake()
    {
    }

    private void Start()
    {
        notificationVariable.Schedule();
    }

    public void LoadGameScene()
    {
        // changeSceneEvent.Raise(Constant.GAME_SCENE);
    }
}