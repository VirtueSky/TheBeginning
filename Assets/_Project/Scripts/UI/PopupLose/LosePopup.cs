using UnityEngine;
using VirtueSky.Events;

namespace TheBeginning.UI
{
    public class LosePopup : UIPopup
    {
        [SerializeField] private EventNoParam replayGameEvent;

        protected override void OnBeforeShow()
        {
            base.OnBeforeShow();
        }

        protected override void OnBeforeHide()
        {
            base.OnBeforeHide();
        }

        public void OnClickReplay()
        {
            Hide();
            replayGameEvent.Raise();
        }
    }
}