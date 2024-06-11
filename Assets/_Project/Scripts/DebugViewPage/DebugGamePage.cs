using System.Collections;
using UnityDebugSheet.Runtime.Core.Scripts;
using VirtueSky.Variables;

namespace TheBeginning.DebugViewPage
{
    public class DebugGamePage : DefaultDebugPageBase
    {
        private IntegerVariable currentCoin;
        private ItemConfig itemConfig;
        private BooleanVariable isOffUIVariable;
        private string _targetCoin;
        protected override string Title => "Tool Debug";

        public void Init(IntegerVariable _currentCoin, ItemConfig _itemConfig, BooleanVariable _isOffUi)
        {
            currentCoin = _currentCoin;
            itemConfig = _itemConfig;
            isOffUIVariable = _isOffUi;
        }

        public override IEnumerator Initialize()
        {
            AddButton("Add 10000 Coin", clicked: () => currentCoin.Value += 10000);
            AddInputField("Input Coin:", valueChanged: s => _targetCoin = s);
            AddButton("Enter Input Coin", clicked: () => currentCoin.Value = int.Parse(_targetCoin));
            AddButton("Unlock All Skin", clicked: () => itemConfig.UnlockAllSkins());
            AddSwitch(isOffUIVariable.Value, "Is Hide UI", valueChanged: b => isOffUIVariable.Value = b);
            return base.Initialize();
        }
    }
}