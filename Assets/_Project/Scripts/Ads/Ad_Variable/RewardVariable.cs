using System;
using UnityEngine;
using VirtueSky.Ads;
using VirtueSky.Core;
using VirtueSky.Events;
using VirtueSky.FirebaseTracking;
using VirtueSky.Inspector;
using VirtueSky.Variables;

[CreateAssetMenu(menuName = "Ads Variable/Reward Variable", fileName = "reward_ad_variable")]
public class RewardVariable : BaseSO
{
    [SerializeField] private AdUnitVariable rewardVariable;

    [Space, SerializeField] private BooleanVariable isOffRewardVariable;
    [SerializeField] private StringEvent showNotificationInGameEvent;

    [Space, HeaderLine("Log Event Firebase Analytic"), SerializeField]
    private LogEventFirebaseNoParam logEventRequestReward;

    [SerializeField] private LogEventFirebaseNoParam logEventShowRewardCompleted;
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
            logEventRequestReward.LogEvent();
            rewardVariable.Show().OnCompleted(() =>
                {
                    DelayHandle(() =>
                    {
                        completeCallback?.Invoke();
                        logEventShowRewardCompleted.LogEvent();
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