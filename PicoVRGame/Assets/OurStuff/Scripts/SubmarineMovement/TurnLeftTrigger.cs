using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnLeftTrigger : MonoBehaviour
{
    private Vector3 buttonOrigin;
    [SerializeField] private GameObject turnLeftButton;
    public bool turnLeftPressed = false;

    [SerializeField] private float maxRotationSpeed = 5;
    [SerializeField] private float rotationIncrease = 1f;

    public float rotationSpeed = 0;


    private void Start()
    {
        buttonOrigin = turnLeftButton.transform.localPosition;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerHand"))
        {
            turnLeftPressed = true;
            turnLeftButton.transform.localPosition = new Vector3(0, buttonOrigin.y - 0.09f, 0);

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
            turnLeftPressed = false;
            rotationSpeed = 0;
            turnLeftButton.transform.localPosition = new Vector3(0, buttonOrigin.y, 0);           
        }
    }
}
