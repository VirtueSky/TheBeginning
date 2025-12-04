using System;
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
        [SerializeField] private StringEvent changeSceneEvent;
        private Rect rect = new Rect(0, 0, 1, 1);
        private bool isProgressDone = false;

        private void Awake() {
            Init();
            LoadScene();
        }

        private void Init() {
            progressBar.fillAmount = 0;
            progressBar.DOFillAmount(1, timeLoading)
                .OnUpdate(progressBar,
                    (image, tween) =>
                    {
                        localeTextComponent.UpdateArgs($"{(int)(progressBar.fillAmount * 100)}");
                        rect.x -= Time.deltaTime * 0.1f;
                        rect.y -= Time.deltaTime * 0.1f;
                        rawImage.uvRect = rect;
                    })
                .OnComplete(() => isProgressDone = true, false);
        }
        
        private async void LoadScene()
        {
            await Addressables.LoadSceneAsync(Constant.SERVICE_SCENE, LoadSceneMode.Additive);
            await UniTask.WaitUntil(()=> isProgressDone);
            App.Delay(1.0f, () => { showNotificationInGameEvent.Raise("Welcome TheBeginning"); });
            if (isFetchRemoteConfigCompleted != null)
            {
                await UniTask.WaitUntil(() => isFetchRemoteConfigCompleted.Value);
            }
            changeSceneEvent.Raise(Constant.GAME_SCENE);
        }
    }
}