using UnityEditor;
using UnityEngine;
using VirtueSky.Attributes;
using VirtueSky.Misc;
using VirtueSky.Variables;

public class Level : MonoBehaviour
{
    [SerializeField] private CurrentLevelVariable currentLevelVariable;
    [SerializeField] private IntegerVariable indexLevelVariable;

    private void Start()
    {
        currentLevelVariable.Value = this;
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