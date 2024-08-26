using System.Collections;
using System.Threading.Tasks;
using TheBeginning.Config;
using UnityDebugSheet.Runtime.Core.Scripts;
using UnityEngine;
using VirtueSky.Variables;

namespace TheBeginning.DebugViewPage
{
    public class DebugGamePage : DefaultDebugPageBase
    {
        private ItemConfig itemConfig;
        private BooleanVariable isOffUIVariable;
        private BooleanVariable isTestingVariable;
        private Sprite iconInput;
        private Sprite iconOk;
        private Sprite iconToggle;
        private Sprite iconCoinDebug;
        private Sprite iconOutfitDebug;
        private string targetCoin = "";
        protected override string Title => "Game Debug";

        public void Init(ItemConfig _itemConfig, BooleanVariable _isOffUi,
            BooleanVariable _isTesting, Sprite _iconInput, Sprite _iconOk, Sprite _iconToggle, Sprite _iconCoinDebug,
            Sprite _iconOutfitDebug)
        {
            itemConfig = _itemConfig;
            isOffUIVariable = _isOffUi;
            isTestingVariable = _isTesting;
            iconInput = _iconInput;
            iconOk = _iconOk;
            iconToggle = _iconToggle;
            iconCoinDebug = _iconCoinDebug;
            iconOutfitDebug = _iconOutfitDebug;
        }

        public override Task Initialize()
        {
            AddButton("Add 10000 Coin", icon: iconCoinDebug, clicked: () => CoinSystem.AddCoin(10000));
            AddInputField("Input Coin:", valueChanged: s => targetCoin = s, icon: iconInput);
            AddButton("Enter Input Coin", clicked: () =>
                {
                    if (targetCoin != "") CoinSystem.SetCoin(int.Parse(targetCoin));
                },
                icon: iconOk);
            AddButton("Unlock All Skin", icon: iconOutfitDebug, clicked: () => itemConfig.UnlockAllSkins());
            AddSwitch(isOffUIVariable.Value, "Is Hide UI", valueChanged: b => isOffUIVariable.Value = b,
                icon: iconToggle);
            AddSwitch(isTestingVariable.Value, "Is Testing", valueChanged: b => isTestingVariable.Value = b,
                icon: iconToggle);
            Reload();
            return base.Initialize();
        }
    }
}