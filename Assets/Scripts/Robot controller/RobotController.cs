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

    public enum RobotDirection { Up, Down, Left, Right,Stop };
    private RobotDirection currentDirection;

    public LineSensor frontRightLineSensor;
    public LineSensor frontLeftLineSensor;
    public LineSensor backLineSensor;

    public DistanceSensor frontDistanseSensor;
    public DistanceSensor leftDistanseSensor;
    public DistanceSensor rightDistanseSensor;

    public bool stop;
    public bool useIA;

    private void Start()
    {
        SetSensorValues();       
    }

    private void SetSensorValues()
    {
        frontDistanseSensor.distance = distanceSensorDistance;
        leftDistanseSensor.distance = distanceSensorDistance;
        rightDistanseSensor.distance = distanceSensorDistance;

        frontRightLineSensor.distance = lineSensorDistance;
        frontLeftLineSensor.distance = lineSensorDistance;
        backLineSensor.distance = lineSensorDistance;
    }

    private void Update()
    {
        if (!stop)
        {
            UpdateSumobot();
        }
      
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!stop)
        {
            Move();
        }
              
    }

    private void UpdateSumobot()
    {
        if (useIA)
        {
            //TODO Use waittime to execute in cycles
            ExecuteIA();
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


    private void OnTriggerEnter2D(Collider2D collision)
    {       
        Debug.Log("COLISION ENTER: " + collision.gameObject.name );        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("COLISION EXIT: " + collision.gameObject.name);
    }

    public override void StartNetwork()
    {
            
    }

    public override float GetFitness()
    {
        return lifetime*lifetimeWeight + win*winWeight + enemyColisions*enemyCollisionsWeight;
    }

    public override void ExecuteIA()
    {
        float[] outputs = neuralNetwork.CalculateOutput(GetInputs());
        float minThreshold = 0.4;

        //Decide which direction move
        float moveUp = outputs[0];
        float moveDown = outputs[1];
        float turnRight = outputs[2];
        float turnLeft = outputs[3];
        float stop = outputs[4];

        //Select one random from the ones with output higher than minimun threshold
       


    }

    public override float[] GetInputs()
    {
        float[] list = new float[6];
        list[0] = frontDistanseSensor.Detect() ? 1 : 0;
        list[1] = rightDistanseSensor.Detect() ? 1 : 0;
        list[2] = leftDistanseSensor.Detect() ? 1 : 0;

        list[3] = frontRightLineSensor.Detect() ? 1 : 0;
        list[4] = frontLeftLineSensor.Detect() ? 1 : 0;
        list[5] = backLineSensor.Detect() ? 1 : 0;

        return list;
    }
}
