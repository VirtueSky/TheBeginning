using PrimeTween;
using TMPro;
using UnityEngine;
using VirtueSky.Audio;
using VirtueSky.Variables;

public class CoinUpdater : MonoBehaviour
{
    public TextMeshProUGUI CurrencyAmountText;
    public int StepCount = 10;
    public float DelayTime = .01f;
    public CoinGenerate coinGenerate;
    [SerializeField] IntegerVariable currencyTotalVariable;

    [Header("Sound")] [SerializeField] public PlaySfxEvent playSoundFx;
    [SerializeField] private SoundData soundCoinMove;


    private int currentCoin;

    private void OnEnable()
    {
        currencyTotalVariable.AddListener(UpdateCurrencyAmountText);
        CurrencyAmountText.text = currencyTotalVariable.Value.ToString();
        SaveCurrency();
    }

    private void OnDisable()
    {
        currencyTotalVariable.RemoveListener(UpdateCurrencyAmountText);
    }

    private void SaveCurrency()
    {
        currentCoin = currencyTotalVariable.Value;
    }

    public void UpdateCurrencyAmountText(int value)
    {
        if (currencyTotalVariable.Value > currentCoin)
        {
            IncreaseCurrency();
        }
        else
        {
            DecreaseCurrency();
        }
    }

    private void IncreaseCurrency()
    {
        bool isFirstMove = false;
        coinGenerate.GenerateCoin(() =>
        {
            if (!isFirstMove)
            {
                isFirstMove = true;
                playSoundFx.Raise(soundCoinMove);
                int currentCurrencyAmount = int.Parse(CurrencyAmountText.text);
                int nextAmount = (currencyTotalVariable.Value - currentCurrencyAmount) / StepCount;
                int step = StepCount;
                CurrencyTextCount(currentCurrencyAmount, nextAmount, step);
            }
        }, () => { SaveCurrency(); });
    }

    private void DecreaseCurrency()
    {
        int currentCurrencyAmount = int.Parse(CurrencyAmountText.text);
        int nextAmount = (currencyTotalVariable.Value - currentCurrencyAmount) / StepCount;
        int step = StepCount;
        CurrencyTextCount(currentCurrencyAmount, nextAmount, step);
        SaveCurrency();
    }

    private void CurrencyTextCount(int currentCurrencyValue, int nextAmountValue, int stepCount)
    {
        if (stepCount == 0)
        {
            CurrencyAmountText.text = currencyTotalVariable.Value.ToString();
            return;
        }

        int totalValue = (currentCurrencyValue + nextAmountValue);
        DOTween.Sequence().AppendInterval(DelayTime).SetUpdate(isIndependentUpdate: true).AppendCallback(() =>
            {
                CurrencyAmountText.text = totalValue.ToString();
            })
            .AppendCallback(() => { CurrencyTextCount(totalValue, nextAmountValue, stepCount - 1); });
    }
}