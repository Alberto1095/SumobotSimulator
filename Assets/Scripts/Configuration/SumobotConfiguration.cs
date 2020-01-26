using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SumobotConfiguration
{
    public float maxSpeed;
    public float rotationSpeed;
    public float steeringSpeed;  
    public float acceleration;

    public SumobotConfiguration()
    {       

    }

    public static SumobotConfiguration GetDefaultPlayerConfig()
    {
        SumobotConfiguration sc = new SumobotConfiguration();
        sc.maxSpeed = 5;
        sc.steeringSpeed = 5;
        sc.acceleration = 50;
        sc.rotationSpeed = 250;        

        return sc;
    }

    public SumobotConfiguration(float maxSpeed, float rotationSpeed, float steeringSpeed, float acceleration)
    {
        this.maxSpeed = maxSpeed;
        this.rotationSpeed = rotationSpeed;
        this.steeringSpeed = steeringSpeed;
        this.acceleration = acceleration;
    }
}
