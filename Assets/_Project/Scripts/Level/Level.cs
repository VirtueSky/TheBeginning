using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using VirtueSky.Misc;

public class Level : MonoBehaviour
{
    private bool _isFingerDown;
    private bool _isFingerDrag;

#if UNITY_EDITOR
    [Button]
    private void StartLevel()
    {
        Data.CurrentLevel = Common.GetNumberInAString(gameObject.name);

        EditorApplication.isPlaying = true;
    }
#endif

    private void Start()
    {
        Observer.WinLevel += OnWin;
        Observer.LoseLevel += OnLose;
    }

    private void OnDestroy()
    {
        Observer.WinLevel -= OnWin;
        Observer.LoseLevel -= OnLose;
    }

    public void OnWin(Level level)
    {
    }

    public void OnLose(Level level)
    {
    }
}