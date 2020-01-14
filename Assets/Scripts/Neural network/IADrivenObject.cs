using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IADrivenObject : MonoBehaviour
{
    //Variables for fitness calculation
    protected float lifetime;
    protected static float lifetimeWeight = 3;
    protected int win;
    protected static float winWeight = 2;
    protected int enemyColisions;
    protected static float enemyCollisionsWeight = 5;

    protected NeuralNetwork neuralNetwork;
    

    public float updateWaitTime;    


    public abstract void StartNetwork();
    public abstract float GetFitness();
    public abstract void ExecuteIA();
    public abstract float[] GetInputs();
}
