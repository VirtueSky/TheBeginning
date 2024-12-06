using UnityEngine;
using UnityEngine.SceneManagement;
using VirtueSky.Inspector;
using VirtueSky.Localization;

namespace TheBeginning.Services
{
    [HideMonoScript]
    public class GameInitialization : ServiceInitialization
    {
        [SerializeField] private GameSettings gameSettings;

        public override void Initialization()
        {
            Application.targetFrameRate = (int)gameSettings.TargetFrameRate;
            Input.multiTouchEnabled = gameSettings.MultiTouchEnabled;
            Locale.LoadLanguageSetting();
            SceneManager.LoadScene(Constant.GAME_SCENE, LoadSceneMode.Additive);
        }
    }
}