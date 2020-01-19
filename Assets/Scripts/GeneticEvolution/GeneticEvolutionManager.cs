using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticEvolutionManager : MonoBehaviour
{
    public static GeneticEvolutionManager Instance = null;

    public GeneticEvolutionConfiguration config;
    public SumobotIAConfiguration iaConfig;

    public GameObject generationPrefab;
    private Generation currentGeneration;

    private int currentStep;

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
        StartEvolution(GeneticEvolutionConfiguration.GetDefaultConfig(), SumobotIAConfiguration.GetDefaultIAConfig());
    }

    public void StartEvolution(GeneticEvolutionConfiguration config,SumobotIAConfiguration iaConfig)
    {
        this.config = config;
        this.iaConfig = iaConfig;
        currentStep = 1;

        SpawnInitialGeneration();

    }

    private void SpawnInitialGeneration()
    {
        GameObject go = Instantiate(generationPrefab, transform);
        currentGeneration = go.GetComponent<Generation>();
        currentGeneration.CreateInitialGeneration(config, iaConfig);
    }

    public void SpawnNextGeneration()
    {
        currentStep++;
        if(currentStep > config.maxSteps)
        {
            //TODO END
            Debug.Log("FINISH GENETIC EVOLUTION");
        }
        else
        {
            GameObject go = Instantiate(generationPrefab, transform);
            Generation g = go.GetComponent<Generation>();
            g.CreateGenerationFromPrevious(currentGeneration);
            Destroy(currentGeneration.gameObject);
            currentGeneration = g;
        }     
        



    }
}
