using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using VirtueSky.Misc;
using VirtueSky.Variables;

public class Level : MonoBehaviour
{
    [SerializeField] private CurrentLevelVariable currentLevelVariable;
    [SerializeField] private IntegerVariable indexLevelVariable;
    private bool _isFingerDown;
    private bool _isFingerDrag;

    private void Start()
    {
        currentLevelVariable.Value = this;
    }

    private void OnDestroy()
    {
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