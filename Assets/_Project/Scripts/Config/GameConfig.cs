
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObject/GameConfig")]
public class GameConfig : ScriptableObject
{
    [Header("UI config")]
    public float DurationPopup = .5f;
    public int WatchAdsMoney = 1000;
    [Header("Level config")] 
    public LevelLoopType LevelLoopType;
    public int MaxLevel = 2;
    public int StartLoopLevel;
    [Header("Gameplay config")]
    public int WinLevelMoney = 100;
    public int PercentWinGiftPerLevel = 10;
}

public enum LevelLoopType
{
    NormalLoop,
    RandomLoop,
}