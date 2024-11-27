using TheBeginning.Config;
using UnityEngine;
using UnityEngine.SceneManagement;
using VirtueSky.Inspector;
using VirtueSky.Localization;

namespace TheBeginning.Services
{
    [HideMonoScript]
    public class GameInitialization : ServiceInitialization
    {
        public override void Initialization()
        {
            Application.targetFrameRate = (int)GameConfig.TargetFrameRate;
            Input.multiTouchEnabled = GameConfig.MultiTouchEnabled;
            Locale.LoadLanguageSetting();
            SceneManager.LoadScene(Constant.GAME_SCENE, LoadSceneMode.Additive);
        }
    }
}