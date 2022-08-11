using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armoury : MonoBehaviour
{

    public GameObject ArmouryPanel;

    public void Armourys()
    {
        ArmouryPanel.SetActive(true);
        //if (ArmouryPanel != null)
        //{
        //    bool isActive = ArmouryPanel.activeSelf;
        //    ArmouryPanel.SetActive(!isActive);
        //}
    }
    public void BackArmourys()
    {
        ArmouryPanel.SetActive(false);
    }
}
