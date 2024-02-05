using PrimeTween;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VirtueSky.Inspector;

[RequireComponent(typeof(Canvas), typeof(GraphicRaycaster), typeof(CanvasGroup))]
public class UIPopup : MonoBehaviour
{
    [HeaderLine(Constant.Environment)] public CanvasGroup canvasGroup;
    public Canvas canvas;
    public bool useAnimation;

    [HeaderLine(Constant.UI_Motion)] [ShowIf(nameof(useAnimation))]
    public GameObject background;

    [ShowIf(nameof(useAnimation))] public GameObject container;

    [TitleColor("Show Animation", CustomColor.Aqua, CustomColor.Beige)] [ShowIf(nameof(useAnimation))]
    public bool useShowAnimation;

    [ShowIf(nameof(useShowAnimation))] public ShowAnimationType showAnimationType;
    [ShowIf(nameof(useShowAnimation))] public float durationShowPopup;

    [ShowIf(nameof(ConditionShowMove))] [SerializeField]
    private MovePopupType showMovePopup;

    [ShowIf(nameof(ConditionShowMove))] [SerializeField]
    private float offsetShowMove;

    [ShowIf(nameof(ConditionShowOutBack))] [SerializeField]
    private Vector3 scaleFromShow = new Vector3(.5f, .5f, .5f);

    [ShowIf(nameof(ConditionShowFlip))] [SerializeField]
    private Vector3 eulerAngleShowFrom = new Vector3(0, 180, 0);

    [TitleColor("Hide Animation", CustomColor.Burlywood, CustomColor.Gold)] [ShowIf(nameof(useAnimation))]
    public bool useHideAnimation;

    [ShowIf(nameof(useHideAnimation))] public HideAnimationType hideAnimationType;
    [ShowIf(nameof(useHideAnimation))] public float durationHidePopup;

    [ShowIf(nameof(ConditionHideMove))] [SerializeField]
    private MovePopupType hideMovePopup;

    [ShowIf(nameof(ConditionHideMove))] [SerializeField]
    private float offsetHideMove;

    [ShowIf(nameof(ConditionHideInBack))] [SerializeField]
    private Vector3 scaleFromHide = new Vector3(0, 0, 0);

    private Tween tween;
    private bool ConditionShowMove => useAnimation && useShowAnimation && showAnimationType == ShowAnimationType.Move;
    private bool ConditionHideMove => useAnimation && useHideAnimation && hideAnimationType == HideAnimationType.Move;

    private bool ConditionShowOutBack =>
        useAnimation && useShowAnimation && showAnimationType == ShowAnimationType.OutBack;

    private bool ConditionHideInBack =>
        useAnimation && useHideAnimation && hideAnimationType == HideAnimationType.InBack;

    private bool ConditionShowFlip => useAnimation && useShowAnimation && showAnimationType == ShowAnimationType.Flip;

    public virtual void Show()
    {
        OnBeforeShow();
        if (useShowAnimation)
        {
            switch (showAnimationType)
            {
                case ShowAnimationType.OutBack:
                    Vector3 currentScale = transform.localScale;
                    transform.localScale = scaleFromShow;
                    gameObject.SetActive(true);
                    tween = transform.Scale(currentScale, durationShowPopup, Ease.OutBack)
                        .OnComplete(() => { OnAfterShow(); });
                    break;
                case ShowAnimationType.Flip:
                    Vector3 currentAngle = transform.localEulerAngles;
                    transform.eulerAngles = eulerAngleShowFrom;
                    gameObject.SetActive(true);
                    tween = transform.EulerAngles(eulerAngleShowFrom, currentAngle, durationShowPopup)
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
                    Vector3 currentPos = transform.position;
                    switch (showMovePopup)
                    {
                        case MovePopupType.Left:
                            transform.position = new Vector3(transform.position.x - offsetShowMove,
                                transform.position.y,
                                transform.position.z);
                            gameObject.SetActive(true);
                            tween = transform.Position(currentPos, durationShowPopup, Ease.Linear)
                                .OnComplete(() => { OnAfterShow(); });
                            break;
                        case MovePopupType.Right:
                            transform.position = new Vector3(transform.position.x + offsetShowMove,
                                transform.position.y,
                                transform.position.z);
                            gameObject.SetActive(true);
                            tween = transform.Position(currentPos, durationShowPopup, Ease.Linear)
                                .OnComplete(() => { OnAfterShow(); });
                            break;
                        case MovePopupType.Up:
                            transform.position = new Vector3(transform.position.x,
                                transform.position.y + offsetShowMove,
                                transform.position.z);
                            gameObject.SetActive(true);
                            tween = transform.Position(currentPos, durationShowPopup, Ease.Linear)
                                .OnComplete(() => { OnAfterShow(); });
                            break;
                        case MovePopupType.Down:
                            transform.position = new Vector3(transform.position.x,
                                transform.position.y - offsetShowMove,
                                transform.position.z);
                            gameObject.SetActive(true);
                            tween = transform.Position(currentPos, durationShowPopup, Ease.Linear)
                                .OnComplete(() => { OnAfterShow(); });
                            break;
                    }

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
                    Vector3 currentScale = transform.localScale;
                    tween = transform.Scale(scaleFromHide, durationHidePopup, Ease.InBack).OnComplete(() =>
                    {
                        gameObject.SetActive(false);
                        transform.localScale = currentScale;
                        OnAfterHide();
                    });
                    break;
                case HideAnimationType.Move:
                    Vector3 currentPos = transform.position;
                    switch (hideMovePopup)
                    {
                        case MovePopupType.Left:
                            Vector3 targetPosL = new Vector3(transform.position.x - offsetHideMove,
                                transform.position.y,
                                transform.position.z);
                            tween = transform.Position(targetPosL, durationHidePopup, Ease.Linear).OnComplete(() =>
                            {
                                gameObject.SetActive(false);
                                transform.position = currentPos;
                                OnAfterHide();
                            });
                            break;
                        case MovePopupType.Right:
                            Vector3 targetPosR = new Vector3(transform.position.x + offsetHideMove,
                                transform.position.y,
                                transform.position.z);
                            tween = transform.Position(targetPosR, durationHidePopup, Ease.Linear).OnComplete(() =>
                            {
                                gameObject.SetActive(false);
                                transform.position = currentPos;
                                OnAfterHide();
                            });
                            break;
                        case MovePopupType.Up:
                            Vector3 targetPosU = new Vector3(transform.position.x,
                                transform.position.y + offsetHideMove,
                                transform.position.z);
                            tween = transform.Position(targetPosU, durationHidePopup, Ease.Linear).OnComplete(() =>
                            {
                                gameObject.SetActive(false);
                                transform.position = currentPos;
                                OnAfterHide();
                            });
                            break;
                        case MovePopupType.Down:
                            Vector3 targetPosD = new Vector3(transform.position.x,
                                transform.position.y - offsetHideMove,
                                transform.position.z);
                            tween = transform.Position(targetPosD, durationHidePopup, Ease.Linear).OnComplete(() =>
                            {
                                gameObject.SetActive(false);
                                transform.position = currentPos;
                                OnAfterHide();
                            });
                            break;
                    }

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
    Fade
}

public enum HideAnimationType
{
    InBack,
    Fade,
    Move
}

public enum MovePopupType
{
    Left,
    Right,
    Up,
    Down
}