using System;
using UnityEngine;

namespace TheBeginning.Currency
{
    public enum CurrencyOperation
    {
        Add,
        Spend,
        Remove,
        Set
    }

    [Serializable]
    public struct CurrencyTransaction
    {
        [SerializeField] private CurrencyType currencyType;
        [SerializeField] private CurrencyOperation operation;
        [SerializeField] private int requestedAmount;
        [SerializeField] private int appliedDelta;
        [SerializeField] private int oldBalance;
        [SerializeField] private int newBalance;
        [SerializeField] private bool hasSourcePosition;
        [SerializeField] private Vector3 sourcePosition;

        public CurrencyType CurrencyType => currencyType;
        public CurrencyOperation Operation => operation;
        public int RequestedAmount => requestedAmount;
        public int AppliedDelta => appliedDelta;
        public int OldBalance => oldBalance;
        public int NewBalance => newBalance;
        public bool HasSourcePosition => hasSourcePosition;
        public Vector3 SourcePosition => sourcePosition;

        public CurrencyTransaction(CurrencyType currencyType, CurrencyOperation operation, int requestedAmount,
            int appliedDelta, int oldBalance, int newBalance, bool hasSourcePosition, Vector3 sourcePosition)
        {
            this.currencyType = currencyType;
            this.operation = operation;
            this.requestedAmount = requestedAmount;
            this.appliedDelta = appliedDelta;
            this.oldBalance = oldBalance;
            this.newBalance = newBalance;
            this.hasSourcePosition = hasSourcePosition;
            this.sourcePosition = sourcePosition;
        }
    }
}
