using PrimeTween;
using TMPro;
using UnityEngine;
using VirtueSky.Audio;
using VirtueSky.Events;
using VirtueSky.Variables;

public class CoinUpdater : MonoBehaviour
{
    public TextMeshProUGUI CurrencyAmountText;
    public int StepCount = 10;
    public float DelayTime = .01f;
    [SerializeField] private GameObject iconCoin;
    [SerializeField] IntegerVariable currentCoin;

    [SerializeField] private EventNoParam moveOneCoinDone;
    [SerializeField] private EventNoParam moveAllCoinDone;
    [SerializeField] private EventNoParam decreaseCoinEvent;
    [SerializeField] private AddTargetToCoinGenerateEvent addTargetToCoinGenerateEvent;
    [SerializeField] private RemoveTargetToCoinGenerateEvent removeTargetToCoinGenerateEvent;

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
            int currentCurrencyAmount = int.Parse(CurrencyAmountText.text);
            int nextAmount = (currentCoin.Value - currentCurrencyAmount) / StepCount;
            int step = StepCount;
            CoinTextCount(currentCurrencyAmount, nextAmount, step);
        }
    }

    void MoveAllCoinDone()
    {
        isFirsCoinMoveDone = false;
    }

    private void DecreaseCoin()
    {
        int currentCurrencyAmount = int.Parse(CurrencyAmountText.text);
        int nextAmount = (currentCoin.Value - currentCurrencyAmount) / StepCount;
        int step = StepCount;
        CoinTextCount(currentCurrencyAmount, nextAmount, step);
    }

    private void CoinTextCount(int currentCurrencyValue, int nextAmountValue, int stepCount)
    {
        if (stepCount == 0)
        {
            CurrencyAmountText.text = currentCoin.Value.ToString();
            return;
        }

        int totalValue = (currentCurrencyValue + nextAmountValue);
        DOTween.Sequence().AppendInterval(DelayTime).SetUpdate(isIndependentUpdate: true).AppendCallback(() =>
            {
                CurrencyAmountText.text = totalValue.ToString();
            })
            .AppendCallback(() => { CoinTextCount(totalValue, nextAmountValue, stepCount - 1); });
    }
}