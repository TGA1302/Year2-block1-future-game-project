using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ForwardBackwardLever2 : MonoBehaviour
{
    [SerializeField] private XRNode inputSourceLeftHand;
    [SerializeField] private XRNode inputSourceRightHand;
    private bool leftButtonPressed = false;
    private bool rightButtonPressed = false;
    private bool bothButtonsPressed = false;

    public bool isDriving = false;
    public float forwardBackwardTilt = 0f;
    private float startRotation;

    [SerializeField] private GameObject topOfLever;

    [SerializeField] private GameObject rightHand;
    [SerializeField] private GameObject leftHand;

    [SerializeField] private GameObject rightHandPrefab;
    [SerializeField] private GameObject leftHandPrefab;

    public GameObject rotationControl;

    private void Start()
    {
        startRotation = transform.localRotation.eulerAngles.x;
    }

    private void Update()
    {
        InputDevice leftDevice = InputDevices.GetDeviceAtXRNode(inputSourceLeftHand);
        InputDevice rightDevice = InputDevices.GetDeviceAtXRNode(inputSourceRightHand);
        leftDevice.TryGetFeatureValue(CommonUsages.triggerButton, out leftButtonPressed);
        rightDevice.TryGetFeatureValue(CommonUsages.triggerButton, out rightButtonPressed);



        //Calculating the Tilt Strength

        float currentTilt = transform.rotation.eulerAngles.z;

        if (currentTilt <= 355 && currentTilt >= 327)
        {
            float tiltStrength = Mathf.Abs(currentTilt - 360);
            forwardBackwardTilt = -tiltStrength;
            isDriving = true;
            Debug.Log("BackwardSpeed" + forwardBackwardTilt);
        }

        else if (currentTilt <= 33 && currentTilt >= 5)
        {
            forwardBackwardTilt = currentTilt;
            isDriving = true;
            Debug.Log("ForwardSpeed" + forwardBackwardTilt);
        }

        else if (currentTilt > 33 && currentTilt < 327 || currentTilt < 5 && currentTilt >= 0 || currentTilt > 355 && currentTilt <= 360)
        {
            forwardBackwardTilt = 0;
            isDriving = false;
            Debug.Log("TiltMore");
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("RightHand") && rightButtonPressed == true)
        {
            other.GetComponent<HandScript>().isGrabbing = true;
            other.GetComponent<HandScript>().grabbedObjectID = GetInstanceID();
        }

        else if (other.CompareTag("RightHand") && rightButtonPressed == false)
        {
            other.GetComponent<HandScript>().isGrabbing = false;
            other.GetComponent<HandScript>().grabbedObjectID = 0;
        }

        else if (other.CompareTag("LeftHand") && leftButtonPressed == true)
        {
            other.GetComponent<HandScript>().isGrabbing = true;
            other.GetComponent<HandScript>().grabbedObjectID = GetInstanceID();
        }

        else if (other.CompareTag("LeftHand") && leftButtonPressed == false)
        {
            other.GetComponent<HandScript>().isGrabbing = false;
            other.GetComponent<HandScript>().grabbedObjectID = 0;
        }

        else if (other.CompareTag("LeftHand") && leftButtonPressed == true && other.CompareTag("RightHand") && rightButtonPressed == true)
        {
            bothButtonsPressed = true;
        }

        else if (other.CompareTag("LeftHand") && leftButtonPressed == false && other.CompareTag("RightHand") && rightButtonPressed == false)
        {
            bothButtonsPressed = false;
        }

        else
        {
            return;
        }


        if (other.GetComponent<HandScript>().isGrabbing == true && other.GetComponent<HandScript>().grabbedObjectID == GetInstanceID() && bothButtonsPressed == false)
        {
            other.GetComponent<HandScript>().handPrefab.transform.position = topOfLever.transform.position;
            Vector3 vector3Up = other.transform.position - transform.position;
            Vector3 rotationControler = (transform.position - rotationControl.transform.position).normalized;

            CalculateRotation(rotationControler, vector3Up);
        }

        else if (other.GetComponent<HandScript>().isGrabbing == true && other.GetComponent<HandScript>().grabbedObjectID == GetInstanceID() && bothButtonsPressed == true)
        {
            other.GetComponent<HandScript>().handPrefab.transform.position = topOfLever.transform.position;
            Vector3 leftHandToLever = leftHand.transform.position - transform.position;
            Vector3 rightHandToLever = rightHand.transform.position - transform.position;
            Vector3 middlePointOfHands = (leftHandToLever + rightHandToLever) / 2;
            Vector3 rotationController = (transform.position - rotationControl.transform.position).normalized;

            CalculateRotation(rotationController, middlePointOfHands);
        }
    }


    private void CalculateRotation(Vector3 directionForward, Vector3 directionUpward)
    {
        Vector3 rotation = Quaternion.LookRotation(directionForward, directionUpward).eulerAngles;

        if (rotation.z >= 0 && rotation.z <= 33 || rotation.z >= 327 && rotation.z <= 360)
        {
            transform.rotation = Quaternion.Euler(rotation);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("RightHand"))
        {
            other.GetComponent<HandScript>().handPrefab.transform.position = other.transform.position;
            other.GetComponent<HandScript>().handPrefab.transform.rotation = other.transform.rotation;
            other.GetComponent<HandScript>().isGrabbing = false;
            other.GetComponent<HandScript>().grabbedObjectID = 0;
        }

        if (other.CompareTag("LeftHand"))
        {
            other.GetComponent<HandScript>().handPrefab.transform.position = other.transform.position;
            other.GetComponent<HandScript>().handPrefab.transform.rotation = other.transform.rotation;
            other.GetComponent<HandScript>().isGrabbing = false;
            other.GetComponent<HandScript>().grabbedObjectID = 0;
        }
    }
}
