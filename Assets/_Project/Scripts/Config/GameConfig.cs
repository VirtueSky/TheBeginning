using UnityEngine;
using VirtueSky.Inspector;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Config/GameConfig")]
public class GameConfig : ScriptableObject
{
    [Space, HeaderLine("Level config")] public int maxLevel = 2;
    public int startLoopLevel;
    public string keyLoadLevel = "Levels/Level";

    [Space, HeaderLine("Gameplay config")] public bool enableAdministrator = true;

    public TargetFrameRate targetFrameRate = TargetFrameRate.Frame60;
    public int winLevelMoney = 100;
    public int percentWinGiftPerLevel = 10;

    [Space, HeaderLine("Notification In Game")]
    public bool enableNotificationInGame = true;

    public float timeDelayHideNotificationInGame = 1.0f;

    [Space, HeaderLine("Require Internet")]
    public bool enableRequireInternet = false;

    public float timeDelayCheckInternet = 5;
    public float timeLoopCheckInternet = .5f;

    [Space, HeaderLine("Show Popup Update")]
    public bool enableShowPopupUpdate = false;
}


public enum TargetFrameRate
{
    ByDevice = -1,
    Frame60 = 60,
    Frame120 = 120,
    Frame240 = 240
}