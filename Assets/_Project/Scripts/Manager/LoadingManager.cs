using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VirtueSky.Inspector;
using VirtueSky.Core;

public class LoadingManager : BaseMono
{
    [HeaderLine("Attributes")] public Image progressBar;
    public TextMeshProUGUI loadingText;

    [Range(0.1f, 10f)] public float timeLoading = 5f;

    [SerializeField] private LoadSceneEvent loadSceneEvent;

    [SerializeField] private List<BaseMono> listObjSpawn = new List<BaseMono>();

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
            () => _flagDoneProgress));
    }

    public void FirebaseIsInitialized()
    {
        firebaseIsInitialized = true;
    }
}