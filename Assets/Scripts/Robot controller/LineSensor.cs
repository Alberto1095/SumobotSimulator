﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineSensor : MonoBehaviour
{   
  
    public Transform parentTransform;
    public float distance;
    public int direction;
    public LineRenderer lineRenderer;
    public bool useLineRenderer;
    public bool lastValueRead;

    public bool Detect()
    {      
        Vector2 directionVector = GetFacingVector() * direction;        
        int layerMask = LayerMask.GetMask("Interior", "Exterior");
        Vector2 origin;
        Vector2 endPoint;

        origin = transform.position;
        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, directionVector,
            distance, layerMask);
        endPoint = origin + directionVector*distance;
        
        DebugLine(origin, endPoint);

        bool detected = DetectOuterRing(hits);
        
        return detected;
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
        lastValueRead = detectedOuterRing && !detectedInnerRing;
        return detectedOuterRing && !detectedInnerRing;
    }
}
