using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameTileController : MonoBehaviour
{
    public enum GameTileState
    {
        ACTIVE,
        INACTIVE
    }

    public GameObject lightingCube;
    public GameObject lightingBack;

    public Renderer lightingCubeTexture;

    public Color on;
    public Color off;

    public GameTileState tileState;

    private GameTileState lastProcessedState;

    // RED E1212180
    // GREEN 86F48C80
    // BLUE 21B9E180

    // Use this for initialization
    void Start()
    {
        tileState = GameTileState.INACTIVE;
        lastProcessedState = GameTileState.ACTIVE;
    }



    // Update is called once per frame
    void Update()
    {
        if (tileState != lastProcessedState)
        {

            switch (tileState)
            {
                case GameTileState.ACTIVE:
                    lightingCubeTexture.material.color = on;
                    break;
                case GameTileState.INACTIVE:
                    lightingCubeTexture.material.color = off;
                    break;
            }
            lastProcessedState = tileState;
        }
        
    }

    private void ToggleTileState()
    {
        switch (tileState)
        {
            case GameTileState.ACTIVE:
                tileState = GameTileState.INACTIVE;
                break;
            case GameTileState.INACTIVE:
                tileState = GameTileState.ACTIVE;
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ToggleTileState();
    }

}
