using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestIAMenuController : MonoBehaviour
{
    public GameObject panel;
    public InputField inputField;
    public InputField outputField;




    public static TestIAMenuController Instance = null;

    private NeuralNetwork nn;
    private SumobotIAConfiguration config;


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


    public void Show(bool b,string iaNameFile)
    {
        panel.SetActive(b);
        config = ConfigurationManager.Instance.GetConfigByName(iaNameFile);
        nn = new NeuralNetwork(SumobotIAConfiguration.Copy(config));
        GetInputsUsed();
    }

    public void OnCalculateOutputPressed()
    {
        string inputString = inputField.text;
        string[] values = inputString.Split(',');
        float[] inputs = new float[values.Length];
        int index = 0;
        foreach(string s in values)
        {
            inputs[index] = float.Parse(values[index]);
            index++;
        }


        float[] outputs = nn.CalculateOutput(inputs);

        string str = "";
        foreach (float s in outputs)
        {
            
            str += " " + ((s*2) -1) + " ,";
        }

        str = str.Substring(0, str.Length - 1);

        outputField.text = str;
    }

    private void GetInputsUsed()
    {
        string str = "";

        if (config.useFrontDistanceSensor)
        {
            str += "FRONT DISTANCE - ";
        }
        if (config.useLeftDistanceSensor)
        {
            str += "LEFT DISTANCE - ";
        }
        if (config.useRightDistanceSensor)
        {
            str += "RIGHT DISTANCE - ";
        }

        if (config.useFrontLeftLineSensor)
        {
            str += "FRONT LEFT LINE - ";
        }
        if (config.useFrontRightLineSensor)
        {
            str += "FRONT RIGHT LINE - ";
        }
        if (config.useBackLineSensor)
        {
            str += "BACK LINE - ";
        }
        if (config.useFrontLineSensor)
        {
            str += "FRONT LINE - ";
        }

        Debug.Log(str);
    }
    
}
