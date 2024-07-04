using PrimeTween;
using TheBeginning;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VirtueSky.Inspector;
using VirtueSky.Core;
using VirtueSky.Events;
using VirtueSky.Threading.Tasks;
using VirtueSky.Variables;
#if UNITY_IOS
using Unity.Advertisement.IosSupport;
#endif

[EditorIcon("icon_manager")]
public class LoadingManager : BaseMono
{
    [SerializeField] private GameConfig gameConfig;
    [HeaderLine("Attributes")] public Image progressBar;
    public TextMeshProUGUI loadingText;
    [Range(0.1f, 10f)] public float timeLoading = 5f;
    [SerializeField] private StringEvent changeSceneEvent;
    [SerializeField] private BooleanVariable isFetchRemoteConfigCompleted;
    private bool flagDoneProgress;
    private bool fetchFirebaseRemoteConfigCompleted = false;

    private void Awake()
    {
        Init();
        LoadScene();
        Input.multiTouchEnabled = gameConfig.multiTouchEnabled;
    }


    private void Init()
    {
#if UNITY_IOS
        if (ATTrackingStatusBinding.GetAuthorizationTrackingStatus() ==
            ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
        {
            ATTrackingStatusBinding.RequestAuthorizationTracking();
        }
#endif

        progressBar.fillAmount = 0;
        progressBar.DOFillAmount(1, timeLoading)
            .OnUpdate(progressBar,
                (image, tween) => loadingText.text = $"Loading... {(int)(progressBar.fillAmount * 100)}%")
            .OnComplete(() => flagDoneProgress = true);
    }

    private async void LoadScene()
    {
        await Addressables.LoadSceneAsync(Constant.SERVICE_SCENE, LoadSceneMode.Additive);
        await UniTask.WaitUntil(() => flagDoneProgress);

        if (isFetchRemoteConfigCompleted != null)
        {
            await UniTask.WaitUntil(() => isFetchRemoteConfigCompleted.Value);
        }

        // Addressables.LoadSceneAsync(Constant.GAME_SCENE, LoadSceneMode.Additive).Completed += OnServiceLoaded;
        changeSceneEvent.Raise(Constant.GAME_SCENE);
    }


    void OnServiceLoaded(AsyncOperationHandle<SceneInstance> scene)
    {
        if (scene.Status == AsyncOperationStatus.Succeeded)
        {
            string sceneName = scene.Result.Scene.name;
            Utility.sceneHolder.Add(sceneName, scene);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        }
    }
}