using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetValueControls : MonoBehaviour
{
    public bool buttonA;
    public bool buttonB;
    public bool buttonMenu;
    public bool buttonLeft;
    public bool buttonRight;
    public bool buttonUp;
    public bool buttonDown;

    public float horizontalAxis;
    public float verticalAxis;

    // Button A & B
    public void SetButtonA(bool val)
    {
        buttonA = val;
    }

    public void SetButtonB(bool val)
    {
        buttonB = val;
    }
    public void SetButtonMenu(bool val)
    {
        buttonMenu = val;
    }

    // Button direction
    public void SetButtonUp(bool val)
    {
        buttonUp = val;
        verticalAxis = (val ? 1 : 0);
    }

    public void SetButtonDown(bool val)
    {
        buttonDown = val;
        verticalAxis = (val ? -1 : 0);
    }

    public void SetButtonLeft(bool val)
    {
        buttonLeft = val;
        horizontalAxis = (val ? -1 : 0);
    }

    public void SetButtonRight(bool val)
    {
        buttonRight = val;
        horizontalAxis = (val ? 1 : 0);
    }
}
