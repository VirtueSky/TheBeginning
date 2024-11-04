using System;
using UnityEngine;
using VirtueSky.DataStorage;
using VirtueSky.Events;
using VirtueSky.Inspector;

[EditorIcon("icon_controller"), HideMonoScript]
public class CoinSystem : MonoBehaviour
{
    [SerializeField] private Vector3Event generateCoinEvent;
    [SerializeField] private EventNoParam addCoinEvent;
    [SerializeField] private EventNoParam minusCoinEvent;
    private static event Action<int, Vector3> OnAddCoinEvent;
    private static event Action<int> OnMinusCoinEvent;
    private static event Action<int, Vector3> OnSetCoinEvent;
    private const string CURRENT_COIN = "CURRENT_COIN";

    private static int CurrentCoin
    {
        get => GameData.Get(CURRENT_COIN, 0);
        set
        {
            GameData.Set(CURRENT_COIN, value);
            GameData.Save();
        }
    }

    private void Awake()
    {
        OnAddCoinEvent += InternalAddCoin;
        OnMinusCoinEvent += InternalMinusCoin;
        OnSetCoinEvent += InternalSetCoin;
    }

    private void OnDestroy()
    {
        OnAddCoinEvent -= InternalAddCoin;
        OnMinusCoinEvent -= InternalMinusCoin;
        OnSetCoinEvent -= InternalSetCoin;
    }

    private void InternalSetCoin(int value, Vector3 posGenerateCoin)
    {
        if (value > CurrentCoin)
        {
            CurrentCoin = value;
            if (posGenerateCoin != default)
            {
                generateCoinEvent.Raise(posGenerateCoin);
            }

            addCoinEvent.Raise();
        }
        else if (value < CurrentCoin)
        {
            CurrentCoin = value;
            minusCoinEvent.Raise();
        }
    }

    private void InternalAddCoin(int value, Vector3 posGenerateCoin)
    {
        CurrentCoin += value;
        if (posGenerateCoin != default)
        {
            generateCoinEvent.Raise(posGenerateCoin);
        }

        addCoinEvent.Raise();
    }

    private void InternalMinusCoin(int value)
    {
        CurrentCoin -= value;
        minusCoinEvent.Raise();
    }

    #region Api

    public static void AddCoin(int value, Vector3 posGenerateCoin = default) =>
        OnAddCoinEvent?.Invoke(value, posGenerateCoin);

    public static void MinusCoin(int value) => OnMinusCoinEvent?.Invoke(value);

    public static void SetCoin(int value, Vector3 posGenerateCoin = default) =>
        OnSetCoinEvent?.Invoke(value, posGenerateCoin);

    public static int GetCurrentCoin() => CurrentCoin;

    #endregion
}