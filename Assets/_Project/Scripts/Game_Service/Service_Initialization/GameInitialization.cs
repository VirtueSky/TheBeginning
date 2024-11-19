using TheBeginning.Config;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using VirtueSky.Inspector;
using VirtueSky.Localization;

namespace TheBeginning.Services
{
    [HideMonoScript]
    public class GameInitialization : ServiceInitialization
    {
        [SerializeField] private GameConfig gameConfig;

        public override void Initialization()
        {
            Application.targetFrameRate = (int)gameConfig.targetFrameRate;
            Input.multiTouchEnabled = gameConfig.multiTouchEnabled;
            Locale.LoadLanguageSetting();
            Addressables.LoadSceneAsync(Constant.GAME_SCENE, LoadSceneMode.Additive);
        }
    }
}