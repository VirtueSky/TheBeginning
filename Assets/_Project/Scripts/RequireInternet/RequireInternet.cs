using UnityEngine;
using VirtueSky.Events;

public class RequireInternet : MonoBehaviour
{
    [SerializeField] private BooleanEvent showRequireInternetEvent;

    private void Start()
    {
        Show(false);
        showRequireInternetEvent.AddListener(Show);
    }

    void Show(bool isShow)
    {
        if (gameObject.activeSelf == isShow) return;
        gameObject.SetActive(isShow);
    }
}