using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PrimeTween;
using UnityEngine;
using VirtueSky.Core;
using Random = UnityEngine.Random;

public class CurrencyGenerate : BaseMono
{
    [SerializeField] private Vector3 from = Vector3.zero;
    [SerializeField] private GameObject to;
    [SerializeField] private int numberCoin;
    [SerializeField] private int delay;
    [SerializeField] private float durationNear;
    [SerializeField] private float durationTarget;
    [SerializeField] private Ease easeNear;
    [SerializeField] private Ease easeTarget;
    [SerializeField] private float scale = 1;
    [SerializeField] private float offsetNear = 1;
    [SerializeField] private CurrencyPool currencyPool;

    private System.Action moveOneCoinDone;
    private bool isScaleIconTo = false;

    private List<GameObject> coinsActive = new List<GameObject>();

    public void SetFrom(Vector3 from)
    {
        this.from = from;
    }

    public void SetToGameObject(GameObject to)
    {
        this.to = to;
    }

    private void Start()
    {
    }

    public async void GenerateCoin(System.Action moveOneCoinDone, System.Action moveAllCoinDone,
        GameObject to = null,
        int numberCoin = -1)
    {
        isScaleIconTo = false;
        this.moveOneCoinDone = moveOneCoinDone;
        //this.moveAllCoinDone = moveAllCoinDone;
        this.to = to == null ? this.to : to;
        this.numberCoin = numberCoin < 0 ? this.numberCoin : numberCoin;

        for (int i = 0; i < this.numberCoin; i++)
        {
            await Task.Delay(Random.Range(0, delay));
            GameObject coin = currencyPool.Spawn(transform);
            coin.transform.localScale = Vector3.one * scale;
            coinsActive.Add(coin);
            coin.transform.position = from;
            MoveCoin(coin, moveAllCoinDone);
            // if (i == numberCoin - 1)
            // {
            //     Observer.CoinMove?.Invoke();
            // }
        }
    }

    private void MoveCoin(GameObject coin, System.Action moveAllCoinDone)
    {
        MoveToTarget(coin, () =>
        {
            coinsActive.Remove(coin);
            currencyPool.DeSpawn(coin);
            if (!isScaleIconTo)
            {
                isScaleIconTo = true;
                ScaleIconTo();
            }

            moveOneCoinDone?.Invoke();
            if (coinsActive.Count == 0)
            {
                moveAllCoinDone?.Invoke();

                from = Vector3.zero;
            }
        });
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
                    coin.transform.DOMove(to.transform.position, durationTarget).SetEase(easeTarget)
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
        to.transform.DOScale(nextScale, durationTarget).SetEase(Ease.OutBack)
            .OnComplete((() => { to.transform.DOScale(currentScale, durationTarget / 2).SetEase(Ease.InBack); }));
    }
}