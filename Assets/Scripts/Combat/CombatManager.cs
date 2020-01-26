using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public List<CombatController> combatControllers;

    //Configuration variables
    public GameObject sumobotPlayerPrefab;
    public GameObject sumobotIAPrefab;
    public GameObject combatAreaPrefab;

    //Singleton
    public static CombatManager Instance = null;

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

   

    public void SpawnGeneration(List<Evaluation> list, SumobotIAConfiguration configSumobot, GeneticEvolutionConfiguration config,Evaluation bestEval)
    {
        Clear();
        int startX = 0;
        int startY = 0;
        int offset = 20;
        int maxRow = 6;
        int rowCount = 0;
        Vector3 pos;
        int count = config.population;
        SumobotIAConfiguration c1, c2;
        for (int i = 0; i < count; i++)
        {
            pos = new Vector3(startX, startY, 0);
            startX += offset;
            rowCount++;
            if (rowCount >= maxRow)
            {
                rowCount = 0;
                startY -= offset;
                startX = 0;
            }
            c1 = SumobotIAConfiguration.Copy(configSumobot);
            c1.weights = list[i].GetEvaluation();
            c2 = SumobotIAConfiguration.Copy(configSumobot);
            c2.weights = new List<float>(bestEval.GetEvaluation());

            CombatController match = CreateIAvsIACombat(pos, c1, c2);
            combatControllers.Add(match);
        }
    }

    public void SpawnRandomGeneration(SumobotIAConfiguration configSumobot, GeneticEvolutionConfiguration config)
    {
        Clear();        
        int startX = 0;
        int startY = 0;
        int offset = 25;
        int maxRow = 6;
        int rowCount = 0;
        Vector3 pos;
        int count = config.population;  
       
        for (int i = 0; i < count; i++)
        {
            pos = new Vector3(startX, startY, 0);
            startX += offset;
            rowCount++;
            if (rowCount >= maxRow)
            {
                rowCount = 0;
                startY -= offset;
                startX = 0;
            }
            CombatController match = CreateIAvsIACombat(pos, SumobotIAConfiguration.Copy(configSumobot), SumobotIAConfiguration.Copy(configSumobot));
            combatControllers.Add(match);
        }
    }

    public void Clear()
    {
        if(combatControllers != null)
        {
            foreach (CombatController cc in combatControllers)
            {
                Destroy(cc.gameObject);
            }
        }

        combatControllers = new List<CombatController>();
        
    }

    public CombatController CreatePlayerVsIACombat(Vector3 position, SumobotConfiguration sumobotConfig, SumobotIAConfiguration iaConfig)
    {
        GameObject go = Instantiate(combatAreaPrefab, position, Quaternion.identity, transform);

        GameObject r1 = Instantiate(sumobotPlayerPrefab, go.transform);
        RobotController rc1 = r1.GetComponent<RobotController>();
        rc1.SetConfig(sumobotConfig);

        GameObject r2 = Instantiate(sumobotIAPrefab, go.transform);
        RobotController rc2 = r2.GetComponent<RobotController>();
        RobotIAController ia = (RobotIAController)rc2;
        ia.SetConfig(iaConfig);


        CombatController controller = go.GetComponent<CombatController>();
        controller.StartMatch(rc1, rc2);

        return controller;
    }

    public CombatController CreateIAvsIACombat(Vector3 position, SumobotIAConfiguration iaConfig1, SumobotIAConfiguration iaConfig2)
    {
        GameObject go = Instantiate(combatAreaPrefab, position, Quaternion.identity, transform);

        GameObject r1 = Instantiate(sumobotIAPrefab, go.transform);
        RobotController rc1 = r1.GetComponent<RobotController>();
        RobotIAController ia1 = (RobotIAController)rc1;
        ia1.SetConfig(iaConfig1);

        GameObject r2 = Instantiate(sumobotIAPrefab, go.transform);
        RobotController rc2 = r2.GetComponent<RobotController>();
        RobotIAController ia2 = (RobotIAController)rc2;
        ia2.SetConfig(iaConfig2);

        CombatController controller = go.GetComponent<CombatController>();
        controller.StartMatch(rc1, rc2);

        return controller;
    }


}
