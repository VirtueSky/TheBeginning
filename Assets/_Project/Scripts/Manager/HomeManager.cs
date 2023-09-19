using UnityEngine;

public class HomeManager : MonoBehaviour
{
    [SerializeField] private LoadSceneEvent loadSceneEvent;


    public void LoadGameScene()
    {
        loadSceneEvent.Raise(new LoadSceneData(false, Constant.GAMEPLAY_SCENE, .1f, null));
    }
}