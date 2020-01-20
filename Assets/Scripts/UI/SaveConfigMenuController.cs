using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveConfigMenuController : MonoBehaviour
{
    public InputField inputName;
    public GameObject panel;

    public static SaveConfigMenuController Instance = null;

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

    public void OnSavePressed()
    {
        string name = inputName.text;
        if(name.Length > 0)
        {
            GeneticEvolutionManager.Instance.EndGeneration(name, true);
        }
        Show(false);
        StartingMenuController.Instance.Show(true);
    }

    public void OnCancelPressed()
    {
        Show(false);
        GeneticEvolutionManager.Instance.EndGeneration("", false);
        StartingMenuController.Instance.Show(true);
    }

   
}
