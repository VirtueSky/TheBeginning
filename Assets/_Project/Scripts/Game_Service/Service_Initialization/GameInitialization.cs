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
        public override void Initialization()
        {
            Application.targetFrameRate = (int)GameConfig.Instance.targetFrameRate;
            Input.multiTouchEnabled = GameConfig.Instance.multiTouchEnabled;
            Locale.LoadLanguageSetting();
            Addressables.LoadSceneAsync(Constant.GAME_SCENE, LoadSceneMode.Additive);
        }
    }
}