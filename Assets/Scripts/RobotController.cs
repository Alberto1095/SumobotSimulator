using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    public Rigidbody2D rigidbody;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("START");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 velocity = new Vector2();
        if (Input.GetKey(KeyCode.W))
        {
            velocity = new Vector2(0, 1);            
           
        }else if(Input.GetKey(KeyCode.S))
        {
            velocity = new Vector2(0, -1);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            velocity = new Vector2(-1, 0);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            velocity = new Vector2(1, 0);
        }

        rigidbody.MovePosition(rigidbody.position + velocity * speed * Time.fixedDeltaTime);
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
