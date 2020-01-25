using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatVsIAManager : MonoBehaviour
{
    private SumobotIAConfiguration currentConfig;
    private CombatController currentCombat;
    public static CombatVsIAManager Instance = null;

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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetMatch();
        }
    }

    public void StartMatch(string name)
    {
        currentConfig = ConfigurationManager.Instance.GetConfigByName(name);
        currentCombat = CombatManager.Instance.CreatePlayerVsIACombat(new Vector3(0, 0, 0), SumobotIAConfiguration.Copy(currentConfig), 
            SumobotIAConfiguration.Copy(currentConfig));
    }

    private void ResetMatch()
    {
        if(currentCombat != null)
        {
            Destroy(currentCombat.gameObject);
        }

        currentCombat = CombatManager.Instance.CreatePlayerVsIACombat(new Vector3(0, 0, 0), SumobotIAConfiguration.Copy(currentConfig),
            SumobotIAConfiguration.Copy(currentConfig));
    }
}
