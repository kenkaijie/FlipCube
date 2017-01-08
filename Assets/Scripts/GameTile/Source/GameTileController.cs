using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class GameTileController : IGameTileController
{
    private IGameTile gameTile;
    private ITileState tileState;

    public GameTileController(IGameTile obj)
    {
        gameTile = obj;
        tileState = null;
    }
    
    public Vector3 GetPosition()
    {
        return gameTile.GetPosition();
    }

    public void DespawnTile()
    {
        gameTile.DespawnTile();
    }

    public void SetTileState(ITileState newTileState)
    {
        tileState = newTileState;
        gameTile.SetTileColor(newTileState.GetColor(), 0.2f);
    }

    public ITileState GetTileState()
    {
        return tileState;
    }

    public float GetSpawnTime()
    {
        return 1.1f;
    }

    public float GetDespawnTime()
    {
        return 1.1f;
    }
}