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

    protected override void Awake()
    {
        base.Awake();       
        lastUpdateTime = -1000;
        SetSensorValues();
    }    

    public override void SetConfig(SumobotConfiguration config)
    {
        base.SetConfig(config);

        if(config is SumobotIAConfiguration)
        {
            SumobotIAConfiguration iaConfig = (SumobotIAConfiguration)config;

            this.useBackLineSensor = iaConfig.useBackLineSensor;
            this.useFrontLeftLineSensor = iaConfig.useFrontLeftLineSensor;
            this.useFrontRightLineSensor = iaConfig.useFrontRightLineSensor;
            this.useFrontLineSensor = iaConfig.useFrontLineSensor;

            this.useFrontDistanceSensor = iaConfig.useFrontDistanceSensor;
            this.useRightDistanceSensor = iaConfig.useRightDistanceSensor;
            this.useLeftDistanceSensor = iaConfig.useLeftDistanceSensor;

            this.distanceSensorDistance = iaConfig.distanceSensorDistance;
            this.lineSensorDistance = iaConfig.lineSensorDistance;

            SetSensorValues();

            neuralNetwork = new NeuralNetwork(iaConfig);
        }
       
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

        if (neuralNetwork != null)
        {
            float[] inputs = GetInputs();
            float[] outputs = neuralNetwork.CalculateOutput(inputs);

            float yOutput = outputs[0]*2 -1;
            float xOutput = outputs[1]*2 -1;

            if(yOutput > 0.33)
            {
                yDirection = 1;
            }else if(yOutput < -0.33)
            {
                yDirection = -1;
            }
            else
            {
                yDirection = 0;
            }

            if (xOutput > 0.33)
            {
                xDirection = 1;
            }
            else if (xOutput < -0.33)
            {
                xDirection = -1;
            }
            else
            {
                xDirection = 0;
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

    public Evaluation GetEvaluation()
    {
        return new Evaluation(neuralNetwork.GetWeights());
    }  

    public float GetFitness()
    {
        return win * winWeight + enemyCollisionsWeight * enemyColisions;
    }

    public override void SetWin(bool b)
    {
        if (b)
        {
            win = 1;
        }
        else
        {
            win = 0;
        }
    }

    private bool HasDetectedSensor()
    {      
        if (useFrontDistanceSensor)
        {
            if (frontDistanseSensor.lastValuedRead)
            {
                return true;
            }
            
        }
        if (useLeftDistanceSensor)
        {
            if (leftDistanseSensor.lastValuedRead)
            {
                return true;
            }
        }
        if (useRightDistanceSensor)
        {
            if (rightDistanseSensor.lastValuedRead)
            {
                return true;
            }
        }

        return false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (enable)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Robot")
                && collision.gameObject != gameObject)
            {
                //Check sensor 
                if (HasDetectedSensor())
                {                   
                    enemyColisions++;
                }
            }
        }        
    }
}
