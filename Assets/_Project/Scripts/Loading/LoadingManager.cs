using Cysharp.Threading.Tasks;
using PrimeTween;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VirtueSky.Inspector;
using VirtueSky.Core;
using VirtueSky.Events;
using VirtueSky.Localization;
using VirtueSky.Variables;

namespace TheBeginning.SceneFlow
{
    [EditorIcon("icon_manager")]
    public class LoadingManager : BaseMono
    {
        [HeaderLine("Attributes")] public Image progressBar;
        [SerializeField] private RawImage rawImage;
        [SerializeField] private LocaleTextComponent localeTextComponent;
        [Range(0.1f, 10f)] public float timeLoading = 5f;
        [SerializeField] private BooleanVariable isFetchRemoteConfigCompleted;
        [SerializeField] private StringEvent showNotificationInGameEvent;
        private Rect rect = new Rect(0, 0, 1, 1);
        private bool _isInit;

        private void Start()
        {
            progressBar.fillAmount = 0;
            progressBar.DOFillAmount(1, timeLoading)
                .OnUpdate(progressBar,
                    (image, tween) =>
                    {
                        if (progressBar.fillAmount >= 0.2f && !_isInit)
                        {
                            _isInit = true;
                            Init();
                        }
                        localeTextComponent.UpdateArgs($"{(int)(progressBar.fillAmount * 100)}");
                        rect.x -= Time.deltaTime * 0.1f;
                        rect.y -= Time.deltaTime * 0.1f;
                        rawImage.uvRect = rect;
                    })
                .OnComplete(Done);
        }

        async void  Init()
        {
            await Addressables.LoadSceneAsync(Constant.SERVICE_SCENE,LoadSceneMode.Additive);
        }

        private async void Done()
        {
            if (isFetchRemoteConfigCompleted != null)
            {
                await UniTask.WaitUntil(() => isFetchRemoteConfigCompleted.Value);
            }

            App.Delay(1.0f, () => { showNotificationInGameEvent.Raise("Welcome TheBeginning"); });
            gameObject.SetActive(false);
        }
    }
}