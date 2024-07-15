using PrimeTween;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VirtueSky.Inspector;
using VirtueSky.Variables;

namespace TheBeginning.UI
{
    [RequireComponent(typeof(Canvas), typeof(GraphicRaycaster), typeof(CanvasGroup))]
    public class UIPopup : MonoBehaviour
    {
        [TitleColor(Constant.Environment, CustomColor.Violet, CustomColor.Lime)]
        public CanvasGroup canvasGroup;

        public Canvas canvas;
        public bool useAnimation;

        [ShowIf(nameof(useAnimation))] [SerializeField]
        protected GameObject background;

        [ShowIf(nameof(useAnimation))] [SerializeField]
        protected GameObject container;

        [TitleColor("Show Animation", CustomColor.Aqua, CustomColor.Beige)] [ShowIf(nameof(useAnimation))]
        public bool useShowAnimation;

        [ShowIf(nameof(useShowAnimation))] public ShowAnimationType showAnimationType;
        [ShowIf(nameof(useShowAnimation))] public float durationShowPopup = .5f;

        [ShowIf(nameof(ConditionShowMove))] [SerializeField]
        private MovePopupType showMovePopup;

        [ShowIf(nameof(ConditionShowMove))] [SerializeField]
        private float offsetShowMove;

        [ShowIf(nameof(ConditionShowOutBack))] [SerializeField]
        private Vector3 scaleFromShow = new Vector3(.5f, .5f, .5f);

        [ShowIf(nameof(ConditionShowFlip))] [SerializeField]
        private Vector3 eulerAngleShowFrom = new Vector3(0, 180, 0);

        [ShowIf(nameof(showAnimationType), ShowAnimationType.OutBackFromPoint)] [SerializeField]
        private Vector3Variable pointShowPos;

        [TitleColor("Hide Animation", CustomColor.Burlywood, CustomColor.Gold)] [ShowIf(nameof(useAnimation))]
        public bool useHideAnimation;

        [ShowIf(nameof(useHideAnimation))] public HideAnimationType hideAnimationType;
        [ShowIf(nameof(useHideAnimation))] public float durationHidePopup = .5f;

        [ShowIf(nameof(ConditionHideMove))] [SerializeField]
        private MovePopupType hideMovePopup;

        [ShowIf(nameof(ConditionHideMove))] [SerializeField]
        private float offsetHideMove;

        [ShowIf(nameof(ConditionHideInBack))] [SerializeField]
        private Vector3 scaleFromHide = new Vector3(0, 0, 0);

        [ShowIf(nameof(hideAnimationType), HideAnimationType.InBackToPoint)] [SerializeField]
        private Vector3Variable pointHidePos;

        private Tween tween;

        private bool ConditionShowMove =>
            useAnimation && useShowAnimation && showAnimationType == ShowAnimationType.Move;

        private bool ConditionHideMove =>
            useAnimation && useHideAnimation && hideAnimationType == HideAnimationType.Move;

        private bool ConditionShowOutBack =>
            useAnimation && useShowAnimation && showAnimationType == ShowAnimationType.OutBack;

        private bool ConditionHideInBack =>
            useAnimation && useHideAnimation && hideAnimationType == HideAnimationType.InBack;

        private bool ConditionShowFlip =>
            useAnimation && useShowAnimation && showAnimationType == ShowAnimationType.Flip;

        public virtual void Show()
        {
            OnBeforeShow();
            if (useShowAnimation)
            {
                Vector3 currentPos = container.transform.position;
                Vector3 currentScale = container.transform.localScale;
                Vector3 currentAngle = container.transform.localEulerAngles;
                switch (showAnimationType)
                {
                    case ShowAnimationType.OutBack:
                        container.transform.localScale = scaleFromShow;
                        gameObject.SetActive(true);
                        tween = container.transform.Scale(currentScale, durationShowPopup, Ease.OutBack)
                            .OnComplete(() => { OnAfterShow(); });
                        break;
                    case ShowAnimationType.Flip:
                        container.transform.eulerAngles = eulerAngleShowFrom;
                        gameObject.SetActive(true);
                        tween = container.transform.EulerAngles(eulerAngleShowFrom, currentAngle, durationShowPopup)
                            .SetEase(Ease.OutBack).OnComplete(() => { OnAfterShow(); });
                        break;
                    case ShowAnimationType.Fade:
                        canvasGroup.alpha = 0;
                        gameObject.SetActive(true);
                        tween = Tween.Alpha(canvasGroup, 1, durationShowPopup, Ease.OutBack).OnComplete(() =>
                        {
                            canvasGroup.alpha = 1;
                            OnAfterShow();
                        });
                        break;
                    case ShowAnimationType.Move:
                        switch (showMovePopup)
                        {
                            case MovePopupType.Left:
                                container.transform.position = new Vector3(
                                    container.transform.position.x - offsetShowMove,
                                    container.transform.position.y,
                                    container.transform.position.z);
                                gameObject.SetActive(true);
                                tween = container.transform.Position(currentPos, durationShowPopup, Ease.Linear)
                                    .OnComplete(() => { OnAfterShow(); });
                                break;
                            case MovePopupType.Right:
                                container.transform.position = new Vector3(
                                    container.transform.position.x + offsetShowMove,
                                    container.transform.position.y,
                                    container.transform.position.z);
                                gameObject.SetActive(true);
                                tween = container.transform.Position(currentPos, durationShowPopup, Ease.Linear)
                                    .OnComplete(() => { OnAfterShow(); });
                                break;
                            case MovePopupType.Up:
                                container.transform.position = new Vector3(container.transform.position.x,
                                    container.transform.position.y + offsetShowMove,
                                    container.transform.position.z);
                                gameObject.SetActive(true);
                                tween = container.transform.Position(currentPos, durationShowPopup, Ease.Linear)
                                    .OnComplete(() => { OnAfterShow(); });
                                break;
                            case MovePopupType.Down:
                                container.transform.position = new Vector3(container.transform.position.x,
                                    container.transform.position.y - offsetShowMove,
                                    container.transform.position.z);
                                gameObject.SetActive(true);
                                tween = container.transform.Position(currentPos, durationShowPopup, Ease.Linear)
                                    .OnComplete(() => { OnAfterShow(); });
                                break;
                        }

                        break;
                    case ShowAnimationType.OutBackFromPoint:
                        container.transform.position = pointShowPos.Value;
                        container.transform.localScale = Vector3.zero;
                        gameObject.SetActive(true);
                        container.transform.Position(currentPos, durationShowPopup, Ease.OutSine);
                        container.transform.Scale(currentScale, durationShowPopup, Ease.OutSine)
                            .OnComplete(() => { OnAfterShow(); });
                        break;
                }
            }
            else
            {
                gameObject.SetActive(true);
                OnAfterShow();
            }
        }

        public virtual void Hide()
        {
            OnBeforeHide();
            if (useHideAnimation)
            {
                Vector3 currentPos = container.transform.position;
                Vector3 currentScale = container.transform.localScale;
                switch (hideAnimationType)
                {
                    case HideAnimationType.Fade:
                        tween = Tween.Alpha(canvasGroup, 0, durationHidePopup, Ease.InBack).OnComplete(() =>
                        {
                            gameObject.SetActive(false);
                            canvasGroup.alpha = 1;
                            OnAfterHide();
                        });
                        break;
                    case HideAnimationType.InBack:

                        tween = container.transform.Scale(scaleFromHide, durationHidePopup, Ease.InBack).OnComplete(
                            () =>
                            {
                                gameObject.SetActive(false);
                                container.transform.localScale = currentScale;
                                OnAfterHide();
                            });
                        break;
                    case HideAnimationType.Move:
                        switch (hideMovePopup)
                        {
                            case MovePopupType.Left:
                                Vector3 targetPosL = new Vector3(container.transform.position.x - offsetHideMove,
                                    container.transform.position.y,
                                    container.transform.position.z);
                                tween = container.transform.Position(targetPosL, durationHidePopup, Ease.Linear)
                                    .OnComplete(
                                        () =>
                                        {
                                            gameObject.SetActive(false);
                                            container.transform.position = currentPos;
                                            OnAfterHide();
                                        });
                                break;
                            case MovePopupType.Right:
                                Vector3 targetPosR = new Vector3(container.transform.position.x + offsetHideMove,
                                    container.transform.position.y,
                                    container.transform.position.z);
                                tween = container.transform.Position(targetPosR, durationHidePopup, Ease.Linear)
                                    .OnComplete(
                                        () =>
                                        {
                                            gameObject.SetActive(false);
                                            container.transform.position = currentPos;
                                            OnAfterHide();
                                        });
                                break;
                            case MovePopupType.Up:
                                Vector3 targetPosU = new Vector3(container.transform.position.x,
                                    container.transform.position.y + offsetHideMove,
                                    container.transform.position.z);
                                tween = container.transform.Position(targetPosU, durationHidePopup, Ease.Linear)
                                    .OnComplete(
                                        () =>
                                        {
                                            gameObject.SetActive(false);
                                            container.transform.position = currentPos;
                                            OnAfterHide();
                                        });
                                break;
                            case MovePopupType.Down:
                                Vector3 targetPosD = new Vector3(container.transform.position.x,
                                    container.transform.position.y - offsetHideMove,
                                    container.transform.position.z);
                                tween = container.transform.Position(targetPosD, durationHidePopup, Ease.Linear)
                                    .OnComplete(
                                        () =>
                                        {
                                            gameObject.SetActive(false);
                                            container.transform.position = currentPos;
                                            OnAfterHide();
                                        });
                                break;
                        }

                        break;
                    case HideAnimationType.InBackToPoint:
                        container.transform.Position(pointHidePos.Value, durationHidePopup, Ease.InSine);
                        container.transform.Scale(Vector3.zero, durationHidePopup, Ease.InSine).OnComplete(() =>
                        {
                            gameObject.SetActive(false);
                            container.transform.position = currentPos;
                            container.transform.localScale = currentScale;
                            OnAfterHide();
                        });
                        break;
                }
            }
            else
            {
                gameObject.SetActive(false);
                OnAfterHide();
            }
        }

        protected virtual void OnBeforeShow()
        {
        }

        protected virtual void OnAfterShow()
        {
            tween.Stop();
        }

        protected virtual void OnBeforeHide()
        {
        }

        protected virtual void OnAfterHide()
        {
            tween.Stop();
        }

#if UNITY_EDITOR
        private void Reset()
        {
            if (canvasGroup == null)
            {
                canvasGroup = GetComponent<CanvasGroup>();
            }

            if (canvas == null)
            {
                canvas = GetComponent<Canvas>();
            }
        }
#endif
    }

    public enum ShowAnimationType
    {
        OutBack,
        Flip,
        Move,
        Fade,
        OutBackFromPoint,
    }

    public enum HideAnimationType
    {
        InBack,
        Fade,
        Move,
        InBackToPoint,
    }

    public enum MovePopupType
    {
        Left,
        Right,
        Up,
        Down
    }
}