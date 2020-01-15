using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SumobotConfiguration
{
    public float moveSpeed;
    public float rotationSpeed;
    public float lineSensorDistance;
    public float distanceSensorDistance;

    public bool useFrontDistanceSensor;
    public bool useLeftDistanceSensor;
    public bool useRightDistanceSensor;

    public bool useFrontRightLineSensor;
    public bool useFrontLeftLineSensor;
    public bool useFrontLineSensor;
    public bool useBackLineSensor;

    public SumobotConfiguration()
    {

    }

    public SumobotConfiguration(float moveSpeed, float rotationSpeed, float lineSensorDistance, 
        float distanceSensorDistance, bool useFrontDistanceSensor, bool useLeftDistanceSensor, 
        bool useRightDistanceSensor, bool useFrontRightLineSensor, bool useFrontLeftLineSensor, 
        bool useFrontLineSensor, bool useBackLineSensor)
    {
        this.moveSpeed = moveSpeed;
        this.rotationSpeed = rotationSpeed;
        this.lineSensorDistance = lineSensorDistance;
        this.distanceSensorDistance = distanceSensorDistance;
        this.useFrontDistanceSensor = useFrontDistanceSensor;
        this.useLeftDistanceSensor = useLeftDistanceSensor;
        this.useRightDistanceSensor = useRightDistanceSensor;
        this.useFrontRightLineSensor = useFrontRightLineSensor;
        this.useFrontLeftLineSensor = useFrontLeftLineSensor;
        this.useFrontLineSensor = useFrontLineSensor;
        this.useBackLineSensor = useBackLineSensor;
    }
}
