using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //Configuration variables
    public GameObject sumobotPlayerPrefab;
    public GameObject sumobotIAPrefab;
    public GameObject combatAreaPrefab;

    //Singleton
    public static Spawner Instance = null;

    private GameObject currentMatch;

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

    private void Start()
    {
        InvokeRepeating("SpawnRandom", 0, 4);
    }

    public void SpawnRandom()
    {
        Destroy(currentMatch);
        SumobotConfiguration config = SumobotConfiguration.GetDefaultPlayerConfig();
        Vector3 pos = new Vector3(0, 0, 0);
        SumobotIAConfiguration ia = SumobotIAConfiguration.GetDefaultIAConfig();


        currentMatch = CreatePlayerVsIACombat(pos, config, ia);
    }
 

    public GameObject CreatePlayerVsIACombat(Vector3 position,SumobotConfiguration sumobotConfig, SumobotIAConfiguration iaConfig)
    {
        GameObject go = Instantiate(combatAreaPrefab, position, Quaternion.identity);
        
        GameObject r1 = Instantiate(sumobotPlayerPrefab, go.transform);
        RobotController rc1 = r1.GetComponent<RobotController>();
        rc1.SetConfig(sumobotConfig);

        GameObject r2 = Instantiate(sumobotIAPrefab, go.transform);
        RobotController rc2 = r2.GetComponent<RobotController>();
        RobotIAController ia = (RobotIAController)rc2;
        ia.SetConfig(iaConfig);
      

        CombatController controller = go.GetComponent<CombatController>();
        controller.StartMatch(rc1, rc2);

        return go;
    }

    public GameObject CreateIAvsIACombat(Vector3 position, SumobotIAConfiguration iaConfig1, SumobotIAConfiguration iaConfig2)
    {
        GameObject go = Instantiate(combatAreaPrefab, position, Quaternion.identity);

        GameObject r1 = Instantiate(sumobotPlayerPrefab, go.transform);
        RobotController rc1 = r1.GetComponent<RobotController>();
        RobotIAController ia1 = (RobotIAController)rc1;
        ia1.SetConfig(iaConfig1);

        GameObject r2 = Instantiate(sumobotPlayerPrefab, go.transform);
        RobotController rc2 = r2.GetComponent<RobotController>();
        RobotIAController ia2 = (RobotIAController)rc2;
        ia2.SetConfig(iaConfig2);

        CombatController controller = go.GetComponent<CombatController>();
        controller.StartMatch(rc1, rc2);

        return go;
    }
}
