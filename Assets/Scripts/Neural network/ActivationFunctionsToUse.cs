using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ActivationFunction { Relu, Sigmoid };

public struct ActivationFunctions
{ 
    public ActivationFunction middleLayersFunction;
    public ActivationFunction finalLayerFunction;    
}
