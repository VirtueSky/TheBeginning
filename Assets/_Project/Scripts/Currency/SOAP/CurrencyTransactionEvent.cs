using UnityEngine;
using VirtueSky.Events;
using VirtueSky.Inspector;

namespace TheBeginning.Currency
{
    [CreateAssetMenu(menuName = "Currency/Currency Transaction Event", fileName = "currency_transaction_event")]
    [EditorIcon("scriptable_event")]
    public class CurrencyTransactionEvent : BaseEvent<CurrencyTransaction>
    {
    }
}
