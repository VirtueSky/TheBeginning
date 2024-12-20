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
        private BooleanVariable debugOnOffUIVariable;
        private BooleanVariable isTestingVariable;
        private Sprite iconInput;
        private Sprite iconOk;
        private Sprite iconToggle;
        private Sprite iconCoinDebug;
        private Sprite iconOutfitDebug;
        private string targetCoin = "";
        protected override string Title => "Game Debug";

        public void Init(BooleanVariable _onOffUi,
            BooleanVariable _isTesting, Sprite _iconInput, Sprite _iconOk, Sprite _iconToggle, Sprite _iconCoinDebug,
            Sprite _iconOutfitDebug)
        {
            debugOnOffUIVariable = _onOffUi;
            isTestingVariable = _isTesting;
            iconInput = _iconInput;
            iconOk = _iconOk;
            iconToggle = _iconToggle;
            iconCoinDebug = _iconCoinDebug;
            iconOutfitDebug = _iconOutfitDebug;
        }


#if UDS_USE_ASYNC_METHODS
        public override Task Initialize()
        {
            OnInitialize();
            return base.Initialize();
        }
#else
        public override IEnumerator Initialize()
        {
            OnInitialize();
            return base.Initialize();
        }
#endif
        void OnInitialize()
        {
            AddButton("Add 10000 Coin", icon: iconCoinDebug, clicked: () => CoinSystem.AddCoin(10000));
            AddInputField("Input Coin:", valueChanged: s => targetCoin = s, icon: iconInput);
            AddButton("Enter Input Coin", clicked: () =>
                {
                    if (targetCoin != "") CoinSystem.SetCoin(int.Parse(targetCoin));
                },
                icon: iconOk);
            AddSwitch(debugOnOffUIVariable.Value, "On/Off UI", valueChanged: b => debugOnOffUIVariable.Value = b,
                icon: iconToggle);
            AddSwitch(isTestingVariable.Value, "Is Testing", valueChanged: b => isTestingVariable.Value = b,
                icon: iconToggle);
            Reload();
        }
    }
}