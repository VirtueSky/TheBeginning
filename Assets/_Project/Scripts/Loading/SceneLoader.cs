using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using VirtueSky.Events;

namespace TheBeginning.SceneFlow {
    public class SceneLoader : MonoBehaviour {
        [SerializeField] StringEvent changeSceneEvent;

        // Raised by the incoming scene once its first screen is visible; the old scene stays on screen until then.
        [SerializeField] EventNoParam sceneReadyEvent;
        [SerializeField] float sceneReadyTimeout = 5f;

        private bool isSceneReady;
        private bool isChangingScene;

        private void Awake() {
            DontDestroyOnLoad(this.gameObject);
        }

        public static Dictionary<string, AsyncOperationHandle<SceneInstance>> sceneHolder =
            new Dictionary<string, AsyncOperationHandle<SceneInstance>>();

        // Domain reload is disabled in Enter Play Mode Options, so statics survive between play sessions.
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetRegistry() => sceneHolder.Clear();

        private void OnEnable() {
            changeSceneEvent.AddListener(ChangeScene);
            if (sceneReadyEvent != null) sceneReadyEvent.AddListener(OnSceneReady);
        }

        private void OnDisable() {
            changeSceneEvent.RemoveListener(ChangeScene);
            if (sceneReadyEvent != null) sceneReadyEvent.RemoveListener(OnSceneReady);
        }

        private void OnSceneReady() => isSceneReady = true;

        private void ChangeScene(string sceneName) {
            ChangeSceneAsync(sceneName).Forget();
        }

        // Load the new scene before unloading the old ones so there is never a frame without a full screen.
        private async UniTaskVoid ChangeSceneAsync(string sceneName) {
            if (isChangingScene) return;
            isChangingScene = true;

            var oldScenes = new List<(Scene scene, AsyncOperationHandle<SceneInstance>? handle)>();
            foreach (var scene in GetAllLoadedScene()) {
                if (scene.name.Equals(Constant.SERVICE_SCENE)) continue;
                oldScenes.Add(sceneHolder.TryGetValue(scene.name, out var oldHandle) ? (scene, oldHandle) : (scene, null));
                sceneHolder.Remove(scene.name);
            }
            isSceneReady = false;

            var handle = Addressables.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            await handle;
            if (handle.Status != AsyncOperationStatus.Succeeded) {
                Debug.LogError($"Load scene {sceneName} failed");
                isChangingScene = false;
                return;
            }

            var loadedScene = handle.Result.Scene;
            sceneHolder[loadedScene.name] = handle;
            SceneManager.SetActiveScene(loadedScene);

            if (sceneReadyEvent != null) {
                await UniTask.WaitUntil(() => isSceneReady)
                    .TimeoutWithoutException(TimeSpan.FromSeconds(sceneReadyTimeout));
            }

            foreach (var (scene, oldHandle) in oldScenes) {
                if (!scene.isLoaded) continue;
                if (oldHandle.HasValue) _ = Addressables.UnloadSceneAsync(oldHandle.Value);
                else _ = SceneManager.UnloadSceneAsync(scene);
            }

            isChangingScene = false;
        }

        private Scene[] GetAllLoadedScene()
        {
            int countLoaded = SceneManager.sceneCount;
            var loadedScenes = new Scene[countLoaded];

            for (var i = 0; i < countLoaded; i++)
            {
                loadedScenes[i] = SceneManager.GetSceneAt(i);
            }

            return loadedScenes;
        }
    }
}
