using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VirtueSky.Inspector;
using VirtueSky.Core;
using VirtueSky.Events;
using VirtueSky.Threading.Tasks;

public class LoadingManager : BaseMono
{
    [HeaderLine("Attributes")] public Image progressBar;
    public TextMeshProUGUI loadingText;

    [Range(0.1f, 10f)] public float timeLoading = 5f;
    [SerializeField] bool isWaitingFetchRemoteConfig = false;
    [SerializeField] private StringEvent changeSceneEvent;

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
        await SceneManager.LoadSceneAsync(Constant.SERVICE_SCENE, LoadSceneMode.Additive);
        await UniTask.WaitUntil(() => flagDoneProgress);
        if (isWaitingFetchRemoteConfig)
        {
            await UniTask.WaitUntil(() => fetchFirebaseRemoteConfigCompleted);
        }

        changeSceneEvent.Raise(Constant.HOME_SCENE);
    }

    public void FirebaseIsInitialized()
    {
        fetchFirebaseRemoteConfigCompleted = true;
    }
}