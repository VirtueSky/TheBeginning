using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using VirtueSky.Audio;
using VirtueSky.Variables;

public class CoinUpdater : MonoBehaviour
{
    public TextMeshProUGUI CurrencyAmountText;
    public int StepCount = 10;
    public float DelayTime = .01f;
    public CoinGenerate coinGenerate;
    [SerializeField] IntegerVariable currentCoin;

    [Header("Sound")] [SerializeField] public PlaySfxEvent playSoundFx;
    [SerializeField] private SoundData soundCoinMove;


    private int _currentCoin;

    private void OnEnable()
    {
        currentCoin.AddListener(UpdateCoinAmountText);
        CurrencyAmountText.text = currentCoin.Value.ToString();
        SaveCurrentCoin();
    }

    private void OnDisable()
    {
        currentCoin.RemoveListener(UpdateCoinAmountText);
    }

    private void SaveCurrentCoin()
    {
        _currentCoin = currentCoin.Value;
    }

    public void UpdateCoinAmountText(int value)
    {
        if (currentCoin.Value > _currentCoin)
        {
            IncreaseCoin();
        }
        else
        {
            DecreaseCoin();
        }
    }

    private void IncreaseCoin()
    {
        bool isFirstMove = false;
        coinGenerate.GenerateCoin(() =>
        {
            if (!isFirstMove)
            {
                isFirstMove = true;
                playSoundFx.Raise(soundCoinMove);
                int currentCurrencyAmount = int.Parse(CurrencyAmountText.text);
                int nextAmount = (currentCoin.Value - currentCurrencyAmount) / StepCount;
                int step = StepCount;
                CoinTextCount(currentCurrencyAmount, nextAmount, step);
            }
        }, () => { SaveCurrentCoin(); });
    }

    private void DecreaseCoin()
    {
        int currentCurrencyAmount = int.Parse(CurrencyAmountText.text);
        int nextAmount = (currentCoin.Value - currentCurrencyAmount) / StepCount;
        int step = StepCount;
        CoinTextCount(currentCurrencyAmount, nextAmount, step);
        SaveCurrentCoin();
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