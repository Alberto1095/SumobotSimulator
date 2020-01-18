using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainingMenuController : MonoBehaviour
{
    public GameObject panel;

    //Menu
    public GameObject[] menus;
    private int currentMenuIndex;

    //Neural network config
    public InputField inputNumInputs;
    public InputField inputNumLevels;
    public InputField inputNumLayersInLevel;
    public Dropdown inputMiddleLayerFunction;
    public Dropdown inputOutputLayerFunction;


    //Sumobot config
    public InputField inputMoveSpeed;
    public InputField inputRotationSpeed;
    public InputField inputLineSensorDistance;
    public InputField inputDistanceSensorDistance;
    public Toggle inputUseFrontDistanceSensor;
    public Toggle inputUseLeftDistanceSensor;
    public Toggle inputUseRightDistanceSensor;
    public Toggle inputUseFrontLineSensor;
    public Toggle inputUseFrontRightLineSensor;
    public Toggle inputUseFrontLeftLineSensor;
    public Toggle inputUseBackLineSensor;

    //Genetic config
    public InputField inputMaxSteps;
    public InputField inputInitialPopulation;
    public Dropdown inputSelectionFunction;
    public Toggle inputUseElitism;
    public Toggle inputUseMutation;
    public Dropdown inputMutationFunction;
    public InputField inputMutationChance;
    public Dropdown inputCrossFunction;
    public InputField inputCrossChance;


    public static TrainingMenuController Instance = null;

    // Initialize the singleton instance.
    private void Awake()
    {

        // If there is not already an instance of SoundManager, set it to this.
        if (Instance == null)
        {
            currentMenuIndex = -1;
            ChangeMenu(0);
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

    public void ChangeMenu(int index)
    {
        if(currentMenuIndex != -1)
        {
            menus[currentMenuIndex].SetActive(false);
        }
        currentMenuIndex = index;
        menus[currentMenuIndex].SetActive(true);
        GetAIConfig();
    }

    private SumobotIAConfiguration GetAIConfig()
    {
        SumobotIAConfiguration config = new SumobotIAConfiguration();

        config.moveSpeed = float.Parse(inputMoveSpeed.text);
        config.rotationSpeed = float.Parse(inputRotationSpeed.text);
        config.lineSensorDistance = float.Parse(inputLineSensorDistance.text);
        config.distanceSensorDistance = float.Parse(inputDistanceSensorDistance.text);
        config.useFrontDistanceSensor = inputUseFrontDistanceSensor.isOn;
        config.useRightDistanceSensor = inputUseRightDistanceSensor.isOn;
        config.useLeftDistanceSensor = inputUseLeftDistanceSensor.isOn;
        config.useFrontLineSensor = inputUseFrontLineSensor.isOn;
        config.useFrontRightLineSensor = inputUseFrontRightLineSensor.isOn;
        config.useFrontLeftLineSensor = inputUseFrontLeftLineSensor.isOn;
        config.useBackLineSensor = inputUseBackLineSensor.isOn;

        config.numLevels = int.Parse(inputNumLevels.text);
        config.numInputs = int.Parse(inputNumInputs.text);

        List<int> numLayersPerLevel = new List<int>();
        string str = inputNumLayersInLevel.text;
        string[] l = str.Split(',');
        foreach(string c in l)
        {
            numLayersPerLevel.Add(int.Parse(c));
        }       
        config.numLayersPerLevel = numLayersPerLevel;
        ActivationFunctions func = new ActivationFunctions();
        func.middleLayersFunction = (ActivationFunction)System.Enum.Parse(typeof(ActivationFunction),
            inputMiddleLayerFunction.options[inputMiddleLayerFunction.value].text);
        func.finalLayerFunction = (ActivationFunction)System.Enum.Parse(typeof(ActivationFunction),
            inputOutputLayerFunction.options[inputOutputLayerFunction.value].text);

        config.weights = null;


        return config;
    }

    private GeneticEvolutionConfiguration GetGeneticEvolutionConfiguration()
    {
        GeneticEvolutionConfiguration config = new GeneticEvolutionConfiguration();
        config.maxSteps = int.Parse(inputMaxSteps.text);
        config.population = int.Parse(inputInitialPopulation.text);
        config.selectionFunction = (SelectionFunction)System.Enum.Parse(typeof(SelectionFunction), 
            inputSelectionFunction.options[inputSelectionFunction.value].text);
        config.useElitismo = inputUseElitism.isOn;
        config.useMutation = inputUseMutation.isOn;
        config.mutationFunction = (MutationFunction)System.Enum.Parse(typeof(MutationFunction),
            inputMutationFunction.options[inputMutationFunction.value].text);
        config.mutationChance = float.Parse(inputMutationChance.text);
        config.crossFunction = (CrossFunction)System.Enum.Parse(typeof(CrossFunction),
            inputCrossFunction.options[inputCrossFunction.value].text);
        config.crossChance = float.Parse(inputCrossChance.text);

        return config;
    }

    public void OnBackPressed()
    {
        Show(false);
        StartingMenuController.Instance.Show(true);
    }

    public void OnStartPressed()
    {

    }

    
}
