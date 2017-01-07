using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class GameTileController :  IGameTileController
{
    private bool tileActivated = false;
    private IGameTile gameTile;

    public GameTileController(IGameTile obj)
    {
        gameTile = obj;
    }

    public void ActivateTile()
    {
        gameTile.ActivateTile();
        tileActivated = true;
    }

    public void DeactivateTile()
    {
        gameTile.DeactivateTile();
        tileActivated = false;
    }

    // contains a state machine for the game tile
    public bool IsActivated()
    {
        return tileActivated;
    }

    public Vector3 GetPosition()
    {
        return gameTile.GetPosition();
    }

}