using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VirtueSky.Core;

public class LoadingManager : BaseMono
{
    [FoldoutGroup("Attributes")] public Image progressBar;
    [FoldoutGroup("Attributes")] public TextMeshProUGUI loadingText;

    [FoldoutGroup("Attributes")] [Range(0.1f, 10f)]
    public float timeLoading = 5f;

    [FoldoutGroup("Attributes")] [SerializeField]
    private LoadSceneEvent loadSceneEvent;

    [FoldoutGroup("Attributes")] [SerializeField]
    private List<BaseMono> listObjSpawn = new List<BaseMono>();

    private bool _flagDoneProgress;
    private bool firebaseIsInitialized = false;

    void Start()
    {
        foreach (var mono in listObjSpawn)
        {
            Instantiate(mono);
            mono.Initialize();
        }

        Initialize();
    }

    public override void Initialize()
    {
        base.Initialize();
        progressBar.fillAmount = 0;
        progressBar.DOFillAmount(5, timeLoading)
            .OnUpdate(() => loadingText.text = $"Loading... {(int)(progressBar.fillAmount * 100)}%")
            .OnComplete(() => _flagDoneProgress = true);
        loadSceneEvent.Raise(new LoadSceneData(true, Constant.HOME_SCENE, timeLoading,
            () => _flagDoneProgress && firebaseIsInitialized));
    }

    public void FirebaseIsInitialized()
    {
        firebaseIsInitialized = true;
    }
}