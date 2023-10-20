using Sirenix.OdinInspector;
using UnityEngine;
using VirtueSky.Component;
using VirtueSky.Misc;

public class BonusArrowHandler : MonoBehaviour
{
    [ReadOnly] public AreaItem CurrentAreaItem;
    public MoveComponent MoveObject => GetComponent<MoveComponent>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BonusArea"))
        {
            CurrentAreaItem = other.GetComponent<AreaItem>();
            CurrentAreaItem.ActivateBorderLight();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("BonusArea"))
        {
            other.GetComponent<AreaItem>().DeActivateBorderLight();
        }
    }
}