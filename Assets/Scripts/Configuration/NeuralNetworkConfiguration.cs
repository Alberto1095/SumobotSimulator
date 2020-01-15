using System.Collections.Generic;

public class NeuralNetworkConfiguration 
{
    public int numLevels;
    public int numInputs;
    public List<int> numLayersPerLevel;
    public ActivationFunctions functions;


    public NeuralNetworkConfiguration()
    {

    }

    public NeuralNetworkConfiguration(int numLevels, int numInputs, List<int> numLayersPerLevel, ActivationFunctions functions)
    {
        this.numLevels = numLevels;
        this.numInputs = numInputs;
        this.numLayersPerLevel = numLayersPerLevel;
        this.functions = functions;
    }

}
