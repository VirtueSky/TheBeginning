using UnityEngine;
using UnityEngine.AddressableAssets;
using VirtueSky.Core;
using VirtueSky.Inspector;
using VirtueSky.Threading.Tasks;
using VirtueSky.Variables;

[EditorIcon("icon_controller")]
public class LevelLoader : BaseMono
{
    [ReadOnly] [SerializeField] private Level currentLevel;
    [ReadOnly] [SerializeField] private Level previousLevel;
    [SerializeField] private IntegerVariable currentIndexLevel;
    [SerializeField] private GameConfig gameConfig;
    [SerializeField] private EventLoadLevel eventLoadLevel;
    [SerializeField] private EventGetCurrentLevel eventGetCurrentLevel;
    [SerializeField] private EventGetPreviousLevel eventGetPreviousLevel;

    public Level CurrentLevel() => currentLevel;
    public Level PreviousLevel() => previousLevel;

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
        var result = await Addressables.LoadAssetAsync<GameObject>($"{gameConfig.keyLoadLevel} {index}");
        if (currentLevel != null) previousLevel = currentLevel;
        currentLevel = result.GetComponent<Level>();
        return currentLevel;
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
}