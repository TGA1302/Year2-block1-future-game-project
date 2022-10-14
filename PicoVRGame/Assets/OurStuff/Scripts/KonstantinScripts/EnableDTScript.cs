using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

[ExecuteInEditMode]
public class EnableDTScript : MonoBehaviour
{
    [SerializeField] private XRNode inputSourceLeftHand;
    [SerializeField] private XRNode inputSourceRightHand;
    private bool leftButtonPressed = false;
    private bool rightButtonPressed = false;
    private bool bothButtonsPressed = false;

    [SerializeField] private Transform rightHand;
    [SerializeField] private Transform leftHand;

    [SerializeField] private GameObject rightHandPrefab;
    [SerializeField] private GameObject leftHandPrefab;

    public Transform Wheel;
    public float WheelRotation = 0f;
    float lastXVal;
    private bool isDecreased;

    [SerializeField] private List<Transform> snappingPositions;
    [SerializeField] private Transform rotationControl;


    private void Start()
    {
        lastXVal = WheelRotation;
    }
    void Update()
    {
        InputDevice leftDevice = InputDevices.GetDeviceAtXRNode(inputSourceLeftHand);
        InputDevice rightDevice = InputDevices.GetDeviceAtXRNode(inputSourceRightHand);
        leftDevice.TryGetFeatureValue(CommonUsages.triggerButton, out leftButtonPressed);
        rightDevice.TryGetFeatureValue(CommonUsages.triggerButton, out rightButtonPressed);

        float x = Input.GetAxis("Horizontal"); //get the wheel to move on A&D but we need vr input
        WheelRotation -= x;
        WheelRotation = Mathf.Clamp(WheelRotation, 0f, float.PositiveInfinity); // clamps the rotation turn left to infinity and turn right up to start position (0)
        transform.localRotation = Quaternion.Euler(WheelRotation, 0f, 0f);

        if (WheelRotation > lastXVal)
        {
            isDecreased = false;
            Debug.Log("Increased");
            lastXVal = WheelRotation;

        }
        else if (WheelRotation < lastXVal)
        {
            isDecreased = true;
            Debug.Log("Decreased");
            lastXVal = WheelRotation;
        }



        if(rightHand.GetComponent<HandScript>().isGrabbing == true && leftHand.GetComponent<HandScript>().isGrabbing == true)
        {
            Quaternion newRotationLeftHand = Quaternion.Euler(leftHand.transform.rotation.eulerAngles.x, 0, 0);
            Quaternion newRotationRightHand = Quaternion.Euler(rightHand.transform.rotation.eulerAngles.x, 0, 0);
            Quaternion finalRotation = Quaternion.Slerp(newRotationLeftHand, newRotationRightHand, 0.5f);
            rotationControl.rotation = finalRotation;
            transform.parent = rotationControl;
            
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("RightHand") && rightButtonPressed == true)
        {

            PlaceHandOnWheel(rightHandPrefab, rightHand, other.gameObject);
        }

        if (other.CompareTag("LeftHand") && leftButtonPressed == true)
        {
            PlaceHandOnWheel(leftHandPrefab, leftHand, other.gameObject);
        }
    }

    private void PlaceHandOnWheel(GameObject handPrefab, Transform hand, GameObject collider)
    {
        float shortestDistance = Vector3.Distance(snappingPositions[0].position, hand.position);
        Transform bestSnap = snappingPositions[0];

        foreach(Transform snappPosition in snappingPositions)
        {
            if (snappPosition.childCount == 0)
            {
                float distance = Vector3.Distance(snappPosition.position, hand.position);

                if(distance < shortestDistance)
                {
                    shortestDistance = distance;
                    bestSnap = snappPosition;
                }
            }
        }

        handPrefab.transform.parent = bestSnap.transform;
        handPrefab.transform.position = bestSnap.transform.position;

        collider.GetComponent<HandScript>().isGrabbing = true;
    }
}