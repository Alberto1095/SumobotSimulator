using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    public Rigidbody2D rigidbody;
    public float moveSpeed;
    public float rotationSpeed;

    public enum RobotDirection { Up, Down, Left, Right,Stop };
    private RobotDirection currentDirection;

    public LineSensor frontRightLineSensor;
    public LineSensor frontLeftLineSensor;
    public LineSensor backLineSensor;

    public bool stop;

    private void Start()
    {
       
    }

    private void Update()
    {
        if (!stop)
        {
            CheckUserInput();
            CheckLineSensors();
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

    private void CheckLineSensors()
    {
        bool detected  = frontRightLineSensor.Detect(1);
        //Debug.Log("DETECTED FRL: " + detected);

        detected = frontLeftLineSensor.Detect(1);
        //Debug.Log("DETECTED FLL: " + detected);

        detected = backLineSensor.Detect(-1);
        Debug.Log("DETECTED BL: " + detected);

    }     


    private void OnTriggerEnter2D(Collider2D collision)
    {       
        Debug.Log("COLISION ENTER: " + collision.gameObject.name );        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("COLISION EXIT: " + collision.gameObject.name);
    }

}
