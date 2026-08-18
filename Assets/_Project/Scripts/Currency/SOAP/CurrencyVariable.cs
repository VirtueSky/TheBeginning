using UnityEngine;
using VirtueSky.Core;
using VirtueSky.Inspector;
using VirtueSky.Variables;

namespace TheBeginning.Currency
{
    [CreateAssetMenu(menuName = "Currency/Currency Variable", fileName = "currency_variable")]
    [EditorIcon("scriptable_variable")]
    public class CurrencyVariable : BaseSO
    {
        [SerializeField] private CurrencyType currencyType;
        [SerializeField] private IntegerVariable balanceVariable;
        [SerializeField] private CurrencyTransactionEvent transactionEvent;

        public CurrencyType CurrencyType => currencyType;
        public int Balance => balanceVariable != null ? balanceVariable.Value : 0;
        public CurrencyTransactionEvent TransactionEvent => transactionEvent;

        /// <summary>
        /// Checks whether the current balance is sufficient for the requested amount.
        /// </summary>
        /// <param name="amount">The amount that needs to be paid. Negative amounts are invalid.</param>
        /// <returns>True when the amount is valid and the balance can cover it.</returns>
        public bool CanAfford(int amount)
        {
            return amount >= 0 && Balance >= amount;
        }

        /// <summary>
        /// Adds a positive amount to the balance and raises an Add transaction event.
        /// </summary>
        /// <param name="amount">The amount to add.</param>
        public void Add(int amount)
        {
            AddInternal(amount, false, default);
        }

        /// <summary>
        /// Adds a positive amount and includes its world position in the transaction event.
        /// Use this overload when the UI needs to animate currency from a source object.
        /// </summary>
        /// <param name="amount">The amount to add.</param>
        /// <param name="sourcePosition">The world position from which the currency originated.</param>
        public void Add(int amount, Vector3 sourcePosition)
        {
            AddInternal(amount, true, sourcePosition);
        }

        /// <summary>
        /// Spends a positive amount only when the current balance is sufficient.
        /// </summary>
        /// <param name="amount">The amount to spend.</param>
        /// <returns>True when the amount was successfully deducted; otherwise, false.</returns>
        public bool TrySpend(int amount)
        {
            if (!ValidatePositiveAmount(amount, nameof(TrySpend)) || !CanAfford(amount))
            {
                return false;
            }

            Commit(CurrencyOperation.Spend, amount, Balance - amount, false, default);
            return true;
        }

        /// <summary>
        /// Removes a positive amount from the balance, clamping the result to zero.
        /// Unlike TrySpend, this method does not require a sufficient balance.
        /// </summary>
        /// <param name="amount">The amount to remove.</param>
        public void Remove(int amount)
        {
            if (!ValidatePositiveAmount(amount, nameof(Remove)))
            {
                return;
            }

            Commit(CurrencyOperation.Remove, amount, Mathf.Max(0, Balance - amount), false, default);
        }

        /// <summary>
        /// Replaces the current balance with the specified amount and raises a Set transaction event.
        /// Negative amounts are clamped to zero.
        /// </summary>
        /// <param name="amount">The new balance.</param>
        public void Set(int amount)
        {
            SetInternal(amount, false, default);
        }

        /// <summary>
        /// Replaces the current balance and includes a source world position in the transaction event.
        /// Negative amounts are clamped to zero.
        /// </summary>
        /// <param name="amount">The new balance.</param>
        /// <param name="sourcePosition">The world position associated with the balance change.</param>
        public void Set(int amount, Vector3 sourcePosition)
        {
            SetInternal(amount, true, sourcePosition);
        }

        /// <summary>
        /// Validates and calculates an Add operation before committing the balance change.
        /// The result is capped at <see cref="int.MaxValue"/> to prevent overflow.
        /// </summary>
        private void AddInternal(int amount, bool hasSourcePosition, Vector3 sourcePosition)
        {
            if (!ValidatePositiveAmount(amount, nameof(Add)))
            {
                return;
            }

            int newBalance = Balance > int.MaxValue - amount ? int.MaxValue : Balance + amount;
            Commit(CurrencyOperation.Add, amount, newBalance, hasSourcePosition, sourcePosition);
        }

        /// <summary>
        /// Normalizes a requested balance and commits it as a Set operation.
        /// </summary>
        private void SetInternal(int amount, bool hasSourcePosition, Vector3 sourcePosition)
        {
            int newBalance = Mathf.Max(0, amount);
            if (amount < 0)
            {
                Debug.LogWarning($"[{currencyType}] Set received a negative amount ({amount}); clamped to 0.");
            }

            Commit(CurrencyOperation.Set, amount, newBalance, hasSourcePosition, sourcePosition);
        }

        /// <summary>
        /// Ensures that Add, Spend, and Remove operations receive a value greater than zero.
        /// </summary>
        /// <returns>True when the amount is positive.</returns>
        private bool ValidatePositiveAmount(int amount, string operation)
        {
            if (amount > 0)
            {
                return true;
            }

            if (amount < 0)
            {
                Debug.LogWarning($"[{currencyType}] {operation} requires a positive amount, received {amount}.");
            }

            return false;
        }

        /// <summary>
        /// Applies the new balance and publishes the resulting transaction to interested listeners.
        /// No event is raised when the balance does not change or the balance variable is missing.
        /// </summary>
        private void Commit(CurrencyOperation operation, int requestedAmount, int newBalance,
            bool hasSourcePosition, Vector3 sourcePosition)
        {
            if (balanceVariable == null)
            {
                Debug.LogError($"[{name}] Balance variable is not assigned.");
                return;
            }

            int oldBalance = balanceVariable.Value;
            if (oldBalance == newBalance)
            {
                return;
            }

            balanceVariable.Value = newBalance;
            transactionEvent?.Raise(new CurrencyTransaction(currencyType, operation, requestedAmount,
                newBalance - oldBalance, oldBalance, newBalance, hasSourcePosition, sourcePosition));
        }
    }
}
