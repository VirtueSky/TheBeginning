using System;
using UnityEngine;
using VirtueSky.Ads;
using VirtueSky.Core;
using VirtueSky.Events;
using VirtueSky.Tracking;
using VirtueSky.Inspector;
using VirtueSky.Variables;

[CreateAssetMenu(menuName = "Ads Variable/Reward Variable", fileName = "reward_ad_variable")]
public class RewardVariable : BaseSO
{
    [SerializeField] private AdUnitVariable rewardVariable;

    [Space, SerializeField] private BooleanVariable isOffRewardVariable;
    [SerializeField] private StringEvent showNotificationInGameEvent;

    [Space, HeaderLine("Log Event Firebase Analytic"), SerializeField]
    private TrackingFirebaseNoParam trackingFirebaseRequestReward;

    [SerializeField] private TrackingFirebaseNoParam trackingFirebaseShowRewardCompleted;
    public AdUnitVariable AdUnitRewardVariable => rewardVariable;

    bool Condition()
    {
        return rewardVariable.IsReady() && !isOffRewardVariable.Value;
    }

    public void Show(Action completeCallback = null, Action skipCallback = null, Action displayCallback = null,
        Action closeCallback = null)
    {
        if (Condition())
        {
            trackingFirebaseRequestReward.TrackEvent();
            rewardVariable.Show().OnCompleted(() =>
                {
                    DelayHandle(() =>
                    {
                        completeCallback?.Invoke();
                        trackingFirebaseShowRewardCompleted.TrackEvent();
                    });
                }).OnDisplayed(displayCallback)
                .OnClosed(() => DelayHandle(closeCallback))
                .OnSkipped(() => DelayHandle(skipCallback));
        }
        else
        {
            showNotificationInGameEvent.Raise("Reward ads not ready");
        }
    }


    void DelayHandle(Action action)
    {
        App.Delay(0.05f, action);
    }
}