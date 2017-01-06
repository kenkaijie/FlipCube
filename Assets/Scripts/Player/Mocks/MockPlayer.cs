using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MockPlayer : IPlayer
{

    public bool failTumble;
    private Vector3 currentPosition;

    private PlayerState state = PlayerState.UNINITIALISED;

    private StateCallback<PlayerState> stateChangeCallback = null;

    public void Despawn()
    {
        return;
    }

    public Vector3 GetCurrentPosition()
    {
        throw new NotImplementedException();
    }

    public Quaternion GetCurrentRotation()
    {
        throw new NotImplementedException();
    }

    public void SetOnStateChangeHandler(StateCallback<PlayerState> callback)
    {
        stateChangeCallback = callback;
    }

    public void Spawn(Vector3 position)
    {
        currentPosition = position;
        return;
    }

    public void StartTumbling(TumblingDirection direction, EventCallback callback)
    {
        if (!failTumble)
        {
            switch (direction)
            {
                case TumblingDirection.UP:
                    currentPosition += Vector3.forward;
                    break;
                case TumblingDirection.DOWN:
                    currentPosition += Vector3.back;
                    break;
                case TumblingDirection.LEFT:
                    currentPosition += Vector3.left;
                    break;
                case TumblingDirection.RIGHT:
                    currentPosition += Vector3.right;
                    break;
                case TumblingDirection.NONE:
                    currentPosition += Vector3.zero;
                    break;
            }
            callback.Invoke(EventCallbackError.NOERROR);
        }
        else
        {
            callback.Invoke(EventCallbackError.ERROR_INCORRECT_PLAYER_STATE);
        }

    }

    public void TransitionToState(PlayerState newState)
    {
        throw new NotImplementedException();
    }

    public PlayerState GetState()
    {
        return state;
    }

}