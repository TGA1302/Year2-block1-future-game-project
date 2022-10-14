using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class MovementTypes : MonoBehaviour
{
    private Vector2 inputAxis;
    private bool gripButtonPressed;
    private CharacterController character;
    [SerializeField] private XROrigin rig;
    [SerializeField] XRNode inputSource;
    [SerializeField] private float addCamHeight = 0.2f;

    [SerializeField] XRRayInteractor teleportRay;

    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private float gravityStrength = -9f;
    [SerializeField] private float fallingSpeed = -8f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] private float addRayLength = 0.1f;

    [SerializeField] private int movementType;
    private bool locked = false;

    [SerializeField] private TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);
        device.TryGetFeatureValue(CommonUsages.gripButton, out gripButtonPressed);
        
        if (gripButtonPressed == true && locked == false)
        {
            locked = true;
            movementType += 1;
            if (movementType == 3)
            {
                movementType = 0;
            }
        }

        if (gripButtonPressed == false)
        {
            locked = false;
        }
    }

    private void FixedUpdate()
    {
        switch(movementType)
        {
            case 0:
                teleportRay.gameObject.SetActive(false);
                ContinuousMovement();
                WriteMovementType("Continuous Movement");
                break;

            case 1:
                teleportRay.gameObject.SetActive(true);
                WriteMovementType("Area & Anchor Teleportation");
                break;

            case 2:
                ContinuousMovement();
                WriteMovementType("Both Together");
                break;
        }

        CapsuleFollowHeadset();
        Gravity();        
    }

    void ContinuousMovement()
    {
        Quaternion headDirection = Quaternion.Euler(0, rig.Camera.transform.eulerAngles.y, 0);
        Vector3 direction = headDirection * new Vector3(inputAxis.x, 0, inputAxis.y);
        character.Move(direction * Time.fixedDeltaTime * movementSpeed);
    }

    void WriteMovementType(string movementName)
    {
        text.text = movementName;
    }

    void Gravity()
    {
        bool isGrounded = CheckIfGrounded();
        if (isGrounded == true)
        {
            fallingSpeed = 0;
        }
        else
        {
            fallingSpeed += gravityStrength * Time.fixedDeltaTime;
        }

        character.Move(Vector3.up * fallingSpeed * Time.fixedDeltaTime);
    }

    bool CheckIfGrounded()
    {
        Vector3 rayOrigin = transform.TransformPoint(character.center);
        float rayLength = character.center.y + addRayLength;
        bool hitGround = Physics.SphereCast(rayOrigin, character.radius, Vector3.down, out RaycastHit hitInformation, rayLength, groundLayer);
        Debug.Log(hitGround);

        return hitGround;
    }

    void CapsuleFollowHeadset()
    {
        character.height = rig.CameraInOriginSpaceHeight + addCamHeight;
        Vector3 capsuleCenter = transform.InverseTransformPoint(rig.Camera.transform.position);
        character.center = new Vector3(capsuleCenter.x, character.height / 2 + character.skinWidth, capsuleCenter.z);
    }
}
