using PrimeTween;
using TMPro;
using UnityEngine;
using VirtueSky.Events;
using VirtueSky.Variables;

public class CoinUpdater : MonoBehaviour
{
    public TextMeshProUGUI CurrencyAmountText;
    [SerializeField] private GameObject iconCoin;
    [SerializeField] IntegerVariable currentCoin;

    [SerializeField] private EventNoParam moveOneCoinDone;
    [SerializeField] private EventNoParam moveAllCoinDone;
    [SerializeField] private EventNoParam decreaseCoinEvent;
    [SerializeField] private GameObjectEvent addTargetToCoinGenerateEvent;
    [SerializeField] private GameObjectEvent removeTargetToCoinGenerateEvent;

    bool isFirsCoinMoveDone = false;

    private void OnEnable()
    {
        moveOneCoinDone.AddListener(MoveOneCoinDone);
        decreaseCoinEvent.AddListener(DecreaseCoin);
        moveAllCoinDone.AddListener(MoveAllCoinDone);
        addTargetToCoinGenerateEvent.Raise(iconCoin);
        CurrencyAmountText.text = currentCoin.Value.ToString();
    }

    private void OnDisable()
    {
        moveOneCoinDone.RemoveListener(MoveOneCoinDone);
        moveAllCoinDone.RemoveListener(MoveAllCoinDone);
        decreaseCoinEvent.RemoveListener(DecreaseCoin);
        removeTargetToCoinGenerateEvent.Raise(iconCoin);
    }

    void MoveOneCoinDone()
    {
        if (!isFirsCoinMoveDone)
        {
            isFirsCoinMoveDone = true;
            UpdateTextCoin();
        }
    }

    void MoveAllCoinDone()
    {
        isFirsCoinMoveDone = false;
    }

    private void DecreaseCoin()
    {
        UpdateTextCoin();
    }

    void UpdateTextCoin()
    {
        int starCoin = int.Parse(CurrencyAmountText.text);
        int coinChange = starCoin;
        Tween.Custom(starCoin, currentCoin.Value, 0.5f, valueChange => coinChange = (int)valueChange)
            .OnUpdate(this, (coin, tween) => { CurrencyAmountText.text = coinChange.ToString(); });
    }
}