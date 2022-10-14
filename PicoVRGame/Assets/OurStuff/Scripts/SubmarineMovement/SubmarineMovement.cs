using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class SubmarineMovement : MonoBehaviour
{
    [SerializeField] private GameObject submarineGO;

    [SerializeField] private SubmarineStates submarineState;

    public delegate void StateSwitch();
    public static event StateSwitch StateSwitched;

    [SerializeField] private ForwardBackwardLever forwardBackwardLever;
    [SerializeField] private TurnLeftTrigger turnLeftButton;
    [SerializeField] private TurnRightTrigger turnRightButton;

    private float rotationDampener = 0.5f;
    public float speed = 2;


    private void Start()
    {
        UpdateSubmarineState(SubmarineStates.Driving);
    }


    private void FixedUpdate()
    {
        if (submarineState == SubmarineStates.Driving && forwardBackwardLever.isDriving == true)
        {
            submarineGO.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * forwardBackwardLever.forwardBackwardTilt * speed * Time.fixedDeltaTime);
        }

        if (submarineState == SubmarineStates.Driving)
        {
            if (turnLeftButton.turnLeftPressed == true && turnRightButton.turnRightPressed == false)
            {
                submarineGO.GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(0, -turnLeftButton.rotationSpeed * rotationDampener, 0));
            }

            else if (turnLeftButton.turnLeftPressed == false && turnRightButton.turnRightPressed == true)
            {
                submarineGO.GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(0, turnRightButton.rotationSpeed * rotationDampener, 0));
            }

            else if (turnLeftButton.turnLeftPressed == true && turnRightButton.turnRightPressed == true)
            {

            }
        }
    }


    public void UpdateSubmarineState(SubmarineStates newState)
    {
        submarineState = newState;
        switch (submarineState)
        {
            case SubmarineStates.Idle:
                break;

            case SubmarineStates.Driving:
                break;
        }

        //StateSwitched();
    }


    public enum SubmarineStates
    {
        Idle,
        Driving
    }
}
