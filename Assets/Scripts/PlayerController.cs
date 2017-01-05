using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    // configuration parameters
    public float magnitude;

    public Vector3 baseForceVector;

    // common variables
    private Rigidbody rb;
    private Vector3 forceVector;

    // cube state
    private Vector3 previousPosition;
    private Vector3 targetPosition;

    private Quaternion previousRotation;
    private Quaternion targetRotation;

    // flags
    public bool tumbling = false;

	// Use this for initialization
	private void Start () {

        rb = GameObject.Find("Cube").GetComponent<Rigidbody>();

        previousPosition = rb.position;
        previousRotation = rb.rotation;

	}

	// Update is called once per frame
	private void Update () {

        float rotationModifier = -1f;

        Vector3 appliedVector = Vector3.zero;
        Quaternion appliedRotation = Quaternion.identity;

		if (!tumbling)
        {
            if (Input.GetButtonDown("Up"))
            {
                rotationModifier = 0f;
                appliedVector = Vector3.forward;
                appliedRotation = Quaternion.AngleAxis(90f, Vector3.right);
            }
            else if (Input.GetButtonDown("Down"))
            {
                rotationModifier = 180f;
                appliedVector = Vector3.back;
                appliedRotation = Quaternion.AngleAxis(-90f, Vector3.right);
            }
            else if (Input.GetButtonDown("Left"))
            {
                rotationModifier = 270f;
                appliedVector = Vector3.left;
                appliedRotation = Quaternion.AngleAxis(-90f, Vector3.forward);
            }
            else if (Input.GetButtonDown("Right"))
            {
                rotationModifier = 90f;
                appliedVector = Vector3.right;
                appliedRotation = Quaternion.AngleAxis(90f, Vector3.forward);
            }
            else
            {
                return;
            }

            if (rotationModifier!=-1f)
            {
                // set the current cube state to the initial cube

                // set out cube to be tumbling
                tumbling = true;

                previousPosition = rb.position;
                previousRotation = rb.rotation;

                targetPosition = previousPosition + appliedVector;
                targetRotation = previousRotation * appliedRotation;

                forceVector = Quaternion.Euler(0f,rotationModifier,0f) * baseForceVector;

                rotationModifier = -1f;

                Debug.Log("Applying Force: " + forceVector.ToString());
                Debug.Log("Position Previous: " + previousPosition.ToString() + ", Target: " + targetPosition.ToString());
                Debug.Log("Rotation Previous: " + previousRotation.ToString() + ", Target: " + targetRotation.ToString());

            }

        }
	}

    private void FixedUpdate()
    {

        if (tumbling)
        {
            Vector3 currentPosition = rb.position;
            Quaternion currentRotation = rb.rotation;

            rb.AddForce(magnitude*forceVector);

            if((currentPosition-targetPosition).sqrMagnitude <= 0.1)
            {
                tumbling = false;

                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                rb.position = targetPosition;
                rb.rotation = targetRotation;

                Debug.Log("Done!");

            }

        }

    }
}
