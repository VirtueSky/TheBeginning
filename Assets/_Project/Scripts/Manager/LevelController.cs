using UnityEngine;
using VirtueSky.Inspector;
using VirtueSky.Variables;
using Debug = System.Diagnostics.Debug;

public class LevelController : MonoBehaviour
{
    [ReadOnly] public Level currentLevel;
    [SerializeField] private IntegerVariable currentIndexLevel;
    private GameConfig Game => Config.Game;

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

        if (indexLevel > Config.Game.MaxLevel)
        {
            indexLevel = (indexLevel - Game.StartLoopLevel) % (Game.MaxLevel - Game.StartLoopLevel + 1) +
                         Game.StartLoopLevel;
        }
        else
        {
            if (Game.LevelLoopType == LevelLoopType.NormalLoop)
            {
                indexLevel = (indexLevel - 1) % Config.Game.MaxLevel + 1;
            }
            else if (Game.LevelLoopType == LevelLoopType.RandomLoop)
            {
                indexLevel = UnityEngine.Random.Range(Game.StartLoopLevel, Game.MaxLevel);
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