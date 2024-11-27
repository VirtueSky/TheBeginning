using System;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using VirtueSky.Inspector;

namespace TheBeginning.Config
{
    [CreateAssetMenu(fileName = "DailyRewardConfig", menuName = "Config/DailyRewardConfig")]
    public class DailyRewardConfig : ScriptableObject
    {
        [SerializeField] private List<DailyRewardData> listDailyRewardData;
        [SerializeField] private List<DailyRewardData> listDailyRewardDataLoop;
        public List<DailyRewardData> ListDailyRewardData => listDailyRewardData;
        public List<DailyRewardData> ListDailyRewardDataLoop => listDailyRewardDataLoop;

#if UNITY_EDITOR

        [Header("Set Data"), SerializeField] private bool enableSetData;

        [ShowIf(nameof(enableSetData)), SerializeField]
        private int totalData;

        [ShowIf(nameof(enableSetData)), SerializeField]
        private List<int> listCoinValue;


        [Header("Set Data Loop"), SerializeField]
        private bool enableSetDataLoop;

        [ShowIf(nameof(enableSetDataLoop)), SerializeField]
        private int totalDataLoop;

        [ShowIf(nameof(enableSetDataLoop)), SerializeField]
        private int coinLoopValue;

        [ShowIf(nameof(ConditionShowButton)), SerializeField]
        private List<Sprite> listIconCoin;

        [ShowIf(nameof(ConditionShowButton)), Button]
        private void SetData()
        {
            if (enableSetDataLoop)
            {
                listDailyRewardDataLoop.Clear();
                for (int i = 0; i < totalDataLoop; i++)
                {
                    if (i < 7)
                    {
                        DailyRewardData data = new DailyRewardData(listIconCoin[i], coinLoopValue);
                        listDailyRewardDataLoop.Add(data);
                    }
                    else
                    {
                        DailyRewardData data = new DailyRewardData(listIconCoin[i % 7], coinLoopValue);
                        listDailyRewardDataLoop.Add(data);
                    }
                }
            }

            if (enableSetData)
            {
                listDailyRewardData.Clear();
                for (int i = 0; i < totalData; i++)
                {
                    if (i < 7)
                    {
                        DailyRewardData data = new DailyRewardData(listIconCoin[i], listCoinValue[i]);
                        listDailyRewardData.Add(data);
                    }
                    else
                    {
                        DailyRewardData data = new DailyRewardData(listIconCoin[i % 7], listCoinValue[i % 7]);
                        listDailyRewardData.Add(data);
                    }
                }
            }

            EditorUtility.SetDirty(this);
        }

        private bool ConditionShowButton => enableSetData || enableSetDataLoop;
#endif
    }

    [Serializable]
    public class DailyRewardData
    {
        public DailyRewardType dailyRewardType;
        public Sprite icon;

        [ShowIf(nameof(dailyRewardType), DailyRewardType.Skin)]
        public string skinID;

        [ShowIf(nameof(dailyRewardType), DailyRewardType.Coin)]
        public int value;

        public DailyRewardData(Sprite _icon, int _value)
        {
            this.dailyRewardType = DailyRewardType.Coin;
            this.icon = _icon;
            this.value = _value;
            this.skinID = "";
        }
    }

    public enum DailyRewardType
    {
        Coin,
        Skin,
    }
}