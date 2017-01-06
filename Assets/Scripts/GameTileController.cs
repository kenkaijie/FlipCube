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

    public GameObject lightingCube;
    public GameObject lightingBack;

    public Renderer lightingCubeTexture;

    private Animator anim;

    public List<Color> tileColorsList = new List<Color>();
    private Dictionary<GameTileControllerState, Color> tileColors = new Dictionary<GameTileControllerState, Color>();

    public GameTileControllerState state = GameTileControllerState.CREATED;
    private GameTileControllerState lastProcessedState = GameTileControllerState.CREATED;

    // RED E1212180
    // GREEN 86F48C80
    // BLUE 21B9E180

    // Use this for initialization
    void Start()
    {
        anim = GetComponentInChildren<Animator>();

        int count = 0;

        if (tileColorsList.Count < sizeof(GameTileControllerState))
        {
            Debug.LogWarning("We have less colors than the amount of states.");
        }

        foreach (GameTileControllerState state in Enum.GetValues(typeof(GameTileControllerState)))
        {
            tileColors.Add(state, tileColorsList[count]);
            count++;
        }
        TransitionToState(GameTileControllerState.INACTIVE);


    }

    private void SetTileColor()
    {
        /*
        if (tileColors.ContainsKey(state))
        {
            lightingCubeTexture.material.color = tileColors[state];
        }
        else
        {
            Debug.LogWarning("Game tile state " + state + " does not contain a color definition, is this intended?");
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        if (state != lastProcessedState)
        {
            SetTileColor();
            lastProcessedState = state;
        }
        
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
