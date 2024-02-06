using TheBeginning.AppControl;
using UnityEditor;
using UnityEngine;
using VirtueSky.Inspector;
using VirtueSky.Misc;
using VirtueSky.Variables;

public class Level : MonoBehaviour
{
    [SerializeField] private IntegerVariable indexLevelVariable;

    private void Start()
    {
        AppControlCurrentLevel.Init(this);
    }


#if UNITY_EDITOR
    [Button]
    private void StartLevel()
    {
        indexLevelVariable.Value = Common.GetNumberInAString(gameObject.name);

        EditorApplication.isPlaying = true;
    }
#endif
}