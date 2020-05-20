using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RobotController : MonoBehaviour
{
    public Rigidbody2D rigidbody;
    //Maxima velocidad
    public float maxSpeed;
    //Velocidad rotation con movimiento
    public float steeringSpeed;
    //Velocidad rotation
    public float rotationSpeed;
    //Aceleracion
    public float acceleration;
    protected float currentSpeed;
    //Direcciones actuales
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
        this.maxSpeed = config.maxSpeed;
        this.rotationSpeed = config.rotationSpeed;
        this.steeringSpeed = config.steeringSpeed;
        this.acceleration = config.acceleration;
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

    public virtual void SetEnable(bool b)
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
                //Rotacion izquierda
                rigidbody.MoveRotation(rigidbody.rotation - rotationSpeed*Time.fixedDeltaTime);
            }
            else if(h == 1)
            {
                //Rotacion derecha
                rigidbody.MoveRotation(rigidbody.rotation + rotationSpeed * Time.fixedDeltaTime);
            }  
           
        }
        else
        {
            //Movimiento con rotation al mismo tiempo
            // Calculo velocidad frontal en funcion de la aceleracion
            Vector2 speed = transform.up * (v * acceleration);
            rigidbody.AddForce(speed);

            //Calculo de la rotacion
            float direction = Vector2.Dot(rigidbody.velocity, rigidbody.GetRelativeVector(Vector2.up));
            if (direction >= 0.0f)
            {
                rigidbody.rotation += h * steeringSpeed * (rigidbody.velocity.magnitude / maxSpeed);
            }
            else
            {
                rigidbody.rotation -= h * steeringSpeed * (rigidbody.velocity.magnitude / maxSpeed);
            }

            //Cambio de direccion en funcion de la rotacion del movimiento
            float driftForce = Vector2.Dot(rigidbody.velocity, rigidbody.GetRelativeVector(Vector2.left)) * 2.0f;
            Vector2 relativeForce = Vector2.right * driftForce;
            rigidbody.AddForce(rigidbody.GetRelativeVector(relativeForce));

            //Controbacion de que no se pasa la velocidad maxima
            if (rigidbody.velocity.magnitude > maxSpeed)
            {
                rigidbody.velocity = rigidbody.velocity.normalized * maxSpeed;
            }
            currentSpeed = rigidbody.velocity.magnitude;            
        }        
    }
   

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (enable)
        {
            //Debug.Log("COLISION EXIT RING: " + collision.gameObject.name);
            if (collision.gameObject.layer == LayerMask.NameToLayer("Exterior"))
            {
                SetEnable(false);
                listenner.OnDeath(this);
            }
        }
        
    }      
}
