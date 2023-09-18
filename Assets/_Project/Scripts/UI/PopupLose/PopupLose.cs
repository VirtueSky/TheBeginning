using System.Reflection;
using UnityEngine;
using VirtueSky.Events;

public class PopupLose : UIPopup
{
    [SerializeField] private EventNoParam replayGameEvent;

    protected override void OnBeforeShow()
    {
        base.OnBeforeShow();
    }

    protected override void OnBeforeHide()
    {
        base.OnBeforeHide();
    }

    public void OnClickReplay()
    {
        MethodBase function = MethodBase.GetCurrentMethod();
        Observer.TrackClickButton?.Invoke(function.Name);
        Hide();
        replayGameEvent.Raise();
    }
}