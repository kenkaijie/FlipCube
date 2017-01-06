using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTileController : MonoBehaviour
{
    public enum GameTileControllerState
    {
        CREATED,
        INACTIVE,
        ACTIVE
    }


    public GameTileControllerState state = GameTileControllerState.CREATED;

    private Animator anim;

    // RED E1212180
    // GREEN 86F48C80
    // BLUE 21B9E180

    // Use this for initialization
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        TransitionToState(GameTileControllerState.INACTIVE);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void ToggleTileState()
    {
        switch (state)
        {
            case GameTileControllerState.ACTIVE:
                TransitionToState(GameTileControllerState.INACTIVE);
                break;
            case GameTileControllerState.INACTIVE:
                TransitionToState(GameTileControllerState.ACTIVE);
                break;
            case GameTileControllerState.CREATED:
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ToggleTileState();
    }

    void TransitionToState(GameTileControllerState newState)
    {
        Debug.Log("Transitioning state (" + state.ToString() + " -> " + newState.ToString() + ")", gameObject);
        switch (newState)
        {
            case GameTileControllerState.CREATED:
                // we can't transition back to this ever
                break;
            case GameTileControllerState.INACTIVE:
                state = GameTileControllerState.INACTIVE;
                anim.SetInteger("ControllerState", (int)state);
                break;
            case GameTileControllerState.ACTIVE:
                state = GameTileControllerState.ACTIVE;
                anim.SetInteger("ControllerState", (int)state);
                break;
            default:
                break;
        }
        if (state != newState)
        {
            Debug.LogError("Incorrect transition. ", gameObject);
        }
    }

}
