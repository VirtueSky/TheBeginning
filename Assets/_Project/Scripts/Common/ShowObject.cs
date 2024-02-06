using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using VirtueSky.Inspector;
using VirtueSky.Variables;

// [DeclareHorizontalGroup("horizontal")]
// [DeclareVerticalGroup("horizontal/vars")]
// [DeclareVerticalGroup("horizontal/buttons")]
public class ShowObject : MonoBehaviour
{
    [SerializeField] private BooleanVariable isTestingVariable;
    public bool IsShowByTesting;
    public bool IsShowByLevel;
    public bool IsShowByTime;
    public float DelayShowTime;
    [SerializeField] private IntegerVariable currentLevelVariable;
    [ShowIf(nameof(IsShowByLevel))] public List<int> LevelsShow;
    [ShowIf("IsShowByTime")] public int MaxTimeShow;

    [ShowIf("IsShowByTime")] [ReadOnly] public string ShowID;

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
        bool testingCondition = !IsShowByTesting || (IsShowByTesting && isTestingVariable);
        bool levelCondition = !IsShowByLevel || (IsShowByLevel && IsLevelInLevelsShow());
        bool timeCondition = !IsShowByTime || (IsShowByTime && UserData.GetNumberShowGameObject(ShowID) <= MaxTimeShow);
        return testingCondition && levelCondition && timeCondition;
    }

    public void OnEnable()
    {
        Setup();
    }

    public void Setup()
    {
        if (DelayShowTime > 0) gameObject.SetActive(false);
        DOTween.Sequence().AppendInterval(DelayShowTime).AppendCallback(() =>
        {
            if (IsShowByTime) UserData.IncreaseNumberShowGameObject(ShowID);
            gameObject.SetActive(EnableToShow());
        });
    }
}