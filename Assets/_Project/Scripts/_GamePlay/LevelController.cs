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
    [SerializeField] private Transform levelHolder;

    private Level nextLevel;
    private Level previousLevel;
    public Level CurrentLevel => currentLevel;


    public async void PrepareLevel()
    {
        await GenerateLevel(currentIndexLevel.Value);
    }

    public async UniTask GenerateLevel(int index)
    {
        if (currentLevel != null)
        {
            Destroy(currentLevel.gameObject);
        }

        if (nextLevel == null)
        {
            Level levelTemp = await GetLevelByIndex(index);
            currentLevel = Instantiate(levelTemp, levelHolder);
            ActiveCurrentLevel(false);
        }
        else
        {
            Level levelTemp = nextLevel;
            currentLevel = Instantiate(levelTemp, levelHolder);
            ActiveCurrentLevel(false);
        }

        nextLevel = await GetLevelByIndex(index + 1);
        previousLevel = await GetLevelByIndex(index - 1);
    }

    int HandleIndexLevel(int indexLevel)
    {
        if (indexLevel > gameConfig.maxLevel)
        {
            return (indexLevel - gameConfig.startLoopLevel) %
                   (gameConfig.maxLevel - gameConfig.startLoopLevel + 1) +
                   gameConfig.startLoopLevel;
        }
        else
        {
            if (gameConfig.levelLoopType == LevelLoopType.NormalLoop)
            {
                return (indexLevel - 1) % gameConfig.maxLevel + 1;
            }
            else if (gameConfig.levelLoopType == LevelLoopType.RandomLoop)
            {
                return UnityEngine.Random.Range(gameConfig.startLoopLevel, gameConfig.maxLevel);
            }
        }

        return 1;
    }

    public void ActiveCurrentLevel(bool active)
    {
        currentLevel.gameObject.SetActive(active);
    }


    public async UniTask<Level> GetLevelByIndex(int indexLevel)
    {
        int index = HandleIndexLevel(indexLevel);
        var asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>($"Levels/Level {index}");
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