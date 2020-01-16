using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RobotController : MonoBehaviour
{
    public Rigidbody2D rigidbody;

    public float moveSpeed;
    public float rotationSpeed;  

    public enum RobotDirection { Up, Down, Left, Right,Stop };
    protected RobotDirection currentDirection;   
   
    public bool enable;   

    protected virtual void Start()
    {
        Debug.Log("START PARENT");
        currentDirection = RobotDirection.Stop;      
    }    

    public virtual void SetConfig(SumobotConfiguration config)
    {
        this.moveSpeed = config.moveSpeed;
        this.rotationSpeed = config.rotationSpeed;      
    }   

    protected void Update()
    {
        if (enable)
        {
            UpdateSumobot();
        }      
    }
    

    // Update is called once per frame
    protected void FixedUpdate()
    {
        if (enable)
        {
            Move();
        }              
    }

    protected abstract void UpdateSumobot();    

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

    protected Vector2 GetCurrentVelocity()
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
}
