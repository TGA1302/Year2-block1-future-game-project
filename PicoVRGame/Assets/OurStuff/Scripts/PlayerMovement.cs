using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 inputAxis;
    [SerializeField] XRNode inputSourceLeftHand;

    [SerializeField] private XROrigin rig;
    [SerializeField] private float addCamHeight = 0.2f;

    [SerializeField] private float movementSpeed = 2f;


    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSourceLeftHand);
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);

        Quaternion headDirection = Quaternion.Euler(0, rig.Camera.transform.eulerAngles.y, 0);
        Vector3 direction = headDirection * new Vector3(inputAxis.x, 0, inputAxis.y);
        rig.transform.Translate(direction * Time.deltaTime * movementSpeed);
    }    
}
