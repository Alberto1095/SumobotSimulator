using System.Collections.Generic;

public enum ActivationFunction { Relu, Sigmoid, Lineal };

public struct ActivationFunctions
{
    public ActivationFunction middleLayersFunction;
    public ActivationFunction finalLayerFunction;

    public ActivationFunctions(ActivationFunction middleLayer, ActivationFunction finalLayer)
    {
        this.middleLayersFunction = middleLayer;
        this.finalLayerFunction = finalLayer;
    }   
}

public class SumobotIAConfiguration: SumobotConfiguration
{
    public float lineSensorDistance;
    public float distanceSensorDistance;

    public bool useFrontDistanceSensor;
    public bool useLeftDistanceSensor;
    public bool useRightDistanceSensor;

    public bool useFrontRightLineSensor;
    public bool useFrontLeftLineSensor;
    public bool useFrontLineSensor;
    public bool useBackLineSensor;

    public int numLevels;
    public int numInputs;
    public List<int> numLayersPerLevel;
    public ActivationFunctions functions;
    public List<float> weights;

    public SumobotIAConfiguration()
    {

    }

    public SumobotIAConfiguration(float maxSpeed, float rotationSpeed, float steeringSpeed, float acceleration,
        float lineSensorDistance, float distanceSensorDistance, bool useFrontDistanceSensor, 
        bool useLeftDistanceSensor, bool useRightDistanceSensor, bool useFrontRightLineSensor, 
        bool useFrontLeftLineSensor, bool useFrontLineSensor, bool useBackLineSensor, int numLevels, 
        int numInputs, List<int> numLayersPerLevel, ActivationFunctions functions, List<float> weights):base(maxSpeed,rotationSpeed,steeringSpeed,acceleration)
    {
        
        this.lineSensorDistance = lineSensorDistance;
        this.distanceSensorDistance = distanceSensorDistance;
        this.useFrontDistanceSensor = useFrontDistanceSensor;
        this.useLeftDistanceSensor = useLeftDistanceSensor;
        this.useRightDistanceSensor = useRightDistanceSensor;
        this.useFrontRightLineSensor = useFrontRightLineSensor;
        this.useFrontLeftLineSensor = useFrontLeftLineSensor;
        this.useFrontLineSensor = useFrontLineSensor;
        this.useBackLineSensor = useBackLineSensor;
        this.numLevels = numLevels;
        this.numInputs = numInputs;
        this.numLayersPerLevel = numLayersPerLevel;
        this.functions = functions;
        this.weights = weights;
    }

    public static SumobotIAConfiguration GetDefaultIAConfig()
    {
        SumobotIAConfiguration sc = new SumobotIAConfiguration();
        sc.maxSpeed = 5;
        sc.rotationSpeed = 250;
        sc.acceleration = 50;
        sc.steeringSpeed = 5;
        sc.lineSensorDistance = 0.1f;
        sc.distanceSensorDistance = 7;

        sc.useFrontDistanceSensor = true;
        sc.useRightDistanceSensor = false;
        sc.useLeftDistanceSensor = false;

        sc.useBackLineSensor = false;
        sc.useFrontLineSensor = false;
        sc.useFrontLeftLineSensor = false;
        sc.useFrontRightLineSensor = false;

        sc.numInputs = 1;
        sc.numLevels = 1;
        sc.numLayersPerLevel = new List<int>() { 5 };
        sc.functions =  new ActivationFunctions(ActivationFunction.Relu, ActivationFunction.Sigmoid);
        sc.weights = null;

        return sc;
    }

    public static SumobotIAConfiguration Copy(SumobotIAConfiguration c)
    {
        SumobotIAConfiguration config = new SumobotIAConfiguration();
        config.maxSpeed = c.maxSpeed;
        config.rotationSpeed = c.rotationSpeed;
        config.steeringSpeed = c.steeringSpeed;
        config.acceleration = c.acceleration;

        config.lineSensorDistance = c.lineSensorDistance;
        config.distanceSensorDistance = c.distanceSensorDistance;
        config.useBackLineSensor = c.useBackLineSensor;
        config.useFrontLineSensor = c.useFrontLineSensor;
        config.useFrontRightLineSensor = c.useFrontRightLineSensor;
        config.useFrontLeftLineSensor = c.useFrontLeftLineSensor;
        config.useFrontDistanceSensor = c.useFrontDistanceSensor;
        config.useLeftDistanceSensor = c.useLeftDistanceSensor;
        config.useRightDistanceSensor = c.useRightDistanceSensor;

        config.numInputs = c.numInputs;
        config.numLevels = c.numLevels;
        config.numLayersPerLevel = new List<int>(c.numLayersPerLevel);
        config.functions = c.functions;
        if(c.weights != null)
        {
            config.weights = new List<float>(c.weights);
        }
       

        return config;
    }
}
