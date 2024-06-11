using PrimeTween;
using TMPro;
using UnityEngine;
using VirtueSky.Core;
using VirtueSky.Events;

public class NotificationInGame : BaseMono
{
    [SerializeField] private TextMeshProUGUI textNoti;
    [SerializeField] private RectTransform container;
    [SerializeField] private GameConfig gameConfig;
    [SerializeField] private float posYShow = -125;
    [SerializeField] private float posYHide = 125;
    [SerializeField] private float timeMove = .5f;
    [SerializeField] private StringEvent showNotificationInGameEvent;
    private bool isShow = false;

    private void Awake()
    {
        if (gameConfig.enableNotificationInGame)
        {
            showNotificationInGameEvent.AddListener(Show);
        }
    }

    private void OnDestroy()
    {
        if (gameConfig.enableNotificationInGame)
        {
            showNotificationInGameEvent.RemoveListener(Show);
        }
    }


    public void Show(string _textNoti)
    {
        if (isShow) return;
        isShow = true;
        gameObject.SetActive(true);
        textNoti.text = _textNoti;
        Tween.UIAnchoredPositionY(container, posYShow, timeMove, Ease.OutBack).OnComplete(() =>
        {
            App.Delay(gameConfig.timeDelayHideNotificationInGame, () => { Hide(); });
        });
    }

    public void Hide()
    {
        if (!isShow) return;
        Tween.UIAnchoredPositionY(container, posYHide, timeMove, Ease.InBack).OnComplete(() =>
        {
            isShow = false;
            gameObject.SetActive(false);
        });
    }
}