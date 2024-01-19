using UnityEngine;


public class HomeManager : MonoBehaviour
{
    [SerializeField] private PopupVariable popupVariable;

    private void Start()
    {
        popupVariable.Value.Show<PopupHome>();
    }
}