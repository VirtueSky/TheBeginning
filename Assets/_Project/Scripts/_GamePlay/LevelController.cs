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

        if (indexLevel > gameConfig.MaxLevel)
        {
            indexLevel = (indexLevel - gameConfig.StartLoopLevel) %
                         (gameConfig.MaxLevel - gameConfig.StartLoopLevel + 1) +
                         gameConfig.StartLoopLevel;
        }
        else
        {
            if (gameConfig.LevelLoopType == LevelLoopType.NormalLoop)
            {
                indexLevel = (indexLevel - 1) % gameConfig.MaxLevel + 1;
            }
            else if (gameConfig.LevelLoopType == LevelLoopType.RandomLoop)
            {
                indexLevel =
                    UnityEngine.Random.Range(gameConfig.StartLoopLevel, gameConfig.MaxLevel);
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