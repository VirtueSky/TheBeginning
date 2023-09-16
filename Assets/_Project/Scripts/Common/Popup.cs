using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class Popup : MonoBehaviour
{
    public bool UseAnimation;
    [ShowIf("UseAnimation")] public GameObject Background;
    [ShowIf("UseAnimation")] public GameObject Container;
    [ShowIf("UseAnimation")] public bool UseShowAnimation;
    [ShowIf("UseShowAnimation")] public ShowAnimationType ShowAnimationType;
    [ShowIf("UseAnimation")] public bool UseHideAnimation;
    [ShowIf("UseHideAnimation")] public HideAnimationType HideAnimationType;
    public CanvasGroup CanvasGroup => GetComponent<CanvasGroup>();
    public Canvas Canvas => GetComponent<Canvas>();

    public virtual void Show()
    {
        OnBeforeShow();
        gameObject.SetActive(true);
        if (UseShowAnimation)
        {
            switch (ShowAnimationType)
            {
                case ShowAnimationType.OutBack:
                    DOTween.Sequence().OnStart(() => Container.transform.localScale = Vector3.one * .9f)
                        .Append(Container.transform.DOScale(Vector3.one, ConfigController.Game.DurationPopup).SetEase(Ease.OutBack).OnComplete(() => { OnAfterShow(); }));
                    break;
                case ShowAnimationType.Flip:
                    DOTween.Sequence().OnStart(() => Container.transform.localEulerAngles = new Vector3(0, 180, 0))
                        .Append(Container.transform.DORotate(Vector3.zero, ConfigController.Game.DurationPopup)).SetEase(Ease.Linear).OnComplete(() => { OnAfterShow(); });
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
        if (UseHideAnimation)
        {
            switch (HideAnimationType)
            {
                case HideAnimationType.InBack:
                    DOTween.Sequence().Append(Container.transform.DOScale(Vector3.one * .7f, ConfigController.Game.DurationPopup).SetEase(Ease.InBack).OnComplete(() =>
                    {
                        gameObject.SetActive(false);
                        OnAfterShow();
                    }));
                    break;
                case HideAnimationType.Fade:
                    CanvasGroup.DOFade(0, ConfigController.Game.DurationPopup).OnComplete(() =>
                    {
                        CanvasGroup.alpha = 1;
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