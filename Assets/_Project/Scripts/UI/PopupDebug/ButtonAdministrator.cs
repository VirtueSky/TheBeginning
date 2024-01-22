using UnityEngine;
using VirtueSky.Core;

public class ButtonAdministrator : BaseMono
{
    public PopupVariable popupVariable;
    public int maxCount = 5;
    private int countClick = 0;
    private int swipeCount = 0;
    private Vector2 startTouchPosition;
    private float minSwipeDistance = 400f;
    private bool isShow = false;

    private void OnEnable()
    {
        InvokeRepeating(nameof(ResetCount), 0, 2f);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(ResetCount));
    }

    private void OnMouseDown()
    {
        countClick++;
        if (countClick >= maxCount)
        {
            countClick = 0;
            popupVariable.Value.Show<PopupAdministrator>(false);
        }
    }

    void ResetCount()
    {
        countClick = 0;
    }
}