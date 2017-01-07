using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IPlayer, IStateful<PlayerState>
{

    // configuration parameters
    private static float magnitude = 60f;
    private static Vector3 baseForceVector = new Vector3(0.0f, -0.6f, 1f);

    // common variables
    private Rigidbody rb;
    private Vector3 forceVector;
    private float rotationModifier = 0f;
    private Vector3 appliedVector = Vector3.zero;
    private Quaternion appliedRotation = Quaternion.identity;
    private EventCallback callback = null;

    // cube positions
	public Vector3 previousPosition {get; private set;}
	public Vector3 targetPosition {get; private set;}

	public Quaternion previousRotation {get; private set;}
	public Quaternion targetRotation {get; private set;}

    PlayerState state = PlayerState.UNINITIALISED;

    private Animator anim;
    private StateCallback<PlayerState> stateChangeCallback = null;

    public void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponentInChildren<Rigidbody>();
        state = PlayerState.CREATED;
    }

    public void StartTumbling(TumblingDirection direction, EventCallback handler)
    {
        if (state==PlayerState.IDLE)
        {
            switch (direction)
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
            TransitionToState(PlayerState.PREP);
        }
        else
        {
            handler.Invoke(EventCallbackError.ERROR_INCORRECT_PLAYER_STATE);
        }
    }

	public void FreezePlayer()
	{
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
	}

    public void TransitionToState(PlayerState newState)
    {
        Debug.Log("Transitioning state (" + state.ToString() + " -> " + newState.ToString() + ")");
        switch (newState)
        {
            case PlayerState.UNINITIALISED:
                // do nothing, we cant transition back to this state
                break;
            case PlayerState.CREATED:
                if (state == PlayerState.UNINITIALISED)
                {
                    state = PlayerState.CREATED;
                }
                else
                {
                    // do nothing, will get caught
                }
                break;
            case PlayerState.IDLE:
                if (state == PlayerState.CREATED || state == PlayerState.BUSY)
                {
                    state = PlayerState.IDLE;
                }
                else
                {
                    // do nothing, will get caught
                }
                break;
            case PlayerState.PREP:
                // do nothing
                if (state == PlayerState.IDLE)
                {
                    state = PlayerState.PREP;
                }
                else
                {
                    // do nothing, will get caught
                }
                break;
            case PlayerState.BUSY:
                if (state == PlayerState.PREP)
                {
                    state = PlayerState.BUSY;
                }
                else
                {
                    // do nothing, will get caught
                }
                break;
        }
        if (state != newState)
        {
            Debug.LogError("Incorrect transition. ");
            if (stateChangeCallback != null)
            {
                stateChangeCallback.Invoke(state, StateTransitionError.ERROR_INVALID_TRANSITION);
            }
        }
        else
        {
            if (stateChangeCallback != null)
            {
                stateChangeCallback.Invoke(state, StateTransitionError.NOERROR);
            }
        }
    }

    public void SetOnStateChangeHandler(StateCallback<PlayerState> callback)
    {
        stateChangeCallback = callback;
    }

    // Update is called once per frame
    private void Update () {
        // large state machine

        switch (state)
        {
            case PlayerState.CREATED:
                if (CheckReady())
                {
                    TransitionToState(PlayerState.IDLE);
                }
                else
                {
                    //do nothing
                }
                break;
            case PlayerState.IDLE:
                break;
            case PlayerState.PREP:
                previousPosition = rb.position;
                previousRotation = rb.rotation;

                targetPosition = previousPosition + appliedVector;
                targetRotation = previousRotation * appliedRotation;

                forceVector = Quaternion.Euler(0f, rotationModifier, 0f) * baseForceVector;

                TransitionToState(PlayerState.BUSY);

                Debug.Log("Applying Force: " + forceVector.ToString());
                Debug.Log("Position Previous: " + previousPosition.ToString() + ", Target: " + targetPosition.ToString());
                Debug.Log("Rotation Previous: " + previousRotation.ToString() + ", Target: " + targetRotation.ToString());
                break;

            case PlayerState.BUSY:
                // do nothing, all done in physics
                break;
        }
	}

    public void Despawn()
    {
        if (state != PlayerState.UNINITIALISED)
        {
			Destroy(gameObject);
            TransitionToState(PlayerState.UNINITIALISED);
        }
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case PlayerState.CREATED:
                // do nothing
                break;
            case PlayerState.IDLE:
                // do nothing
                break;
            case PlayerState.PREP:
                // do nothing
                break;
            case PlayerState.BUSY:
                rb.AddForce(magnitude * forceVector);
                if ((rb.position - targetPosition).sqrMagnitude <= 0.1)
                {

                    FreezePlayer();

                    rb.position = targetPosition;
                    rb.rotation = targetRotation;

                    Debug.Log("Done!");

                    if (callback != null)
                    {
                        callback.Invoke(EventCallbackError.NOERROR);
                    }

                    TransitionToState(PlayerState.IDLE);

                }
                break;
        }
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

    public Quaternion GetCurrentRotation()
    {
        return previousRotation;
    }

    public Vector3 GetCurrentPosition()
    {
        return previousPosition;
    }

    public PlayerState GetState()
    {
        return state;
    }

    public void Teleport(Vector3 position, Quaternion rotation)
    {
        FreezePlayer();
        rb.position = position;
        //rb.rotation = rotation;
    }
    
    public void Destroy()
    {
        anim.SetTrigger("Despawn");
    }

}
