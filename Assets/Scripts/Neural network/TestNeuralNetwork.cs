using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNeuralNetwork : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Test1();
    }

    private void Test1()
    {
        int numLevels = 2;
        List<int> layersLevel = new List<int>(new int[]{1,1});
        int inputNumber = 2;
        ActivationFunctions functions = new ActivationFunctions(ActivationFunction.Lineal, ActivationFunction.Lineal);
        NeuralNetwork nn = new NeuralNetwork(numLevels, layersLevel, inputNumber, functions);

        List<float> weights = new List<float>(new float[] {1,1,1,2,-10});
        nn.InitializeUsingCustomWeightValues(weights);

        float[] inputs = new float[] { 1, 2 };

        float[] output = nn.CalculateOutput(inputs);

        foreach(float v in output)
        {
            Debug.Log("Output: " + v);
        }
    }
}
