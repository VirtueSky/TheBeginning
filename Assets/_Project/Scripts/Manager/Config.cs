using UnityEngine;
using VirtueSky.Inspector;
using VirtueSky.Core;
#if UNITY_EDITOR
using VirtueSky.UtilsEditor;
#endif


public class Config : BaseMono
{
    [HeaderLine("Scriptable Config")] [SerializeField]
    private GameConfig gameConfig;

    [SerializeField] private DailyRewardConfig dailyRewardConfig;

    [SerializeField] private CountryConfig countryConfig;

    [SerializeField] private ItemConfig itemConfig;

    public static GameConfig Game;
    public static DailyRewardConfig DailyRewardConfig;
    public static CountryConfig CountryConfig;
    public static ItemConfig ItemConfig;

    protected void Awake()
    {
        Game = gameConfig;
        DailyRewardConfig = dailyRewardConfig;
        CountryConfig = countryConfig;
        ItemConfig = itemConfig;
    }

    public override void Initialize()
    {
        base.Initialize();
        ItemConfig.Initialize();
    }

#if UNITY_EDITOR
    [Button]
    private void Load()
    {
        string path = "Assets/_Project/Config/";
        gameConfig = FileExtension.GetConfigFromFolder<GameConfig>(path);
        dailyRewardConfig = FileExtension.GetConfigFromFolder<DailyRewardConfig>(path);
        countryConfig = FileExtension.GetConfigFromFolder<CountryConfig>(path);
        itemConfig = FileExtension.GetConfigFromFolder<ItemConfig>(path);
    }
#endif
}