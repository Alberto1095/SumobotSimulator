using System.Collections.Generic;

public interface IADrivenObject
{
    float GetFitness();    
    void ExecuteIA();
    float[] GetInputs();
    Evaluation GetEvaluation();
}
