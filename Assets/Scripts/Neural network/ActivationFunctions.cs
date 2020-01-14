using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ActivationFunction { Relu, Sigmoid,Lineal };

public struct ActivationFunctions
{ 
    public ActivationFunction middleLayersFunction;
    public ActivationFunction finalLayerFunction;    

    public ActivationFunctions(ActivationFunction middleLayer,ActivationFunction finalLayer)
    {
        this.middleLayersFunction = middleLayer;
        this.finalLayerFunction = finalLayer;
    }
}
