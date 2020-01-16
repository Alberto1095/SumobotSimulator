using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotIAController : RobotController,IADrivenObject
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

    public LineSensor frontRightLineSensor;
    public LineSensor frontLeftLineSensor;
    public LineSensor backLineSensor;
    public LineSensor frontLineSensor;

    public DistanceSensor frontDistanseSensor;
    public DistanceSensor leftDistanseSensor;
    public DistanceSensor rightDistanseSensor;

    public bool useDebugLines;

    //Variables for neural network
    protected int win;
    public float winWeight;
    protected int enemyColisions;
    public float enemyCollisionsWeight;

    protected NeuralNetwork neuralNetwork;
    protected float lastUpdateTime;
    public float updateWaitTime;   

    protected override void Start()
    {
        base.Start();       
        lastUpdateTime = -1000;
        SetSensorValues();
    }    

    public override void SetConfig(SumobotConfiguration config)
    {
        base.SetConfig(config);

        this.useBackLineSensor = config.useBackLineSensor;
        this.useFrontLeftLineSensor = config.useFrontLeftLineSensor;
        this.useFrontRightLineSensor = config.useFrontRightLineSensor;
        this.useFrontLineSensor = config.useFrontLineSensor;

        this.useFrontDistanceSensor = config.useFrontDistanceSensor;
        this.useRightDistanceSensor = config.useRightDistanceSensor;
        this.useLeftDistanceSensor = config.useLeftDistanceSensor;

        this.distanceSensorDistance = config.distanceSensorDistance;
        this.lineSensorDistance = config.lineSensorDistance;

        SetSensorValues();
      
    }

    private void SetSensorValues( )
    {
        frontRightLineSensor.distance = lineSensorDistance;
        frontLeftLineSensor.distance = lineSensorDistance;
        frontLeftLineSensor.distance = lineSensorDistance;
        backLineSensor.distance = lineSensorDistance;

        frontDistanseSensor.distance = distanceSensorDistance;
        rightDistanseSensor.distance = distanceSensorDistance;
        leftDistanseSensor.distance = distanceSensorDistance;
    }

    private bool CanUpdateIA()
    {
        float elapsed = Time.time - lastUpdateTime;
        if (elapsed > updateWaitTime)
        {
            lastUpdateTime = Time.time;
            return true;
        }
        return false;
    }

    protected override void UpdateSumobot()
    {
        if (CanUpdateIA())
        {
            GetInputs();
            ExecuteIA();
        }
    }
    

    public void ExecuteIA()
    {
        if(neuralNetwork != null)
        {
            float[] outputs = neuralNetwork.CalculateOutput(GetInputs());
            float minThreshold = 0.4f;

            //Decide which direction move
            //Select one random from the ones with output higher than minimun threshold
            List<int> list = new List<int>();

            for (int i = 0; i < outputs.Length; i++)
            {
                if (outputs[i] >= minThreshold)
                {
                    list.Add(i);
                }
            }

            int outputSelected;
            int randomIndex;
            if (list.Count > 0)
            {
                randomIndex = Random.Range(0, list.Count - 1);
                outputSelected = list[randomIndex];
            }
            else
            {
                outputSelected = Random.Range(0, outputs.Length-1);                
            }            

            switch (outputSelected)
            {
                case 0:
                    currentDirection = RobotDirection.Up;
                    break;
                case 1:
                    currentDirection = RobotDirection.Down;
                    break;
                case 2:
                    currentDirection = RobotDirection.Right;
                    break;
                case 3:
                    currentDirection = RobotDirection.Left;
                    break;
                case 4:
                    currentDirection = RobotDirection.Stop;
                    break;
            }
        }
        
    }

    public float[] GetInputs()
    {
       
        List<float> l = new List<float>();

        if (useFrontDistanceSensor)
        {
            l.Add(frontDistanseSensor.Detect() ? 1 : 0);
        }
        if (useLeftDistanceSensor)
        {
            l.Add(leftDistanseSensor.Detect() ? 1 : 0);
        }
        if (useRightDistanceSensor)
        {
            l.Add(rightDistanseSensor.Detect() ? 1 : 0);
        }

        if (useFrontLeftLineSensor)
        {
            l.Add(frontLeftLineSensor.Detect() ? 1 : 0);
        }
        if (useFrontRightLineSensor)
        {
            l.Add(frontRightLineSensor.Detect() ? 1 : 0);
        }
        if (useBackLineSensor)
        {
            l.Add(backLineSensor.Detect() ? 1 : 0);
        }
        if (useFrontLineSensor)
        {
            l.Add(frontLineSensor.Detect() ? 1 : 0);
        }
        

        float[] list = l.ToArray();

        return list;
    }

    public List<float> GetEvaluation()
    {
        return neuralNetwork.GetWeights();
    }

    public void StartNetwork(NeuralNetworkConfiguration config)
    {
        neuralNetwork = new NeuralNetwork(config);
    }

    public float GetFitness()
    {
        return win * winWeight + enemyCollisionsWeight * enemyColisions;
    }
}
