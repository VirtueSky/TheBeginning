using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaItem : MonoBehaviour
{
    public int MultiBonus = 1;
    public GameObject BoderLight;

    public void ActivateBorderLight()
    {
        BoderLight.SetActive(true);
    }

    public void DeActivateBorderLight()
    {
        BoderLight.SetActive(false);
    }
}