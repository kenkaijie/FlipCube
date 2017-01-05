using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerController : MonoBehaviour {

    public enum PlayerControllerState
    {
        CREATED, // initial state
        IDLE, // no input is needed, hold position
        PREP, // an input was sent, setup vectors to actuate
        BUSY // current in the middle of tumbling
    }

    public enum TumblingDirection
    {
        UP,DOWN,LEFT,RIGHT, NONE
    }

    // completion callback
    public delegate void OnFinishCallback();

    // configuration parameters
    public float magnitude;

    public Vector3 baseForceVector;

    // common variables
    private Rigidbody rb;
    private Vector3 forceVector;
    private float rotationModifier = 0f;
    private Vector3 appliedVector = Vector3.zero;
    private Quaternion appliedRotation = Quaternion.identity;
    private OnFinishCallback callback = null;

    // cube positions
	public Vector3 previousPosition {get; private set;}
	public Vector3 targetPosition {get; private set;}

	public Quaternion previousRotation {get; private set;}
	public Quaternion targetRotation {get; private set;}

    // component state
    private PlayerControllerState state = PlayerControllerState.CREATED;

	private void Start () {
        rb = GameObject.Find("Cube").GetComponent<Rigidbody>();
        previousPosition = rb.position;
        previousRotation = rb.rotation;
    }

    private bool CheckReady()
    {
        if (rb.velocity == Vector3.zero && rb.angularVelocity == Vector3.zero)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public bool SetTumblingDirection(TumblingDirection dir, OnFinishCallback handler)
    {
        if (state==PlayerControllerState.IDLE)
        {
            switch (dir)
            {
                case TumblingDirection.UP:
                    rotationModifier = 0f;
                    appliedVector = Vector3.forward;
                    appliedRotation = Quaternion.AngleAxis(90f, Vector3.right);
                    break;
                case TumblingDirection.DOWN:
                    rotationModifier = 180f;
                    appliedVector = Vector3.back;
                    appliedRotation = Quaternion.AngleAxis(-90f, Vector3.right);
                    break;
                case TumblingDirection.LEFT:
                    rotationModifier = 270f;
                    appliedVector = Vector3.left;
                    appliedRotation = Quaternion.AngleAxis(-90f, Vector3.forward);
                    break;
                case TumblingDirection.RIGHT:
                    rotationModifier = 90f;
                    appliedVector = Vector3.right;
                    appliedRotation = Quaternion.AngleAxis(90f, Vector3.forward);
                    break;
                case TumblingDirection.NONE:
                    rotationModifier = 0f;
                    appliedVector = Vector3.zero;
                    appliedRotation = Quaternion.identity;
                    break;

            }
            callback = handler;
            TransitionToState(PlayerControllerState.PREP);
            return true;
        }
        else
        {
            return false;
        }
    }

	public void FreezePlayer()
	{
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
	}

	// Update is called once per frame
	private void Update () {
        // large state machine

        switch (state)
        {
            case PlayerControllerState.CREATED:
                if (CheckReady())
                {
                    TransitionToState(PlayerControllerState.IDLE);
                }
                else
                {
                    //do nothing
                }
                break;
            case PlayerControllerState.IDLE:
                break;
            case PlayerControllerState.PREP:
                previousPosition = rb.position;
                previousRotation = rb.rotation;

                targetPosition = previousPosition + appliedVector;
                targetRotation = previousRotation * appliedRotation;

                forceVector = Quaternion.Euler(0f, rotationModifier, 0f) * baseForceVector;

                TransitionToState(PlayerControllerState.BUSY);

                Debug.Log("Applying Force: " + forceVector.ToString());
                Debug.Log("Position Previous: " + previousPosition.ToString() + ", Target: " + targetPosition.ToString());
                Debug.Log("Rotation Previous: " + previousRotation.ToString() + ", Target: " + targetRotation.ToString());
                break;

            case PlayerControllerState.BUSY:
                // do nothing, all done in physics
                break;
        }
	}

    private void TransitionToState(PlayerControllerState newState)
    {
        Debug.Log("Transitioning state (" + state.ToString() + " -> " + newState.ToString() + ")", gameObject);
        switch (newState)
        {
            case PlayerControllerState.CREATED:
                // do nothing, we cant transition back to this state
                break;
            case PlayerControllerState.IDLE:
                if (state == PlayerControllerState.CREATED || state == PlayerControllerState.BUSY)
                {
                    state = PlayerControllerState.IDLE;
                }
                else
                {
                    // do nothing, will get caught
                }
                break;
            case PlayerControllerState.PREP:
                // do nothing
                if (state == PlayerControllerState.IDLE)
                {
                    state = PlayerControllerState.PREP;
                }
                else
                {
                    // do nothing, will get caught
                }
                break;
            case PlayerControllerState.BUSY:
                if (state == PlayerControllerState.PREP)
                {
                    state = PlayerControllerState.BUSY;
                }
                else
                {
                    // do nothing, will get caught
                }
                break;
        }
        if (state != newState)
        {
            Debug.LogError("Incorrect transition. ", gameObject);
        }
    }

    private void FixedUpdate()
    {

        switch (state)
        {
            case PlayerControllerState.CREATED:
                // do nothing
                break;
            case PlayerControllerState.IDLE:
                // do nothing
                break;
            case PlayerControllerState.PREP:
                // do nothing
                break;
            case PlayerControllerState.BUSY:
                rb.AddForce(magnitude * forceVector);
                if ((rb.position - targetPosition).sqrMagnitude <= 0.1)
                {
 
					FreezePlayer();

                    rb.position = targetPosition;
                    rb.rotation = targetRotation;

                    Debug.Log("Done!");

                    if (callback != null)
                    {
                        callback.Invoke();
                    }

                    TransitionToState(PlayerControllerState.IDLE);

                }
                break;
        }
    }
}
