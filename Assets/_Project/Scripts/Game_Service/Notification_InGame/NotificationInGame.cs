using PrimeTween;
using TheBeginning.Config;
using TMPro;
using UnityEngine;
using VirtueSky.Core;
using VirtueSky.Events;

namespace TheBeginning.Services
{
    public class NotificationInGame : BaseMono
    {
        [SerializeField] private TextMeshProUGUI textNoti;
        [SerializeField] private RectTransform container;
        [SerializeField] private float posYShow = -125;
        [SerializeField] private float posYHide = 125;
        [SerializeField] private float timeMove = .5f;
        [SerializeField] private StringEvent showNotificationInGameEvent;
        [SerializeField] private GameSettings gameSettings;
        private bool isShow = false;

        private void Awake()
        {
            if (gameSettings.EnableNotificationInGame)
            {
                showNotificationInGameEvent.AddListener(Show);
            }
        }

        private void OnDestroy()
        {
            if (gameSettings.EnableNotificationInGame)
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
            Tween.UIAnchoredPositionY(container, posYShow, timeMove, Ease.OutBack)
                .OnComplete(() => { App.Delay(gameSettings.TimeDelayHideNotificationInGame, () => { Hide(); }); });
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
}