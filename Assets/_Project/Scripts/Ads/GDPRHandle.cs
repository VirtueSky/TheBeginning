using UnityEngine;
using VirtueSky.Ads;
using VirtueSky.Variables;

public class GDPRHandle : MonoBehaviour
{
    [SerializeField] private Advertising advertisingPrefab;
    [SerializeField] private BooleanVariable isGDPRCanRequestAds;

    private void OnEnable()
    {
    }

    void GDPRHandleRequestAds(bool request)
    {
        if (request)
        {
            Instantiate(advertisingPrefab, this.transform);
        }
    }
}