﻿using System.Collections.Generic;
using UnityEngine;

public class NeuralNetwork 
{
    private int numLevels, inputNumber;
    private NeuralLayer firstLayer;
    private List<int> numLayersPerLevel;
    private ActivationFunctions functions;

    public NeuralNetwork(int numLevels, List<int> numLayersPerLevel, int inputNumber,ActivationFunctions functions)
    {
        this.numLevels = numLevels;
        this.inputNumber = inputNumber;
        this.numLayersPerLevel = numLayersPerLevel;
        this.functions = functions;
    }

    public NeuralNetwork(SumobotIAConfiguration config)
    {
        this.numLevels = config.numLevels;
        this.functions = config.functions;        
        this.numLayersPerLevel = config.numLayersPerLevel;
        this.inputNumber = config.numInputs;

        if(config.weights == null)
        {
          
            InitializeRandom();
        }
        else
        {
            InitializeUsingCustomWeightValues(config.weights);
        }
    }

    public void InitializeRandom()
    {
        firstLayer = new NeuralLayer(numLayersPerLevel, numLevels, inputNumber,functions);
    }

    public void InitializeUsingCustomWeightValues(List<float> weights)
    {
        firstLayer = new NeuralLayer(numLayersPerLevel, numLevels, inputNumber, weights,functions);
    }

    public float[] CalculateOutput(float[] inputs)
    {
        return firstLayer.CalculateOutputs(inputs);
    }

    public List<float> GetWeights()
    {
        List<float> list = new List<float>();
        return firstLayer.GetWeights(list);

    }
}
