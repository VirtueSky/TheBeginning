using System;
using UnityEngine;
using VirtueSky.Ads;
using VirtueSky.Core;
using VirtueSky.Events;
using VirtueSky.Tracking;
using VirtueSky.Inspector;
using VirtueSky.Variables;

[CreateAssetMenu(menuName = "Ads Variable/Reward Variable", fileName = "reward_ad_variable")]
public class RewardAdVariable : AdVariable
{
    [SerializeField] private AdUnitVariable rewardVariable;

    [Space, SerializeField] private BooleanVariable isOffRewardVariable;
    [SerializeField] private StringEvent showNotificationInGameEvent;

    [Space, HeaderLine("Log Event Firebase Analytic"), SerializeField]
    private TrackingFirebaseOneParam trackingFirebaseRequestReward;

    [SerializeField] private TrackingFirebaseOneParam trackingFirebaseShowRewardCompleted;
    public AdUnitVariable AdUnitRewardVariable => rewardVariable;

    public override void Init()
    {
    }

    bool Condition()
    {
        return rewardVariable.IsReady() && !isOffRewardVariable.Value;
    }

    public void Show(Action completeCallback = null, Action skipCallback = null, Action displayCallback = null,
        Action closeCallback = null, string trackingRewardPosition = "")
    {
        if (Condition())
        {
            trackingFirebaseRequestReward.TrackEvent(trackingRewardPosition);
            rewardVariable.Show().OnCompleted(() =>
                {
                    DelayHandle(() =>
                    {
                        completeCallback?.Invoke();
                        trackingFirebaseShowRewardCompleted.TrackEvent(trackingRewardPosition);
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