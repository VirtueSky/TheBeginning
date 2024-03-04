using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using VirtueSky.Core;
using VirtueSky.Inspector;
using VirtueSky.Threading.Tasks;
using VirtueSky.Variables;

[EditorIcon("icon_controller")]
public class LevelController : BaseMono
{
    [ReadOnly] [SerializeField] private Level currentLevel;
    [SerializeField] private IntegerVariable currentIndexLevel;
    [SerializeField] private GameConfig gameConfig;
    public Level CurrentLevel => currentLevel;

    public void PrepareLevel()
    {
        GenerateLevel(currentIndexLevel.Value);
    }

    public async UniTask GenerateLevel(int indexLevel)
    {
        if (currentLevel != null)
        {
            Destroy(currentLevel.gameObject);
        }

        if (indexLevel > gameConfig.maxLevel)
        {
            indexLevel = (indexLevel - gameConfig.startLoopLevel) %
                         (gameConfig.maxLevel - gameConfig.startLoopLevel + 1) +
                         gameConfig.startLoopLevel;
        }
        else
        {
            if (gameConfig.levelLoopType == LevelLoopType.NormalLoop)
            {
                indexLevel = (indexLevel - 1) % gameConfig.maxLevel + 1;
            }
            else if (gameConfig.levelLoopType == LevelLoopType.RandomLoop)
            {
                indexLevel =
                    UnityEngine.Random.Range(gameConfig.startLoopLevel, gameConfig.maxLevel);
            }
        }

        Level level = await GetLevelByIndex(indexLevel);
        currentLevel = Instantiate(level);
        ActiveCurrentLevel(false);
    }

    public void ActiveCurrentLevel(bool active)
    {
        currentLevel.gameObject.SetActive(active);
    }


    public async UniTask<Level> GetLevelByIndex(int indexLevel)
    {
        var asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>($"Levels/Level {indexLevel}");
        await asyncOperationHandle;
        if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
        {
            return asyncOperationHandle.Result.GetComponent<Level>();
        }
        else
        {
            Debug.LogError($"Failed to load level {indexLevel}");
            return null;
        }
    }
}