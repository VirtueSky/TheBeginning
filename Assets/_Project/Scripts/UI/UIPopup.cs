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

    [ShowIf(nameof(ConditionShowMove))] private MovePopupType showMovePopup;

    [ShowIf(nameof(ConditionShowOutBack))] [SerializeField]
    private Vector3 scaleFromShow = new Vector3(.5f, .5f, .5f);

    [TitleColor("Hide Animation", CustomColor.Burlywood, CustomColor.Gold)] [ShowIf(nameof(useAnimation))]
    public bool useHideAnimation;

    [ShowIf(nameof(useHideAnimation))] public HideAnimationType hideAnimationType;
    [ShowIf(nameof(useHideAnimation))] public float durationHidePopup;

    [ShowIf(nameof(ConditionHideMove))] private MovePopupType hideMovePopup;

    [ShowIf(nameof(ConditionHideInBack))] [SerializeField]
    private Vector3 scaleFromHide = new Vector3(0, 0, 0);

    private Tween tween;
    private bool ConditionShowMove => useAnimation && useShowAnimation && showAnimationType == ShowAnimationType.Move;
    private bool ConditionHideMove => useAnimation && useHideAnimation && hideAnimationType == HideAnimationType.Move;

    private bool ConditionShowOutBack =>
        useAnimation && useShowAnimation && showAnimationType == ShowAnimationType.OutBack;

    private bool ConditionHideInBack =>
        useAnimation && useHideAnimation && hideAnimationType == HideAnimationType.InBack;

    public virtual void Show()
    {
        OnBeforeShow();
        gameObject.SetActive(true);
        if (useShowAnimation)
        {
            switch (showAnimationType)
            {
                case ShowAnimationType.OutBack:
                    Vector3 currentScale = transform.localScale;
                    transform.localScale = scaleFromShow;
                    tween = transform.Scale(currentScale, durationShowPopup, Ease.OutBack)
                        .OnComplete(() => { OnAfterShow(); });
                    break;
                case ShowAnimationType.Flip:
                    break;
                case ShowAnimationType.Move:
                    break;
            }
        }
        else
        {
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
                    tween = Tween.Alpha(canvasGroup, 0, durationHidePopup).OnComplete(() =>
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

                    break;
            }
        }
        else
        {
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
    Move
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