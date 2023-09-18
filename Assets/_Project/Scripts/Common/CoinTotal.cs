using TMPro;
using UnityEngine;

public class CoinTotal : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currencyTotal;

    private void Awake()
    {
        //  Observer.CurrencyTotalChanged += UpdateCurrencyText;
        UpdateCurrencyText();
    }

    private void UpdateCurrencyText()
    {
        //  currencyTotal.text = Data.CurrencyTotal.ToString();
    }
}