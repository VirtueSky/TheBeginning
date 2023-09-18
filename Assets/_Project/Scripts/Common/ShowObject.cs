using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using VirtueSky.Variables;

// [DeclareHorizontalGroup("horizontal")]
// [DeclareVerticalGroup("horizontal/vars")]
// [DeclareVerticalGroup("horizontal/buttons")]
public class ShowObject : MonoBehaviour
{
    public bool IsShowByTesting;
    public bool IsShowByLevel;
    public bool IsShowByTime;
    public float DelayShowTime;
    [SerializeField] private IntegerVariable currentLevelVariable;
    [ShowIf(nameof(IsShowByLevel))] public List<int> LevelsShow;
    [ShowIf("IsShowByTime")] public int MaxTimeShow;

    [ShowIf("IsShowByTime")] [ReadOnly]
    public string ShowID;

    [ShowIf("IsShowByTime")]
    [Button]
    public void RandomShowID()
    {
        if (ShowID == null || ShowID == "")
        {
            // ShowID = Ulid.NewUlid().ToString();
        }
    }

    private bool IsLevelInLevelsShow()
    {
        foreach (int item in LevelsShow)
        {
            if (currentLevelVariable.Value == item)
            {
                return true;
            }
        }

        return false;
    }

    private bool EnableToShow()
    {
        bool testingCondition = !IsShowByTesting || (IsShowByTesting && Data.IsTesting);
        bool levelCondition = !IsShowByLevel || (IsShowByLevel && IsLevelInLevelsShow());
        bool timeCondition = !IsShowByTime || (IsShowByTime && Data.GetNumberShowGameObject(ShowID) <= MaxTimeShow);
        return testingCondition && levelCondition && timeCondition;
    }

    public void Awake()
    {
        Setup();

        if (IsShowByLevel) Observer.CurrentLevelChanged += Setup;
        if (IsShowByTesting) Observer.DebugChanged += Setup;
    }

    private void OnDestroy()
    {
        if (IsShowByLevel) Observer.CurrentLevelChanged -= Setup;
        if (IsShowByTesting) Observer.DebugChanged -= Setup;
    }

    public void Setup()
    {
        if (DelayShowTime > 0) gameObject.SetActive(false);
        DOTween.Sequence().AppendInterval(DelayShowTime).AppendCallback(() =>
        {
            if (IsShowByTime) Data.IncreaseNumberShowGameObject(ShowID);
            gameObject.SetActive(EnableToShow());
        });
    }
}