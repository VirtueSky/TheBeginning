using UnityEditor;
using UnityEngine;
using VirtueSky.Core;
using VirtueSky.Inspector;
using VirtueSky.Misc;
using VirtueSky.Variables;

public class Level : BaseMono
{
    [SerializeField] private IntegerVariable indexLevelVariable;
    [SerializeField] private EventGetTransformCurrentLevel eventGetTransformCurrentLevel;
    public Transform GetTransform() => transform;

    public override void OnEnable()
    {
        base.OnEnable();
        eventGetTransformCurrentLevel.AddListener(GetTransform);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        eventGetTransformCurrentLevel.RemoveListener(GetTransform);
    }

#if UNITY_EDITOR
    [Button]
    private void StartLevel()
    {
        indexLevelVariable.Value = gameObject.name.GetNumberInAString();
        EditorApplication.isPlaying = true;
    }
#endif
}