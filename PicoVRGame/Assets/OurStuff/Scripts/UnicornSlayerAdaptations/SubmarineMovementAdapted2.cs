using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class SubmarineMovementAdapted2 : MonoBehaviour
{
    [SerializeField] private GameObject submarineGO;

    [SerializeField] private SubmarineStates submarineState;

    public delegate void StateSwitch();
    public static event StateSwitch StateSwitched;

    [SerializeField] private ForwardBackwardLever2 forwardBackwardLever;
    [SerializeField] private LeftRightLever2 leftRightLever;

    private float rotationSpeed = 0.5f;
    public float movementSpeed = 0.5f;

    [SerializeField] private Transform carrot;

    private void Start()
    {
        UpdateSubmarineState(SubmarineStates.Driving);
    }


    private void Update()
    {
        if (submarineState == SubmarineStates.Driving && forwardBackwardLever.isDriving == true)
        {
            //carrot.transform.position = new Vector3(0, 0,forwardBackwardLever.forwardBackwardTilt / 2);
            Vector3 direction = Vector3.Lerp(Vector3.zero, Vector3.right, forwardBackwardLever.forwardBackwardTilt * movementSpeed * Time.deltaTime);
            transform.Translate(direction, Space.Self);
            //submarineGO.GetComponent<Rigidbody>().AddRelativeForce(Vector3.right * forwardBackwardLever.forwardBackwardTilt * speed * Time.fixedDeltaTime);
        }

        if (submarineState == SubmarineStates.Driving && leftRightLever.isDriving == true)
        {
            
            Vector3 rotationTarget = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + leftRightLever.leftRightTilt * Time.deltaTime, transform.eulerAngles.z);
            Quaternion newRotation = Quaternion.Euler(rotationTarget);
            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, rotationSpeed);
            //submarineGO.GetComponent<Rigidbody>().AddTorque(new Vector3(0, leftRightLever.leftRightTilt * speed * Time.fixedDeltaTime, 0));
        }

        //if (submarineState == SubmarineStates.Driving)
        //{
        //    if (turnLeftButton.turnLeftPressed == true && turnRightButton.turnRightPressed == false)
        //    {
        //        submarineGO.GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(0, -turnLeftButton.rotationSpeed * rotationDampener, 0));
        //    }

        //    else if (turnLeftButton.turnLeftPressed == false && turnRightButton.turnRightPressed == true)
        //    {
        //        submarineGO.GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(0, turnRightButton.rotationSpeed * rotationDampener, 0));
        //    }

        //    else if (turnLeftButton.turnLeftPressed == true && turnRightButton.turnRightPressed == true)
        //    {

        //    }
        //}
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
