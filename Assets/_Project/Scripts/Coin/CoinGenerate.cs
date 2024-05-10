using System;
using System.Collections.Generic;
using System.Linq;
using PrimeTween;
using UnityEngine;
using VirtueSky.Audio;
using VirtueSky.Core;
using VirtueSky.Events;
using VirtueSky.Threading.Tasks;
using VirtueSky.Variables;
using Random = UnityEngine.Random;

public class CoinGenerate : BaseMono
{
    [SerializeField] private Transform holder;
    [SerializeField] private int numberCoin;
    [SerializeField] private int delay;
    [SerializeField] private float durationNear;
    [SerializeField] private float durationTarget;
    [SerializeField] private Ease easeNear;
    [SerializeField] private Ease easeTarget;
    [SerializeField] private float scale = 1;
    [SerializeField] private float offsetNear = 1;
    [SerializeField] private Vector3Event setFromCoinEvent;
    [SerializeField] private AddTargetToCoinGenerateEvent addTargetToCoinGenerateEvent;
    [SerializeField] private RemoveTargetToCoinGenerateEvent removeTargetToCoinGenerateEvent;
    [SerializeField] private EventNoParam moveOneCoinDone;
    [SerializeField] private EventNoParam moveAllCoinDone;
    [SerializeField] private EventNoParam decreaseCoinEvent;
    [SerializeField] private IntegerVariable currentCoin;
    [SerializeField] private CoinPool coinPool;
    [Header("Sound")] [SerializeField] public PlaySfxEvent playSoundFx;
    [SerializeField] private SoundData soundCoinMove;

    private bool isScaleIconTo = false;
    private Vector3 from;
    private GameObject to;
    private List<GameObject> coinsActive = new List<GameObject>();
    private List<GameObject> listTo = new List<GameObject>();
    private int cacheCurrentCoin;

    public override void OnEnable()
    {
        base.OnEnable();
        currentCoin.AddListener(HandleGenerateCoin);
        setFromCoinEvent.AddListener(SetFrom);
        addTargetToCoinGenerateEvent.AddListener(AddTo);
        removeTargetToCoinGenerateEvent.AddListener(RemoveTo);
        SetFrom(holder.position);
        SaveCache();
    }

    public override void OnDisable()
    {
        base.OnDisable();
        currentCoin.RemoveListener(HandleGenerateCoin);
        setFromCoinEvent.RemoveListener(SetFrom);
        addTargetToCoinGenerateEvent.RemoveListener(AddTo);
        removeTargetToCoinGenerateEvent.RemoveListener(RemoveTo);
    }

    private void SaveCache()
    {
        cacheCurrentCoin = currentCoin.Value;
    }

    private void HandleGenerateCoin(int value)
    {
        if (currentCoin.Value > cacheCurrentCoin)
        {
            GenerateCoin();
        }
        else
        {
            decreaseCoinEvent.Raise();
            SaveCache();
        }
    }

    public void SetFrom(Vector3 from)
    {
        this.from = from;
    }

    public void AddTo(GameObject obj)
    {
        listTo.Add(obj);
    }

    public void RemoveTo(GameObject obj)
    {
        listTo.Remove(obj);
    }

    private GameObject GetTo()
    {
        return listTo.Last();
    }


    public async void GenerateCoin()
    {
        isScaleIconTo = false;
        for (int i = 0; i < numberCoin; i++)
        {
            await UniTask.Delay(Random.Range(0, delay));
            GameObject coin = coinPool.Spawn(holder);
            coin.transform.localScale = Vector3.one * scale;
            coinsActive.Add(coin);
            coin.transform.position = from;

            MoveToTarget(coin, () =>
            {
                coinsActive.Remove(coin);
                coinPool.DeSpawn(coin);
                if (!isScaleIconTo)
                {
                    isScaleIconTo = true;
                    playSoundFx.Raise(soundCoinMove);
                    ScaleIconTo();
                }

                moveOneCoinDone.Raise();
                if (coinsActive.Count == 0)
                {
                    moveAllCoinDone.Raise();
                    SaveCache();
                    SetFrom(holder.position);
                }
            });
        }
    }

    private void MoveToTarget(GameObject coin, Action completed)
    {
        coin.transform
            .DOMove(coin.transform.position + (Vector3)Random.insideUnitCircle * offsetNear,
                durationNear)
            .SetEase(easeNear)
            .OnComplete(
                () =>
                {
                    coin.transform.DOMove(GetTo().transform.position, durationTarget).SetEase(easeTarget)
                        .OnComplete(completed);
                });
    }

    public void SetNumberCoin(int _numberCoin)
    {
        numberCoin = _numberCoin;
    }

    private void ScaleIconTo()
    {
        Vector3 currentScale = Vector3.one;
        Vector3 nextScale = currentScale + new Vector3(.1f, .1f, .1f);
        GetTo().transform.DOScale(nextScale, durationTarget).SetEase(Ease.OutBack)
            .OnComplete((() => { GetTo().transform.DOScale(currentScale, durationTarget / 2).SetEase(Ease.InBack); }));
    }
}