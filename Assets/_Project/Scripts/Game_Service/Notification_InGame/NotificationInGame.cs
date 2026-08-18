using TheBeginning.Config;
using TMPro;
using UnityEngine;
using VirtueSky.Core;
using VirtueSky.Events;
using VirtueSky.Tweening;

namespace TheBeginning.Services
{
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
            if (gameConfig.EnableNotificationInGame)
            {
                showNotificationInGameEvent.AddListener(Show);
            }
        }

        private void OnDestroy()
        {
            if (gameConfig.EnableNotificationInGame)
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
            Tween.Create(posYHide, posYShow, timeMove).WithEase(Ease.OutBack).WithOnComplete(() =>
            {
                App.Delay(gameConfig.TimeDelayHideNotificationInGame, Hide);
            }).BindToAnchoredPositionY(container);
        }

        public void Hide()
        {
            if (!isShow) return;
            Tween.Create(posYShow, posYHide, timeMove).WithEase(Ease.InBack).WithOnComplete(() =>
            {
                isShow = false;
                gameObject.SetActive(false);
            }).BindToAnchoredPositionY(container);
        }
    }
}