using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using PrimeTween;
using UnityEngine;
using VirtueSky.Audio;
using VirtueSky.Core;
using VirtueSky.Events;
using VirtueSky.Inspector;
using VirtueSky.ObjectPooling;
using VirtueSky.Variables;
using Random = UnityEngine.Random;

public class CoinGenerate : BaseMono
{
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private Transform holder;
    [SerializeField] private int numberCoin;
    [SerializeField] private float delayNear;
    [SerializeField] private float delayTarget;
    [SerializeField] private float durationNear;
    [SerializeField] private float durationTarget;
    [SerializeField] private Ease easeNear;
    [SerializeField] private Ease easeTarget;
    [SerializeField] private float scale = 1;
    [SerializeField] private float offsetNear = 1;
    [HeaderLine("Curvy")]
    [SerializeField] float arcHeight = 2.0f; // độ nhấc cong cơ bản
    [SerializeField] float midBoost = 1.4f; // >1 làm cong mạnh ở giữa (khuyến nghị 1.2–1.8)
    [SerializeField] float sideJitter = 0.3f; // lệch ngang nhẹ cho tự nhiên
    [SerializeField] bool rotateAlong = true; // xoay theo tiếp tuyến
    
    [SerializeField] private Vector3Event setFromCoinEvent;
    [SerializeField] private GameObjectEvent addTargetToCoinGenerateEvent;
    [SerializeField] private GameObjectEvent removeTargetToCoinGenerateEvent;
    [SerializeField] private EventNoParam moveOneCoinDone;
    [SerializeField] private EventNoParam moveAllCoinDone;
    [SerializeField] private EventNoParam decreaseCoinEvent;
    [SerializeField] private EventNoParam addCoinEvent;
    [SerializeField] private EventNoParam minusCoinEvent;
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
        addCoinEvent.AddListener(GenerateCoin);
        minusCoinEvent.AddListener(DecreaseCoin);
        setFromCoinEvent.AddListener(SetFrom);
        addTargetToCoinGenerateEvent.AddListener(AddTo);
        removeTargetToCoinGenerateEvent.AddListener(RemoveTo);
        SetFrom(holder.position);
        SaveCache();
    }

    public override void OnDisable()
    {
        base.OnDisable();
        addCoinEvent.RemoveListener(GenerateCoin);
        minusCoinEvent.RemoveListener(DecreaseCoin);
        setFromCoinEvent.RemoveListener(SetFrom);
        addTargetToCoinGenerateEvent.RemoveListener(AddTo);
        removeTargetToCoinGenerateEvent.RemoveListener(RemoveTo);
    }

    private void SaveCache()
    {
        cacheCurrentCoin = CoinSystem.GetCurrentCoin();
    }

    private void DecreaseCoin()
    {
        decreaseCoinEvent.Raise();
        SaveCache();
    }

    public void SetFrom(Vector3 from)
    {
        this.from = from;
    }

    public void AddTo(GameObject obj)
    {
        listTo.Add(obj);
        to = listTo.Last();
    }

    public void RemoveTo(GameObject obj)
    {
        listTo.Remove(obj);
        if (listTo.Count > 0)
        {
            to = listTo.Last();
        }
    }


    private void GenerateCoin()
    {
        isScaleIconTo = false;
        for (int i = 0; i < numberCoin; i++)
        {
            GameObject coin = coinPrefab.Spawn(holder);
            coin.transform.localScale = Vector3.one * scale;
            coinsActive.Add(coin);
            coin.transform.position = from;

            MoveToTarget(coin, () =>
            {
                coinsActive.Remove(coin);
                coin.DeSpawn();
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

     private void MoveToTarget(GameObject currency, Action completed) {
        var t = currency.transform;
        t.DOKill();
        Vector3 start = t.position;

        float rndDelayNear = UnityEngine.Random.Range(0f, delayNear);
        Vector3 nearPoint = start + (Vector3)UnityEngine.Random.insideUnitCircle.normalized *
            UnityEngine.Random.Range(offsetNear * 0.5f, offsetNear);

        App.Delay(this, rndDelayNear, () => {
            t.DOMove(nearPoint, durationNear).SetEase(easeNear).OnComplete(() => {
                float rndDelayTarget = UnityEngine.Random.Range(0f, delayTarget);
                Vector3 end = to.transform.position;

                Vector3 up = Vector3.up;
                Vector3 dir = (end - nearPoint).normalized;
                Vector3 side = Vector3.Cross(up, dir).normalized;

                float arc = arcHeight * UnityEngine.Random.Range(0.9f, 1.1f);
                float sJit = sideJitter * UnityEngine.Random.Range(-1f, 1f);

                Vector3 c1 = Vector3.Lerp(nearPoint, end, 1f / 3f) + up * arc * 0.9f + side * sJit;
                Vector3 c2 = Vector3.Lerp(nearPoint, end, 2f / 3f) + up * arc * 1.1f - side * sJit;

                Vector3 midLine = Vector3.Lerp(nearPoint, end, 0.5f);
                c1 = midLine + (c1 - midLine) * midBoost;
                c2 = midLine + (c2 - midLine) * midBoost;


                App.Delay(this, rndDelayTarget, () => {
                    t.DOScale(currency.transform.localScale * 0.95f, durationTarget * 0.25f).SetLoops(2, LoopType.Yoyo)
                        .SetEase(Ease.OutSine);

                    DOVirtual.Float(0f, 1f, durationTarget, tt => {
                            Vector3 p = CubicBezier(nearPoint, c1, c2, end, tt);
                            t.position = p;

                            if (rotateAlong) {
                                Vector3 v = CubicBezierTangent(nearPoint, c1, c2, end, tt).normalized;
                                if (v.sqrMagnitude > 1e-6f)
                                    t.rotation =
                                        Quaternion.LookRotation(v,
                                            up);
                            }
                        })
                        .SetEase(easeTarget)
                        .OnComplete(() => completed?.Invoke());
                });
            });
        });
    }

    private Vector3 CubicBezier(Vector3 p0, Vector3 c1, Vector3 c2, Vector3 p3, float t) {
        float u = 1f - t;
        return (u * u * u) * p0 + 3f * (u * u) * t * c1 + 3f * u * (t * t) * c2 + (t * t * t) * p3;
    }

    private Vector3 CubicBezierTangent(Vector3 p0, Vector3 c1, Vector3 c2, Vector3 p3, float t) {
        float u = 1f - t;
        return 3f * ((u * u) * (c1 - p0) + 2f * u * t * (c2 - c1) + (t * t) * (p3 - c2));
    }

    public void SetNumberCoin(int _numberCoin)
    {
        numberCoin = _numberCoin;
    }

    private void ScaleIconTo()
    {
        Vector3 currentScale = Vector3.one;
        Vector3 nextScale = currentScale + new Vector3(.1f, .1f, .1f);
        to.transform.DOScale(nextScale, durationTarget).SetEase(Ease.OutBack)
            .OnComplete((() => { to.transform.DOScale(currentScale, durationTarget / 2).SetEase(Ease.InBack); }));
    }
}