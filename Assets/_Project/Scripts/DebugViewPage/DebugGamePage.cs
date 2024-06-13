using System.Collections;
using System.Threading.Tasks;
using UnityDebugSheet.Runtime.Core.Scripts;
using UnityEngine;
using VirtueSky.Variables;

namespace TheBeginning.DebugViewPage
{
    public class DebugGamePage : DefaultDebugPageBase
    {
        private IntegerVariable currentCoin;
        private ItemConfig itemConfig;
        private BooleanVariable isOffUIVariable;
        private BooleanVariable isTestingVariable;
        private Sprite iconInput;
        private Sprite iconOk;
        private Sprite iconToggle;
        private string targetCoin = "";
        protected override string Title => "Game Debug";

        public void Init(IntegerVariable _currentCoin, ItemConfig _itemConfig, BooleanVariable _isOffUi,
            BooleanVariable _isTesting, Sprite _iconInput, Sprite _iconOk, Sprite _iconToggle)
        {
            currentCoin = _currentCoin;
            itemConfig = _itemConfig;
            isOffUIVariable = _isOffUi;
            isTestingVariable = _isTesting;
            iconInput = _iconInput;
            iconOk = _iconOk;
            iconToggle = _iconToggle;
        }

        public override Task Initialize()
        {
            AddButton("Add 10000 Coin", clicked: () => currentCoin.Value += 10000);
            AddInputField("Input Coin:", valueChanged: s => targetCoin = s, icon: iconInput);
            AddButton("Enter Input Coin", clicked: () =>
                {
                    if (targetCoin != "")
                    {
                        currentCoin.Value = int.Parse(targetCoin);
                    }
                },
                icon: iconOk);
            AddButton("Unlock All Skin", clicked: () => itemConfig.UnlockAllSkins());
            AddSwitch(isOffUIVariable.Value, "Is Hide UI", valueChanged: b => isOffUIVariable.Value = b,
                icon: iconToggle);
            AddSwitch(isTestingVariable.Value, "Is Testing", valueChanged: b => isTestingVariable.Value = b,
                icon: iconToggle);
            Reload();
            return base.Initialize();
        }
    }
}