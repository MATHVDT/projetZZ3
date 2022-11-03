using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputTest : MonoBehaviour
{
    public bool buttonA;
    public bool buttonB;

    public float horizontalAxis;
    public float verticalAxis;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(horizontalAxis + ", " + verticalAxis);
    }

    public void OnButtonA(InputValue input)
    {
        //Debug.Log("OnButtonA()");
        buttonA = input.isPressed;
    }

    public void OnButtonB(InputValue input)
    {
        //Debug.Log("OnButtonB()");
        buttonB = input.isPressed;
    }

    public void OnHorizontal(InputValue val)
    {
        //Debug.Log("OnHorizontal() : " + val);
        horizontalAxis = val.Get<float>();
    }

    public void OnVertical(InputValue val)
    {
        //Debug.Log("OnVertical() : " + val);
        verticalAxis = val.Get<float>();
    }

}
