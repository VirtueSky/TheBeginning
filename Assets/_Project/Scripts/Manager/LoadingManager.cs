using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VirtueSky.Inspector;
using VirtueSky.Core;
using VirtueSky.Threading.Tasks;

public class LoadingManager : BaseMono
{
    [HeaderLine("Attributes")] public Image progressBar;
    public TextMeshProUGUI loadingText;

    [Range(0.1f, 10f)] public float timeLoading = 5f;
    [SerializeField] bool isWaitingFetchRemoteConfig = false;

    private bool flagDoneProgress;
    private bool fetchFirebaseRemoteConfigCompleted = false;

    private void Awake()
    {
        Init();
        LoadScene();
    }

    private void Init()
    {
        progressBar.fillAmount = 0;
        progressBar.DOFillAmount(1, timeLoading)
            .OnUpdate(progressBar,
                (image, tween) => loadingText.text = $"Loading... {(int)(progressBar.fillAmount * 100)}%")
            .OnComplete(() => flagDoneProgress = true);
    }

    private async void LoadScene()
    {
        await UniTask.WaitUntil(() => flagDoneProgress);
        await SceneManager.LoadSceneAsync(Constant.SERVICE_SCENE);
        if (isWaitingFetchRemoteConfig)
        {
            await UniTask.WaitUntil(() => fetchFirebaseRemoteConfigCompleted);
        }

        SceneManager.LoadSceneAsync(Constant.HOME_SCENE, LoadSceneMode.Additive).completed +=
            operation =>
            {
                if (operation.isDone)
                {
                    SceneManager.SetActiveScene(SceneManager.GetSceneByName(Constant.HOME_SCENE));
                }
            };
    }

    public void FirebaseIsInitialized()
    {
        fetchFirebaseRemoteConfigCompleted = true;
    }
}