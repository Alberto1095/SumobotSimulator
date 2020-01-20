using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ConfigTxtFile 
{
    private string fileName;
    private SumobotIAConfiguration iaConfig;

    public ConfigTxtFile(){
      
    }

    public void SaveToFile(string path, SumobotIAConfiguration iaConfig)
    {
        string str = "";
        str += iaConfig.moveSpeed + "\n";
        str += iaConfig.rotationSpeed + "\n";

        str += iaConfig.lineSensorDistance + "\n";
        str += iaConfig.distanceSensorDistance + "\n";

        str += iaConfig.useFrontDistanceSensor + "\n";
        str += iaConfig.useLeftDistanceSensor + "\n";
        str += iaConfig.useRightDistanceSensor + "\n";

        str += iaConfig.useBackLineSensor + "\n";
        str += iaConfig.useFrontLineSensor + "\n";
        str += iaConfig.useFrontLeftLineSensor + "\n";
        str += iaConfig.useFrontRightLineSensor + "\n";

        str += iaConfig.numLevels + "\n";
        str += iaConfig.numInputs + "\n";

        string layersPerLevel = "";
        foreach(int i in iaConfig.numLayersPerLevel)
        {
            layersPerLevel += i +"/";
        }
        layersPerLevel = layersPerLevel.Substring(0,layersPerLevel.Length-1);
        str += layersPerLevel + "\n";

        str += iaConfig.functions.middleLayersFunction + "\n";
        str += iaConfig.functions.finalLayerFunction + "\n";

        if(iaConfig.weights != null)
        {
            string weights = "";
            foreach (float i in iaConfig.weights)
            {
                weights += i + "/";
            }
            weights = weights.Substring(0, weights.Length - 1);
            str += weights;
        }      


        File.WriteAllText(path, str);
    }

    public void ReadFromFile(string path,string name)
    {
        fileName = name;
        iaConfig = new SumobotIAConfiguration();

        string[] lines = File.ReadAllLines(path);

        iaConfig.moveSpeed = float.Parse(lines[0]);
        iaConfig.rotationSpeed = float.Parse(lines[1]);

        iaConfig.lineSensorDistance = float.Parse(lines[2]);
        iaConfig.distanceSensorDistance = float.Parse(lines[3]);

        iaConfig.useFrontDistanceSensor = bool.Parse(lines[4]);
        iaConfig.useLeftDistanceSensor = bool.Parse(lines[5]);
        iaConfig.useRightDistanceSensor = bool.Parse(lines[6]);

        iaConfig.useBackLineSensor = bool.Parse(lines[7]);
        iaConfig.useFrontLineSensor = bool.Parse(lines[8]);
        iaConfig.useFrontLeftLineSensor = bool.Parse(lines[9]);
        iaConfig.useFrontRightLineSensor = bool.Parse(lines[10]);

        iaConfig.numLevels = int.Parse(lines[11]);
        iaConfig.numInputs = int.Parse(lines[12]);

        List<int> numLayersPerLevel = new List<int>();        
        string[] l = lines[13].Split('/');
        foreach (string c in l)
        {
            numLayersPerLevel.Add(int.Parse(c));
        }
        iaConfig.numLayersPerLevel = numLayersPerLevel;

        ActivationFunctions func = new ActivationFunctions();
        func.middleLayersFunction = (ActivationFunction)System.Enum.Parse(typeof(ActivationFunction),lines[14]);
        func.finalLayerFunction = (ActivationFunction)System.Enum.Parse(typeof(ActivationFunction),lines[15]);
        iaConfig.functions = func;
        
        if(lines.Length > 16)
        {
            string str = lines[16];
            List<float> weigthList = new List<float>();
            string[] wList = str.Split('/');
            foreach (string c in wList)
            {
                weigthList.Add(float.Parse(c));
            }
            iaConfig.weights = weigthList;
        }
        
        
    }
}
