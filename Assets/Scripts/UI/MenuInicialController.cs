using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInicialController : MonoBehaviour
{
    public GameObject panel;

    public static MenuInicialController Instance = null;


    public void Show(bool b)
    {
        panel.SetActive(b);
    }

    public void OnTrainingButtonPressed()
    {
        //TODO
    }

    public void OnCombatButtonPressed()
    {
        //TODO
    }
}
