using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnRightTrigger : MonoBehaviour
{
    private Vector3 buttonOrigin;
    [SerializeField] private GameObject turnRightButton;
    public bool turnRightPressed = false;

    [SerializeField] private float maxRotationSpeed = 5;
    [SerializeField] private float rotationIncrease = 1f;

    public float rotationSpeed = 0;


    private void Start()
    {
        buttonOrigin = turnRightButton.transform.localPosition;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerHand"))
        {
            turnRightPressed = true;
            turnRightButton.transform.localPosition = new Vector3(0, buttonOrigin.y - 0.09f, 0);

            while (rotationSpeed < maxRotationSpeed)
            {
                rotationSpeed += rotationIncrease;
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerHand"))
        {
            turnRightPressed = false;
            rotationSpeed = 0;
            turnRightButton.transform.localPosition = new Vector3(0, buttonOrigin.y, 0);
        }
    }
}
