using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas), typeof(GraphicRaycaster), typeof(CanvasGroup))]
public class UIPopup : MonoBehaviour
{
    [FoldoutGroup(Constant.Environment)] public CanvasGroup canvasGroup;
    [FoldoutGroup(Constant.Environment)] public Canvas canvas;
    public bool UseAnimation;

    [FoldoutGroup(Constant.UI_Motion)] [ShowIf("UseAnimation")]
    public GameObject Background;

    [FoldoutGroup(Constant.UI_Motion)] [ShowIf("UseAnimation")]
    public GameObject Container;

    [FoldoutGroup(Constant.UI_Motion)] [ShowIf("UseAnimation")]
    public bool UseShowAnimation;

    [FoldoutGroup(Constant.UI_Motion)] [ShowIf("UseShowAnimation")]
    public ShowAnimationType ShowAnimationType;

    [FoldoutGroup(Constant.UI_Motion)] [ShowIf("UseAnimation")]
    public bool UseHideAnimation;

    [FoldoutGroup(Constant.UI_Motion)] [ShowIf("UseHideAnimation")]
    public HideAnimationType HideAnimationType;

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
                        .Append(Container.transform.DOScale(Vector3.one, Config.Game.DurationPopup)
                            .SetEase(Ease.OutBack).OnComplete(() => { OnAfterShow(); }));
                    break;
                case ShowAnimationType.Flip:
                    DOTween.Sequence().OnStart(() => Container.transform.localEulerAngles = new Vector3(0, 180, 0))
                        .Append(Container.transform.DORotate(Vector3.zero, Config.Game.DurationPopup))
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
        if (UseHideAnimation)
        {
            switch (HideAnimationType)
            {
                case HideAnimationType.InBack:
                    DOTween.Sequence().Append(Container.transform
                        .DOScale(Vector3.one * .7f, Config.Game.DurationPopup).SetEase(Ease.InBack)
                        .OnComplete(() =>
                        {
                            gameObject.SetActive(false);
                            OnAfterShow();
                        }));
                    break;
                case HideAnimationType.Fade:
                    canvasGroup.DOFade(0, Config.Game.DurationPopup).OnComplete(() =>
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