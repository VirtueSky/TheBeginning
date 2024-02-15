using UnityEngine;
using VirtueSky.Inspector;
using VirtueSky.Variables;
using Debug = System.Diagnostics.Debug;

public class LevelController : MonoBehaviour
{
    [ReadOnly] public Level currentLevel;
    [SerializeField] private IntegerVariable currentIndexLevel;
    [SerializeField] private GameConfig gameConfig;

    public void PrepareLevel()
    {
        GenerateLevel(currentIndexLevel.Value);
    }

    public void GenerateLevel(int indexLevel)
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

        Level level = GetLevelByIndex(indexLevel);
        currentLevel = Instantiate(level);
        currentLevel.gameObject.SetActive(false);
    }

    public Level GetLevelByIndex(int indexLevel)
    {
        var levelGo = Resources.Load($"Levels/Level {indexLevel}") as GameObject;
        Debug.Assert(levelGo != null, nameof(levelGo) + " != null");
        return levelGo.GetComponent<Level>();
    }
}