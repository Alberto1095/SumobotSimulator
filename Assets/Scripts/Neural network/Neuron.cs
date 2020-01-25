using System.Collections.Generic;
using UnityEngine;

public class Neuron
{ 

    private ActivationFunction function;
    private List<float> weights;
    private int numberOfInputs;

    //Constructor neuron using custum weights (bias last value)
    public Neuron(List<float> weights, int numberOfInputs,ActivationFunction function)
    {
        this.numberOfInputs = numberOfInputs;
        this.weights = weights;
        this.function = function;
    }

    //Constructor for neuron with random weights
    public Neuron(int numberOfInputs, ActivationFunction function)
    {
        this.numberOfInputs = numberOfInputs;
        this.function = function;
        InitializeRandomWeightValues();        
    }


    public List<float> GetWeights()
    {
        return weights;
    }

    private void InitializeRandomWeightValues()
    {
        float num;
        float max = 1;
        float min = -1;
        weights = new List<float>();
        for (int i = 0; i < numberOfInputs + 1; i++)
        {
            num = (float)(min + Random.value*(max - min));            
            weights.Add(num);
        }
    }

    public float CalculateOutput(float[] inputValues)
    {
        float f = 0;
        for (int i = 0; i < numberOfInputs; i++)
        {
			f += inputValues[i] * weights[i];
        }	
		//Add bias value
		f += weights[weights.Count - 1];

        float salida = 0;
        //Activation function
        if (function == ActivationFunction.Relu)
        {           
            //ReLU
            if (f < 0){
               salida = 0;
            }
            else
            {
               salida = f;
            }
        }
        else if (function == ActivationFunction.Sigmoid)
        {           
            //Sigmoid
            salida = 1 / (1 + Mathf.Exp(-1 * f));
        }else if(function == ActivationFunction.Lineal)
        {
            salida = f;
        }

        return salida;
    }
}
