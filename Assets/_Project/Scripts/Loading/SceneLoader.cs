using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using VirtueSky.Events;

namespace TheBeginning.SceneFlow {
    public class SceneLoader : MonoBehaviour {
        [SerializeField] StringEvent changeSceneEvent;

        private void Awake() {
            DontDestroyOnLoad(this.gameObject);
        }

        public static Dictionary<string, AsyncOperationHandle<SceneInstance>> sceneHolder =
            new Dictionary<string, AsyncOperationHandle<SceneInstance>>();

        private void OnEnable() {
            changeSceneEvent.AddListener(ChangeScene);
        }

        private void OnDisable() {
            changeSceneEvent.RemoveListener(ChangeScene);
        }

        private void ChangeScene(string sceneName) {
            foreach (var scene in GetAllLoadedScene()) {
                if (!scene.name.Equals(Constant.SERVICE_SCENE)) {
                    if (sceneHolder.ContainsKey(scene.name)) {
                        Addressables.UnloadSceneAsync(sceneHolder[scene.name]);
                        sceneHolder.Remove(scene.name);
                    }
                    else {
                        SceneManager.UnloadSceneAsync(scene);
                    }
                }
            }
            Addressables.LoadSceneAsync(sceneName, LoadSceneMode.Additive).Completed += OnAdditiveSceneLoaded;
        }
        void OnAdditiveSceneLoaded(AsyncOperationHandle<SceneInstance> scene)
        {
            if (scene.Status == AsyncOperationStatus.Succeeded)
            {
                string sceneName = scene.Result.Scene.name;
                sceneHolder.Add(sceneName, scene);
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
            }
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