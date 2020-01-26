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

    public Evaluation bestEvaluation;
    public float bestFitness;

    private int currentStep;
    private bool started = false;

    // Initialize the singleton instance.
    private void Awake()
    {

        // If there is not already an instance of SoundManager, set it to this.
        if (Instance == null)
        {
            bestFitness = -1;
            bestEvaluation = null;
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
        //StartEvolution(GeneticEvolutionConfiguration.GetDefaultConfig(), SumobotIAConfiguration.GetDefaultIAConfig());
    }

    public void StartEvolution(GeneticEvolutionConfiguration config,SumobotIAConfiguration iaConfig)
    {
        started = true;
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
        Debug.Log("NEXT GENERATION: " + currentStep);
        if (currentStep >= config.maxSteps)
        {
            OnFinish();
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            currentStep = config.maxSteps;
        }
    }

    public void OnFinish()
    {
        SaveConfigMenuController.Instance.Show(true);
    }

    public void EndGeneration(string fileName,bool save)
    {
        if (save)
        {
            SumobotIAConfiguration config = SumobotIAConfiguration.Copy(iaConfig);
            config.weights = currentGeneration.GetBestFitnessEval().GetEvaluation();
            ConfigurationManager.Instance.SaveConfig(fileName, config);            
        }
        CombatManager.Instance.Clear();
        Destroy(currentGeneration.gameObject);
    }
}
