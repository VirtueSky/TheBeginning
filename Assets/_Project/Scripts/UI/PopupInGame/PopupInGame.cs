using System.Collections.Generic;
using System.Linq;
using TheBeginning.LevelSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using VirtueSky.Audio;
using VirtueSky.Events;
using VirtueSky.Inspector;
using VirtueSky.Variables;

namespace TheBeginning.UI
{
    public class PopupInGame : UIPopup
    {
        [HeaderLine(Constant.Normal_Attribute)]
        public TextMeshProUGUI LevelText;

        public TextMeshProUGUI LevelTypeText;

        [HeaderLine(Constant.SO_Event)] [SerializeField]
        private EventNoParam replayEvent;

        [SerializeField] private EventNoParam callReturnHomeEvent;
        [SerializeField] private EventNoParam nextLevelEvent;
        [SerializeField] private EventNoParam backLevelEvent;
        [SerializeField] private FloatEvent winLevelEvent;
        [SerializeField] private FloatEvent loseLevelEvent;

        [HeaderLine(Constant.SO_Variable)] [SerializeField]
        private IntegerVariable indexLevelVariable;

        [SerializeField] private InterAdVariable interAdVariable;

        [HeaderLine("Audio")] [SerializeField] private PlayMusicEvent playMusicEvent;

        [SerializeField] private SoundData musicInGame;

        private List<UIEffect> UIEffects => GetComponentsInChildren<UIEffect>().ToList();


        protected override void OnBeforeShow()
        {
            base.OnBeforeShow();
            Setup(indexLevelVariable.Value);
            indexLevelVariable.AddListener(Setup);
            playMusicEvent.Raise(musicInGame);
        }

        protected override void OnBeforeHide()
        {
            base.OnBeforeHide();
            indexLevelVariable.RemoveListener(Setup);
        }

        public void Setup(int currentLevel)
        {
            LevelText.text = $"Level {currentLevel}";
            // LevelTypeText.text = $"Level ";
        }

        public void OnClickHome()
        {
            callReturnHomeEvent.Raise();
        }

        public void OnClickReplay()
        {
            interAdVariable.Show(() => { replayEvent.Raise(); });
        }

        public void OnClickPrevious()
        {
            backLevelEvent.Raise();
        }

        public void OnClickSkip()
        {
            nextLevelEvent.Raise();
        }

        public void OnClickLose()
        {
            loseLevelEvent.Raise(1);
        }

        public void OnClickWin()
        {
            winLevelEvent.Raise(1);
        }

        public void HideUI(Level level = null)
        {
            if (UIEffects.Count == 0) return;
            foreach (UIEffect item in UIEffects)
            {
                item.PlayAnim();
            }
        }
    }
}