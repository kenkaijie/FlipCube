using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class GameTileController : IGameTileController
{
    private bool tileActivated = false;
    private IGameTile gameTile;

    public GameTileController(IGameTile obj)
    {
        gameTile = obj;
    }

    public void ActivateTile()
    {
        gameTile.SetTileColor(new Color(0.8982f, 0.129f, 0.129f, 0.502f), 0.2f);
        tileActivated = true;
    }

    public void DeactivateTile()
    {
        gameTile.SetTileColor(new Color(0.525f, 0.957f, 0.549f, 0.502f), 0.2f);
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

    public void DespawnTile()
    {
        gameTile.DespawnTile();
    }

    public void SetTileColor(Color color)
    {
        gameTile.SetTileColor(color, 0.2f);
    }
}