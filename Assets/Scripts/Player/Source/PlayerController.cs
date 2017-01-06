using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PlayerController: IPlayerController
{
    // positional
    private Vector3 playerCurrentPosition;
    private Vector3 playerTargetPosition;
    private IPlayer player = null;
    private LogicControllerState state;
    private StateCallback<LogicControllerState> stateChangeCallback = null;
    private EventCallback eventCallback = null;


    public PlayerController(IPlayer playerObj)
    {
        Initiliase(playerObj);
    }

    public PlayerController(IPlayer playerObj, StateCallback<LogicControllerState> callback)
    {
        Initiliase(playerObj);
        stateChangeCallback = callback;
    }

    private void Initiliase(IPlayer playerObj)
    {
        state = LogicControllerState.CREATED;
        player = playerObj;
        player.SetOnStateChangeHandler(OnPlayerStateChange);
        // if the player has already initailised itself, note that it cannot be any other state, as nothing "should"
        // directly interact with player
        if (playerObj.GetState() == PlayerState.IDLE)
        {
            TransitionToState(LogicControllerState.IDLE);
        }
    }

    public void StartMovePlayer(TumblingDirection direction, EventCallback callback)
    {
        if (state == LogicControllerState.IDLE)
        {
            player.StartTumbling(direction, OnPlayerMoveFinish);
            eventCallback = callback;
            state = LogicControllerState.BUSY;
        }
    }

    private void OnPlayerMoveFinish(EventCallbackError errorCode)
    {
        if (errorCode == EventCallbackError.NOERROR)
        {
            playerCurrentPosition = playerTargetPosition;
            if (eventCallback != null)
            {
                eventCallback.Invoke(EventCallbackError.NOERROR);
            }
        }
        else
        {
            if (errorCode != EventCallbackError.ERROR_INCORRECT_PLAYER_STATE)
            {
                Debug.LogError("Error trying to finish player move, " + errorCode);
                TransitionToState(LogicControllerState.ERROR);
                if (eventCallback != null)
                {
                    eventCallback.Invoke(EventCallbackError.ERROR_INCORRECT_PLAYER_STATE);
                }
            }
           
        }
        
    }

    public Vector3 GetPlayerCurrentPosition()
    {
        return playerCurrentPosition;
    }

    public Vector3 GetPlayerTargetPosition()
    {
        return playerTargetPosition;
    }

    public void TransitionToState(LogicControllerState newState)
    {
        Debug.Log("Transitioning state (" + state.ToString() + " -> " + newState.ToString() + ")");
        switch (newState)
        {
            case LogicControllerState.CREATED:
                // we can;t transition here
                break;
            case LogicControllerState.IDLE:
                if (state == LogicControllerState.BUSY || state == LogicControllerState.CREATED)
                {
                    state = LogicControllerState.IDLE;
                }
                else
                {
                    // do nothing
                }
                break;
            case LogicControllerState.BUSY:
                if (state == LogicControllerState.IDLE)
                {
                    state = LogicControllerState.BUSY;
                }
                else
                {
                    // do nothing
                }
                break;
            case LogicControllerState.ERROR:
                Debug.LogError("State transitioned into error!");
                break;
            default:
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

    public void SetOnStateChangeHandler(StateCallback<LogicControllerState> callback)
    {
        stateChangeCallback = callback;
    }

    private void OnPlayerStateChange(PlayerState newState, StateTransitionError errorCode)
    {
        if (errorCode == StateTransitionError.NOERROR)
        {
            switch (newState)
            {
                case PlayerState.UNINITIALISED:
                    break;
                case PlayerState.CREATED:
                    break;
                case PlayerState.IDLE:
                    TransitionToState(LogicControllerState.IDLE);
                    break;
                case PlayerState.PREP:
                    break;
                case PlayerState.BUSY:
                    break;
                default:
                    break;
            }
        }
        else
        {
            TransitionToState(LogicControllerState.ERROR);
        }
    }

    public LogicControllerState GetState()
    {
        return state;
    }

}