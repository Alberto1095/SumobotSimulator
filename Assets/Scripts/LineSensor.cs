using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineSensor : MonoBehaviour
{   
  
    public Transform parentTransform;
    public float distance;

    public bool Detect(int direction)
    {      
        Vector2 directionVector = GetFacingVector() * direction;        
        int layerMask = LayerMask.GetMask("Interior", "Exterior");
        Vector2 origin;
        Vector2 endPoint;

        origin = transform.position;
        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, directionVector,
            distance, layerMask);
        endPoint = origin + directionVector * direction*distance;
        Debug.DrawLine(new Vector3(origin.x, origin.y), new Vector3(endPoint.x, endPoint.y),
            Color.green, 0, true);        
        bool detected = DetectOuterRing(hits);
        
        return detected;
    }

    private Vector2 GetFacingVector()
    {
        Vector2 forwardVector = new Vector2(transform.up.x, transform.up.y);
        return forwardVector;
    }

    private bool DetectOuterRing(RaycastHit2D[] hits)
    {

        bool detectedInnerRing = false;
        bool detectedOuterRing = false;
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Interior"))
            {
                detectedInnerRing = true;
            }
            else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Exterior"))
            {
                detectedOuterRing = true;
            }
        }

        return detectedOuterRing && !detectedInnerRing;
    }
}
