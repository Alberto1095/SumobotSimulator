using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IADrivenObject : MonoBehaviour
{
    //Variables for fitness calculation
    protected float lifetime;
    public float lifetimeWeight;
    protected int win;
    public float winWeight;
    protected int enemyColisions;
    public float enemyCollisionsWeight;

    protected NeuralNetwork neuralNetwork;
    protected float lastUpdateTime;
    public float updateWaitTime;  

    protected bool CanUpdateIA()
    {
        float elapsed = Time.time - lastUpdateTime;
        if(elapsed > updateWaitTime)
        {
            lastUpdateTime = Time.time;
            return true;
        }
        return false;
    }

    public abstract void StartNetwork(NeuralNetwork nn);
    public abstract float GetFitness();
    public abstract void ExecuteIA();
    public abstract float[] GetInputs();
    public abstract List<float> GetEvaluation();
}
