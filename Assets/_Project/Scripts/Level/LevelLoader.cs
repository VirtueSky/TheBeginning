using Cysharp.Threading.Tasks;
using TheBeginning.Config;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VirtueSky.Core;
using VirtueSky.Inspector;
using VirtueSky.Variables;

namespace TheBeginning.LevelSystem
{
    [EditorIcon("icon_controller"), HideMonoScript]
    public class LevelLoader : BaseMono
    {
        [ReadOnly] [SerializeField] private Level currentLevel;
        [ReadOnly] [SerializeField] private Level previousLevel;
        [SerializeField] private IntegerVariable currentIndexLevel;
        [SerializeField] private EventLoadLevel eventLoadLevel;
        [SerializeField] private EventGetCurrentLevel eventGetCurrentLevel;
        [SerializeField] private EventGetPreviousLevel eventGetPreviousLevel;

        private Level CurrentLevel() => currentLevel;
        private Level PreviousLevel() => previousLevel;

        public override void OnEnable()
        {
            base.OnEnable();
            eventLoadLevel.AddListener(LoadLevel);
            eventGetCurrentLevel.AddListener(CurrentLevel);
            eventGetPreviousLevel.AddListener(PreviousLevel);
        }

        private void Start()
        {
            var instance = LoadLevel();
        }

        public async UniTask<Level> LoadLevel()
        {
            int index = HandleIndexLevel(currentIndexLevel.Value);
            var result = await Addressables.LoadAssetAsync<GameObject>($"Level {index}");
            if (currentLevel != null)
            {
                previousLevel = currentLevel;
            }
            else
            {
                int indexPrev = HandleIndexLevel(currentIndexLevel.Value - 1);
                var resultPre = await Addressables.LoadAssetAsync<GameObject>($"Level {indexPrev}");
                previousLevel = resultPre.GetComponent<Level>();
            }

            currentLevel = result.GetComponent<Level>();
            return currentLevel;
        }

        int HandleIndexLevel(int indexLevel)
        {
            if (indexLevel > GameConfig.Instance.maxLevel)
            {
                return (indexLevel - GameConfig.Instance.startLoopLevel) %
                       (GameConfig.Instance.maxLevel - GameConfig.Instance.startLoopLevel + 1) +
                       GameConfig.Instance.startLoopLevel;
            }

            if (indexLevel > 0 && indexLevel <= GameConfig.Instance.maxLevel)
            {
                //return (indexLevel - 1) % gameConfig.maxLevel + 1;
                return indexLevel;
            }

            if (indexLevel == 0)
            {
                return GameConfig.Instance.maxLevel;
            }

            return 1;
        }

        public void ActiveCurrentLevel(bool active)
        {
            currentLevel.gameObject.SetActive(active);
        }
    }
}