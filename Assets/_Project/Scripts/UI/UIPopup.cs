using PrimeTween;
using UnityEngine;
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

    [ShowIf(nameof(useAnimation))] public bool useShowAnimation;

    [ShowIf(nameof(useShowAnimation))] public ShowAnimationType showAnimationType;
    [ShowIf(nameof(useShowAnimation))] public float durationShowPopup;
    [ShowIf(nameof(useAnimation))] public bool useHideAnimation;

    [ShowIf(nameof(useHideAnimation))] public HideAnimationType hideAnimationType;
    [ShowIf(nameof(useHideAnimation))] public float durationHidePopup;

    public virtual void Show()
    {
        OnBeforeShow();
        gameObject.SetActive(true);
        if (useShowAnimation)
        {
            switch (showAnimationType)
            {
                case ShowAnimationType.OutBack:
                    DOTween.Sequence().ChainCallback(() =>
                            container.transform.localScale = Vector3.one * .9f)
                        .Append(container.transform.DOScale(Vector3.one, durationShowPopup)
                            .SetEase(Ease.OutBack).OnComplete(() => { OnAfterShow(); }));
                    break;
                case ShowAnimationType.Flip:
                    DOTween.Sequence().ChainCallback(() =>
                            container.transform.localEulerAngles = new Vector3(0, 180, 0))
                        .Append(container.transform.DORotate(Vector3.zero,
                            durationShowPopup))
                        .SetEase(Ease.Linear).OnComplete(() => { OnAfterShow(); });
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
                case HideAnimationType.InBack:
                    DOTween.Sequence().Append(container.transform
                        .DOScale(Vector3.one * .7f, durationHidePopup).SetEase(Ease.InBack)
                        .OnComplete(() =>
                        {
                            gameObject.SetActive(false);
                            OnAfterShow();
                        }));
                    break;
                case HideAnimationType.Fade:
                    canvasGroup.DOFade(0, durationHidePopup).OnComplete(() =>
                    {
                        canvasGroup.alpha = 1;
                        gameObject.SetActive(false);
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

    protected virtual void AfterInstantiate()
    {
    }

    protected virtual void OnBeforeShow()
    {
    }

    protected virtual void OnAfterShow()
    {
    }

    protected virtual void OnBeforeHide()
    {
    }

    protected virtual void OnAfterHide()
    {
    }

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
}

public enum ShowAnimationType
{
    OutBack,
    Flip,
}

public enum HideAnimationType
{
    InBack,
    Fade,
}