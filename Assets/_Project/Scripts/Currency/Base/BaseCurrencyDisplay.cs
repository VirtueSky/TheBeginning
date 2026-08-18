using System.Globalization;
using TMPro;
using UnityEngine;
using VirtueSky.Events;
using VirtueSky.Tweening;

namespace TheBeginning.Currency
{
    public abstract class BaseCurrencyDisplay : MonoBehaviour
    {
        [SerializeField] private CurrencyVariable currency;
        [SerializeField] private TextMeshProUGUI currencyAmountText;
        [SerializeField] private GameObject iconTarget;
        [SerializeField] private GameObjectEvent addTargetEvent;
        [SerializeField] private GameObjectEvent removeTargetEvent;
        [SerializeField] private EventNoParam moveOneCurrencyDoneEvent;
        [SerializeField] private EventNoParam moveAllCurrencyDoneEvent;

        private bool isFirstCurrencyMoveDone;
        private bool isAnimationRunning;

        protected virtual void OnEnable()
        {
            if (currency == null)
            {
                Debug.LogError($"[{GetType().Name}] Currency variable is not assigned.");
                return;
            }

            currency.TransactionEvent?.AddListener(OnCurrencyTransaction);
            moveOneCurrencyDoneEvent?.AddListener(OnMoveOneCurrencyDone);
            moveAllCurrencyDoneEvent?.AddListener(OnMoveAllCurrencyDone);

            if (iconTarget != null)
            {
                addTargetEvent?.Raise(iconTarget);
            }

            SetDisplay(currency.Balance);
        }

        protected virtual void OnDisable()
        {
            if (currency != null)
            {
                currency.TransactionEvent?.RemoveListener(OnCurrencyTransaction);
            }

            moveOneCurrencyDoneEvent?.RemoveListener(OnMoveOneCurrencyDone);
            moveAllCurrencyDoneEvent?.RemoveListener(OnMoveAllCurrencyDone);

            if (iconTarget != null)
            {
                removeTargetEvent?.Raise(iconTarget);
            }

            isFirstCurrencyMoveDone = false;
            isAnimationRunning = false;
        }

        private void OnCurrencyTransaction(CurrencyTransaction transaction)
        {
            if (transaction.CurrencyType != currency.CurrencyType)
            {
                return;
            }

            if (transaction.AppliedDelta > 0 && transaction.HasSourcePosition)
            {
                isAnimationRunning = true;
                return;
            }

            UpdateDisplay(transaction.OldBalance, transaction.NewBalance);
        }

        private void OnMoveOneCurrencyDone()
        {
            if (!isAnimationRunning || isFirstCurrencyMoveDone)
            {
                return;
            }

            isFirstCurrencyMoveDone = true;
            UpdateDisplay(GetDisplayedBalance(), currency.Balance);
        }

        private void OnMoveAllCurrencyDone()
        {
            isFirstCurrencyMoveDone = false;
            isAnimationRunning = false;
            SetDisplay(currency.Balance);
        }

        protected virtual void UpdateDisplay(int oldValue, int newValue)
        {
            if (currencyAmountText == null)
            {
                Debug.LogWarning($"[{GetType().Name}] Currency amount text is not assigned.");
                return;
            }

            int currentDisplayed = GetDisplayedBalance(oldValue);
            if (currentDisplayed == newValue)
            {
                SetDisplay(newValue);
                return;
            }

            Tween.Create(currentDisplayed, newValue, 0.5f).OnValueChanged(value =>
            {
                if (currencyAmountText != null)
                {
                    currencyAmountText.text = FormatCurrency((int)value);
                }
            }).Play();
        }

        private int GetDisplayedBalance(int fallback = 0)
        {
            if (currencyAmountText == null || string.IsNullOrEmpty(currencyAmountText.text))
            {
                return fallback;
            }

            string cleanText = currencyAmountText.text.Replace(",", string.Empty).Replace(".", string.Empty);
            return int.TryParse(cleanText, out int value) ? value : fallback;
        }

        private void SetDisplay(int value)
        {
            if (currencyAmountText != null)
            {
                currencyAmountText.text = FormatCurrency(value);
            }
        }

        protected virtual string FormatCurrency(int value)
        {
            return value.ToString("N0", CultureInfo.InvariantCulture);
        }
    }
}
