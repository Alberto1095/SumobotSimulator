using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SumobotSpawner : MonoBehaviour
{
    //Configuration variables
    public GameObject sumobotPrefab;

    //Singleton

    private void Start()
    {
        CreateSumobot(new Vector3(0,-1.25f,0),Quaternion.identity);
        CreateSumobot(new Vector3(0, 1.25f, 0), Quaternion.Euler(0,0,180));
    }

   public GameObject CreateSumobot(Vector3 position,Quaternion rotation)
   {    /*
        GameObject robot = Instantiate(sumobotPrefab, position, rotation);

        int numLevels = 1;
        List<int> layersLevel = new List<int>(new int[] {5});
        int inputNumber = 6;
        ActivationFunctions functions = new ActivationFunctions(ActivationFunction.Lineal, ActivationFunction.Lineal);
        NeuralNetwork nn = new NeuralNetwork(numLevels, layersLevel, inputNumber, functions);
        nn.InitializeRandom();
        RobotController rc = robot.GetComponent<RobotController>();
        
        //rc.StartNetwork();
        rc.enable = true;
        rc.useIA = true;

        return robot;
        */
        return null;
   }
}
