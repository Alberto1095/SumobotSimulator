﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour,CombatListenner
{
    private RobotController robotA;
    private RobotController robotB;

    public enum CombatResult { WINA,WINB,DRAW};
    [HideInInspector]
    public CombatResult result;

    private bool started;
    private float startTime;
    private float maxTime;     

    // Start is called before the first frame update
    void Start()
    {
        started = false;
        //20 segundos
        maxTime = 20000;
    }

    // Update is called once per frame
    void Update()
    {
        CheckTimeFinished();
    }

    public void StartMatch(RobotController r1,RobotController r2)
    {
        robotA = r1;
        robotB = r2;

        robotA.listenner = this;
        robotB.listenner = this;

        SetRobotPositions();

        robotA.SetEnable(true);
        robotB.SetEnable(true);

        startTime = Time.time;
        started = true;
    }

    private void SetRobotPositions()
    {
        Vector3 center = transform.position;
        Vector3 pos1 = new Vector3(center.x, center.y + 1, center.z);
        Vector3 pos2 = new Vector3(center.x, center.y - 1, center.z);
        Quaternion rot1 = Quaternion.Euler(0, 0, -90);
        Quaternion rot2 = Quaternion.Euler(0, 0, 90);

        robotA.SetStartPosition(pos1, rot1);
        robotB.SetStartPosition(pos2, rot2);
    }
    
    private void CheckTimeFinished()
    {
        if (started)
        {
            float elapsed = Time.time - startTime;
            if(elapsed > maxTime)
            {
                started = false;

                robotA.SetEnable(false);
                robotB.SetEnable(false);

                result = CombatResult.DRAW;
                SetResult();           }
        }       
    }

    private void SetResult()
    {
        switch (result)
        {
            case CombatResult.DRAW:
                robotA.SetWin(false);
                robotB.SetWin(false);
                break;
            case CombatResult.WINA:
                robotA.SetWin(true);
                robotB.SetWin(false);
                break;
            case CombatResult.WINB:
                robotA.SetWin(false);
                robotB.SetWin(true);
                break;
        }
    }

    public void OnDeath(RobotController rc)
    {
        if(rc == robotA)
        {
            //Robot B wins
            robotB.SetEnable(false);
            result = CombatResult.WINB;
        }
        else
        {
            //Robot A wins
            robotA.SetEnable(false);
            result = CombatResult.WINA;
        }

        SetResult();
    }
}
