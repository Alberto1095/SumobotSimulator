using System.Collections.Generic;

public interface IADrivenObject
{
    void StartNetwork(NeuralNetworkConfiguration config);
    float GetFitness();    
    void ExecuteIA();
    float[] GetInputs();
    List<float> GetEvaluation();
}
