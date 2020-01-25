using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SumobotConfiguration
{
    public float moveSpeed;
    public float rotationSpeed;
    

    public SumobotConfiguration()
    {       

    }

    public static SumobotConfiguration GetDefaultPlayerConfig()
    {
        SumobotConfiguration sc = new SumobotConfiguration();
        sc.moveSpeed = 6;
        sc.rotationSpeed = 200;        

        return sc;
    }

    public SumobotConfiguration(float moveSpeed, float rotationSpeed)
    {
        this.moveSpeed = moveSpeed;
        this.rotationSpeed = rotationSpeed;
    }
}
