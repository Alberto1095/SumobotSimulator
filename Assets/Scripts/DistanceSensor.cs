using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceSensor : MonoBehaviour
{     
    public Transform parentTransform;
    public float rotation;
    public float distance;
    public LineRenderer lineRenderer;
    public bool useLineRenderer;   

    public bool Detect()
    {      
        Vector2 directionVector = RotateVector(GetFacingVector(),rotation);        
        int layerMask = LayerMask.GetMask("Robot");
        Vector2 origin;
        Vector2 endPoint;

        origin = transform.position;
        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, directionVector,
            distance, layerMask);
        endPoint = origin + directionVector*distance;
        
        DebugLine(origin, endPoint);

        bool detected = DetectEnemy(hits);
        
        return detected;
    }

    private Vector2 RotateVector(Vector2 v, float aDegree)
    {
        return Quaternion.Euler(0, 0, aDegree) * v;
    }


    private void DebugLine(Vector2 startPoint,Vector2 endPoint)
    {
        if (useLineRenderer)
        {
            lineRenderer.enabled = true;
            Vector3[] positions = new Vector3[2];
            positions[0] = new Vector3(startPoint.x, startPoint.y,-5);
            positions[1] = new Vector3(endPoint.x, endPoint.y,-5);

            lineRenderer.SetPositions(positions);
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    private Vector2 GetFacingVector()
    {
        Vector2 forwardVector = new Vector2(transform.up.x, transform.up.y);
        return forwardVector;
    }

    private bool DetectEnemy(RaycastHit2D[] hits)
    {
        bool detectedEnemy = false;      
        foreach (RaycastHit2D hit in hits)
        {           
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Robot") 
                && hit.collider.gameObject != parentTransform.gameObject)
            {
                detectedEnemy = true;
            }
            
        }

        return detectedEnemy;
    }
}
