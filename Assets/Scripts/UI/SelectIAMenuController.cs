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

    private bool testMode;

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
        UpdateButtonList();
    }

    public void UpdateButtonList()
    {
        ClearButtons();
        List<string> files = ConfigurationManager.Instance.GetFileNameList();

        foreach (string str in files)
        {
            AddButton(str);
        }
    }

    private void ClearButtons()
    {
        int count = contentPanel.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            Destroy(contentPanel.transform.GetChild(i).gameObject);
        }
    }

    private void AddButton(string name)
    {
        GameObject newButton = Instantiate(buttonPrefab) as GameObject;
        newButton.transform.SetParent(contentPanel.transform, false);

        Button b = newButton.GetComponent<Button>();
        b.onClick.AddListener(() => OnButtonPressed(name));

        Text t = newButton.transform.GetChild(0).GetComponent<Text>();
        t.text = name;
        
    }

    public void Show(bool b,bool testMode)
    {
        this.testMode = testMode;
        panel.SetActive(b);
    }

    public void Show(bool b)
    {
        panel.SetActive(b);
    }

    public void OnButtonPressed(string name)
    {
        Show(false);
        if (testMode)
        {
            TestIAMenuController.Instance.Show(true,name);
        }
        else
        {
            CombatVsIAManager.Instance.StartMatch(name);
        }
        
    }

}
