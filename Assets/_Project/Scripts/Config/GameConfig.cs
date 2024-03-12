using UnityEngine;
using VirtueSky.Inspector;
using VirtueSky.Utils;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Config/GameConfig")]
public class GameConfig : ScriptableObject
{
    [Space, HeaderLine("Level config")] public LevelLoopType levelLoopType;

    public int maxLevel = 2;
    public int startLoopLevel;
    public string keyLoadLevel = "Levels/Level";

    [Space, HeaderLine("Gameplay config")] public bool enableAdministrator = true;

    public TargetFrameRate targetFrameRate = TargetFrameRate.Frame60;
    public int winLevelMoney = 100;
    public int percentWinGiftPerLevel = 10;
}

public enum LevelLoopType
{
    NormalLoop,
    RandomLoop,
}

public enum TargetFrameRate
{
    ByDevice = -1,
    Frame60 = 60,
    Frame120 = 120,
    Frame240 = 240
}