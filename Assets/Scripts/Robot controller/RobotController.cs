using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : IADrivenObject
{
    public Rigidbody2D rigidbody;

    public float moveSpeed;
    public float rotationSpeed;

    public float lineSensorDistance;
    public float distanceSensorDistance;

    public bool useFrontDistanceSensor;
    public bool useLeftDistanceSensor;
    public bool useRightDistanceSensor;

    public bool useFrontRightLineSensor;
    public bool useFrontLeftLineSensor;
    public bool useFrontLineSensor;
    public bool useBackLineSensor;

    public enum RobotDirection { Up, Down, Left, Right,Stop };
    private RobotDirection currentDirection;

    public LineSensor frontRightLineSensor;
    public LineSensor frontLeftLineSensor;
    public LineSensor backLineSensor;
    public LineSensor frontLineSensor;

    public DistanceSensor frontDistanseSensor;
    public DistanceSensor leftDistanseSensor;
    public DistanceSensor rightDistanseSensor;

    public bool useDebugLines;
    public bool enable;
    public bool useIA;

    private void Start()
    {
        currentDirection = RobotDirection.Stop;
        lastUpdateTime = -1000;
        SetSensorValues();
    }    

    public void SetConfig(SumobotConfiguration config)
    {
        this.moveSpeed = config.moveSpeed;
        this.rotationSpeed = config.rotationSpeed;

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

    private void Update()
    {
        if (enable)
        {
            UpdateSumobot();
        }      
    }
    

    // Update is called once per frame
    void FixedUpdate()
    {
        if (enable)
        {
            Move();
        }              
    }

    private void UpdateSumobot()
    {
        if (useIA)
        {
            if (CanUpdateIA())
            {
                Debug.Log("UPDATE IA");
                ExecuteIA();
            }
           
        }
        else
        {
            CheckUserInput();
        }
    }

    private void CheckUserInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            currentDirection = RobotDirection.Up;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            currentDirection = RobotDirection.Down;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            currentDirection = RobotDirection.Left;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            currentDirection = RobotDirection.Right;
        }
        else
        {
            currentDirection = RobotDirection.Stop;
        }
    }

    private void Move()
    {
        switch (currentDirection)
        {
            case RobotDirection.Up:                
                rigidbody.MovePosition(rigidbody.position + 
                    GetCurrentVelocity() * moveSpeed * Time.fixedDeltaTime);
                break;
            case RobotDirection.Down:
                rigidbody.MovePosition(rigidbody.position +
                   -1*GetCurrentVelocity() * moveSpeed * Time.fixedDeltaTime);
                break;
            case RobotDirection.Left:
                rigidbody.MoveRotation(rigidbody.rotation + rotationSpeed * Time.fixedDeltaTime);
                break;
            case RobotDirection.Right:
                rigidbody.MoveRotation(rigidbody.rotation -rotationSpeed * Time.fixedDeltaTime);
                break;
            case RobotDirection.Stop:
                break;
        }       
    }

    private Vector2 GetCurrentVelocity()
    {
        Vector2 forwardVector = new Vector2(transform.up.x, transform.up.y);        
        return forwardVector;
    }     
   

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("COLISION EXIT: " + collision.gameObject.name);
        if(collision.gameObject.layer == LayerMask.NameToLayer("Exterior"))
        {
            enable = false;
        }
    }   

    public override void ExecuteIA()
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

    public override float[] GetInputs()
    {
       
        List<float> l = new List<float>();

        if (useFrontDistanceSensor)
        {
            l.Add( frontDistanseSensor.Detect() ? 1 : 0);
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

    public override List<float> GetEvaluation()
    {
        return neuralNetwork.GetWeights();
    }
}
