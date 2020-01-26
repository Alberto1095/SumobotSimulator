using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotPlayerController : RobotController
{   

    protected override void UpdateSumobot()
    {
       CheckUserInput();       
    }

    private void CheckUserInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            yDirection = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            yDirection = -1;
        }
        else
        {
            yDirection = 0;
        }

        if (Input.GetKey(KeyCode.A))
        {
            xDirection = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            xDirection = 1;
        }
        else
        {
            xDirection = 0;
        }
    }

    public override void SetWin(bool b)
    {
        
    }

}
