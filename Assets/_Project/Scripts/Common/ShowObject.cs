using System.Collections.Generic;
using UnityEngine;
using VirtueSky.Core;
using VirtueSky.Inspector;
using VirtueSky.Variables;

public class ShowObject : MonoBehaviour
{
    [SerializeField] private BooleanVariable isTestingVariable;
    public bool IsShowByTesting;
    public bool IsShowByLevel;
    public float DelayShowTime;
    [SerializeField] private IntegerVariable currentLevelVariable;
    [ShowIf(nameof(IsShowByLevel))] public List<int> LevelsShow;


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
        bool testingCondition = !IsShowByTesting || (IsShowByTesting && isTestingVariable.Value);
        bool levelCondition = !IsShowByLevel || (IsShowByLevel && IsLevelInLevelsShow());
        return testingCondition && levelCondition;
    }

    private void Awake()
    {
        isTestingVariable.AddListener(SetupByIsTesting);
        currentLevelVariable.AddListener(SetupByIndexLevel);
    }

    private void OnDestroy()
    {
        isTestingVariable.RemoveListener(SetupByIsTesting);
        currentLevelVariable.RemoveListener(SetupByIndexLevel);
    }

    public void OnEnable()
    {
        Setup();
    }

    void SetupByIsTesting(bool isActive)
    {
        Setup();
    }

    void SetupByIndexLevel(int level)
    {
        Setup();
    }


    public void Setup()
    {
        if (DelayShowTime > 0) gameObject.SetActive(false);
        App.Delay(DelayShowTime, () => { gameObject.SetActive(EnableToShow()); });
    }
}