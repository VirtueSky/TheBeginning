using System;
using UnityEngine;
using VirtueSky.Core;

public class Block : MonoBehaviour
{
    [SerializeField] private GameObject buttonCloseBlock;
    [SerializeField] private float timeDelayShowButtonClose;

    private void OnEnable()
    {
        buttonCloseBlock.SetActive(false);
        App.Delay(this, timeDelayShowButtonClose, () => { buttonCloseBlock.SetActive(true); });
    }
}