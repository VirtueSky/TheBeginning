using System.Collections;
using System.Threading.Tasks;
using UnityDebugSheet.Runtime.Core.Scripts;
using VirtueSky.Variables;

namespace TheBeginning.DebugViewPage
{
    public class DebugGamePage : DefaultDebugPageBase
    {
        private IntegerVariable currentCoin;
        private ItemConfig itemConfig;
        private BooleanVariable isOffUIVariable;
        private BooleanVariable isTestingVariable;
        private string _targetCoin;
        protected override string Title => "Game Debug";

        public void Init(IntegerVariable _currentCoin, ItemConfig _itemConfig, BooleanVariable _isOffUi,
            BooleanVariable _isTesting)
        {
            currentCoin = _currentCoin;
            itemConfig = _itemConfig;
            isOffUIVariable = _isOffUi;
            isTestingVariable = _isTesting;
        }

        public override Task Initialize()
        {
            AddButton("Add 10000 Coin", clicked: () => currentCoin.Value += 10000);
            AddInputField("Input Coin:", valueChanged: s => _targetCoin = s, icon: DebugViewStatic.IconInputDebug);
            AddButton("Enter Input Coin", clicked: () => currentCoin.Value = int.Parse(_targetCoin),
                icon: DebugViewStatic.IconOkeDebug);
            AddButton("Unlock All Skin", clicked: () => itemConfig.UnlockAllSkins());
            AddSwitch(isOffUIVariable.Value, "Is Hide UI", valueChanged: b => isOffUIVariable.Value = b,
                icon: DebugViewStatic.IconToggleDebug);
            AddSwitch(isTestingVariable.Value, "Is Testing", valueChanged: b => isTestingVariable.Value = b,
                icon: DebugViewStatic.IconToggleDebug);
            Reload();
            return base.Initialize();
        }
    }
}