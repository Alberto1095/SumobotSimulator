using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralLayer
{
    private List<Neuron> neurons;
    private int neuronNumber;
    private NeuralLayer nextLayer;
    private int inputNumber;

    public NeuralLayer(List<int> sizeList, int layersLeft, int inputNumber,ActivationFunctions functions)
    {
        this.inputNumber = inputNumber;
        this.neuronNumber = sizeList[0];
        sizeList.RemoveAt(0);
        
        layersLeft--;
        if (layersLeft > 0)
        {
            InitializeNeuronsRandom(functions.middleLayersFunction);
            //The number of inputs in the next layers is the number of neurons of this one
            nextLayer = new NeuralLayer(sizeList, layersLeft, neuronNumber,functions);
        }
        else
        {
            InitializeNeuronsRandom(functions.finalLayerFunction);
        }
    }

    public NeuralLayer(List<int> sizeList, int layersLeft, int inputNumber, List<float> weights,ActivationFunctions functions)
    {
        this.inputNumber = inputNumber;
        this.neuronNumber = sizeList[0];
        sizeList.RemoveAt(0);
        
        layersLeft--;
        if (layersLeft > 0)
        {
            List<float> weightsLeft = InitializeNeuronsUsingCustomWeights(weights,functions.middleLayersFunction);
            //The number of inputs in the next layers is the number of neurons of this one
            nextLayer = new NeuralLayer(sizeList, layersLeft, neuronNumber, weightsLeft,functions);
        }
        else
        {
            InitializeNeuronsUsingCustomWeights(weights, functions.finalLayerFunction);
        }
    }

    private void InitializeNeuronsRandom(ActivationFunction function)
    {
        neurons = new List<Neuron>();
        for (int i = 0; i < neuronNumber; i++)
        {
            neurons.Add(new Neuron(inputNumber,function));
        }
    }

    private List<float> InitializeNeuronsUsingCustomWeights(List<float> weights,ActivationFunction function)
    {
        List<float> valuesLeft = weights;
        List<float> valuesToUse = new List<float>();
        int weightsInEveryNeuron = inputNumber + 1; //Bias		
        neurons = new List<Neuron>();
        for (int i = 0; i < neuronNumber; i++)
        {
            valuesToUse = new List<float>();
            for (int j = 0; j < weightsInEveryNeuron; j++)
            {
                valuesToUse.Add(valuesLeft[0]);
                valuesLeft.RemoveAt(0);
            }
            neurons.Add(new Neuron(valuesToUse, inputNumber,function));
        }

        return valuesLeft;
    }

    private bool IsFinalLayer()
    {
        return nextLayer == null;
    }

    public float[] CalculateOutputs(float[] inputs)
    {
        //Calculate the output of every neuron using the inputs parameters and add to the output array
        float[] outputs = new float[neurons.Count];

        for (int index = 0; index < neuronNumber; index++)
        {
            outputs[index] = neurons[index].CalculateOutput(inputs);
        }

        if (IsFinalLayer())
        {
            return outputs;
        }
        else
        {
            //use the outputs of this layer as the inputs in the next layer
            return nextLayer.CalculateOutputs(outputs);
        }

    }

    public List<float> GetWeights(List<float> weights)
    {
        foreach (Neuron neuron in neurons)
        {            
            weights.AddRange(neuron.GetWeights());
        }
        if (IsFinalLayer())
        {
            return weights;
        }
        else
        {
            return nextLayer.GetWeights(weights);
        }

    }

}
