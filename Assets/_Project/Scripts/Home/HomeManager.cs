using TheBeginning.AppControl;
using UnityEngine;


public class HomeManager : MonoBehaviour
{
    private void Start()
    {
        AppControlPopup.Show<PopupHome>();
    }
}