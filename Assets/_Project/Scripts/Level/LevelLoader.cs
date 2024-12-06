using UnityEngine;
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
        [SerializeField] private LevelSettings levelSettings;
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

        private Level LoadLevel()
        {
            int index = HandleIndexLevel(currentIndexLevel.Value);
            var result = levelSettings.GePrefabLevel($"Level {index}");
            if (currentLevel != null)
            {
                previousLevel = currentLevel;
            }
            else
            {
                int indexPrev = HandleIndexLevel(currentIndexLevel.Value - 1);
                var resultPre = levelSettings.GePrefabLevel($"Level {indexPrev}");
                previousLevel = resultPre.GetComponent<Level>();
            }

            currentLevel = result.GetComponent<Level>();
            return currentLevel;
        }

        int HandleIndexLevel(int indexLevel)
        {
            if (indexLevel > levelSettings.MaxLevel)
            {
                return (indexLevel - levelSettings.StartLoopLevel) %
                       (levelSettings.MaxLevel - levelSettings.StartLoopLevel + 1) +
                       levelSettings.StartLoopLevel;
            }

            if (indexLevel > 0 && indexLevel <= levelSettings.MaxLevel)
            {
                //return (indexLevel - 1) % gameConfig.maxLevel + 1;
                return indexLevel;
            }

            if (indexLevel == 0)
            {
                return levelSettings.MaxLevel;
            }

            return 1;
        }

        public void ActiveCurrentLevel(bool active)
        {
            currentLevel.gameObject.SetActive(active);
        }
    }
}