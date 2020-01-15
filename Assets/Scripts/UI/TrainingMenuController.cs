using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingMenuController : MonoBehaviour
{
    public GameObject panel;

    public static TrainingMenuController Instance = null;

    // Initialize the singleton instance.
    private void Awake()
    {

        // If there is not already an instance of SoundManager, set it to this.
        if (Instance == null)
        {
            Instance = this;          
        }
        //If an instance already exists, destroy whatever this object is to enforce the singleton.
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        //Dont destroy on load
        DontDestroyOnLoad(gameObject);
    }


    public void Show(bool b)
    {
        panel.SetActive(b);
    }

    
}
