using Coffee.UIEffects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TheBeginning.UI
{
    public sealed class LeaderboardElementView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI txtRank;
        [SerializeField] private TextMeshProUGUI txtUserName;
        [SerializeField] private TextMeshProUGUI txtScore;
        [SerializeField] private Image imgForcegound;
        [SerializeField] private Image imgOverlay1;
        [SerializeField] private Image imgOverlay2;
        [SerializeField] private GameObject you;
        [SerializeField] private Image imgHeader;
        [SerializeField] private GameObject decor;


        public void Init(int rank, string userName, int score, LeaderboardElementColor color, bool self)
        {
            txtRank.text = $"{rank}";
            txtUserName.text = userName;
            txtScore.color = color.colorText;
            txtUserName.color = color.colorText;
            imgForcegound.color = color.colorBackground;
            imgOverlay1.color = color.colorOverlay;
            imgOverlay2.color = color.colorBorder;
            imgHeader.color = color.colorHeader;
            txtScore.text = score.ToString();
            you.SetActive(self);
            if (rank != 1 && rank != 2 && rank != 3)
            {
                txtRank.gameObject.SetActive(true);
                decor.SetActive(false);
            }
            else
            {
                txtRank.gameObject.SetActive(false);
                decor.SetActive(true);
            }

            // TryGetComponent<UIShiny>(out var uiShiny);
            //  if (uiShiny != null) uiShiny.enabled = (rank == 1 || rank == 2 || rank == 3);
        }
    }
}