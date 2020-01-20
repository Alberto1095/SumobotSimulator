using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectIAMenuController : MonoBehaviour
{
    public GameObject panel;
    public GameObject contentPanel;
    public GameObject buttonPrefab;

    public static SelectIAMenuController Instance = null;

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

    public void Start()
    {
        List<string> files = ConfigurationManager.Instance.GetFileNameList();

        foreach(string str in files)
        {
            AddButton(str);
        }

    }

    private void AddButton(string name)
    {
        GameObject newButton = Instantiate(buttonPrefab) as GameObject;
        newButton.transform.SetParent(contentPanel.transform, false);

        Button b = newButton.GetComponent<Button>();
        b.onClick.AddListener(() => OnStartMatchPressed(name));

        Text t = newButton.transform.GetChild(0).GetComponent<Text>();
        t.text = name;
        
    }

    public void Show(bool b)
    {
        panel.SetActive(b);
    }

    public void OnStartMatchPressed(string name)
    {
        Show(false);
        
        CombatVsIAManager.Instance.StartMatch(name);
    }

}
