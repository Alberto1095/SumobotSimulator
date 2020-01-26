using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RobotController : MonoBehaviour
{
    public Rigidbody2D rigidbody;

    public float maxSpeed;
    public float steerging;
    public float moveRotation;
    public float acceleration;
    protected float currentSpeed;

    protected float xDirection, yDirection;
   
    public bool enable;
    
    public CombatListenner listenner;

    protected virtual void Awake()
    {
        xDirection = 0;
        yDirection = 0;
    }    

    public virtual void SetConfig(SumobotConfiguration config)
    {
        this.maxSpeed = config.moveSpeed;
        this.steerging = config.rotationSpeed;      
    }   

    protected void Update()
    {
        if (enable)
        {
            UpdateSumobot();
        }      
    }

    public void SetStartPosition(Vector3 pos,Quaternion rotation)
    {
        transform.SetPositionAndRotation(pos, rotation);
    }

    public abstract void SetWin(bool b);

    // Update is called once per frame
    protected void FixedUpdate()
    {
        if (enable)
        {
            Move();
        }              
    }

    public void SetEnable(bool b)
    {
        enable = b;
        if (!enable)
        {
            //TODO STOP OBJECT IN CASE IS MOVING
            rigidbody.velocity = new Vector2(0, 0);
        }
    }

    protected abstract void UpdateSumobot();    

    private void Move()
    {
        float h = -xDirection;
        float v = yDirection;
               
        if(v == 0)
        {
            if (h == -1)
            {
                rigidbody.MoveRotation(rigidbody.rotation - moveRotation*Time.fixedDeltaTime);
            }
            else if(h == 1)
            {
                rigidbody.MoveRotation(rigidbody.rotation + moveRotation * Time.fixedDeltaTime);
            }  
           
        }
        else
        {
            // Calculate speed from input and acceleration (transform.up is forward)
            Vector2 speed = transform.up * (v * acceleration);
            rigidbody.AddForce(speed);


            // Create car rotation
            float direction = Vector2.Dot(rigidbody.velocity, rigidbody.GetRelativeVector(Vector2.up));
            if (direction >= 0.0f)
            {
                rigidbody.rotation += h * steerging * (rigidbody.velocity.magnitude / maxSpeed);
            }
            else
            {
                rigidbody.rotation -= h * steerging * (rigidbody.velocity.magnitude / maxSpeed);
            }

            // Change velocity based on rotation
            float driftForce = Vector2.Dot(rigidbody.velocity, rigidbody.GetRelativeVector(Vector2.left)) * 2.0f;
            Vector2 relativeForce = Vector2.right * driftForce;
            rigidbody.AddForce(rigidbody.GetRelativeVector(relativeForce));

            // Force max speed limit
            if (rigidbody.velocity.magnitude > maxSpeed)
            {
                rigidbody.velocity = rigidbody.velocity.normalized * maxSpeed;
            }
            currentSpeed = rigidbody.velocity.magnitude;
            Debug.Log("SPEED: " + currentSpeed);
        }
        
    }
   

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (enable)
        {
            //Debug.Log("COLISION EXIT RING: " + collision.gameObject.name);
            if (collision.gameObject.layer == LayerMask.NameToLayer("Exterior"))
            {
                //SetEnable(false);
                //listenner.OnDeath(this);
            }
        }
        
    }      
}
