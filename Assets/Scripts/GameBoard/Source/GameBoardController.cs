using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameBoardController : IGameBoardController
{
    private IGameBoard gameBoard;
    private Dictionary<Vector3, ITileState> tileStates;

    public GameBoardController(IGameBoard obj, List<Vector3> coordinates, ITileState defaultTileState)
    {
        gameBoard = obj;
        tileStates = new Dictionary<Vector3, ITileState>();
        foreach (Vector3 point in coordinates)
        {
            tileStates.Add(point, defaultTileState);
        }
    }

    public bool CheckPositionBounds(Vector3 position)
    {
        if (tileStates.ContainsKey(position))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetTileColor(Vector3 position, ITileState tileState)
    {
        if (CheckPositionBounds(position))
        {
            gameBoard.SetTileState(position, tileState);
            tileStates[position] = tileState;
        }
    }

    public void GenerateBoard()
    {
        gameBoard.GenerateBoard(tileStates.Keys.ToList());
    }

    public void TearDownBoard()
    {
        gameBoard.TearDownBoard();
    }

    public int GetTileStateCount(ITileState tileState)
    {
        int count = 0;
        foreach (KeyValuePair<Vector3,ITileState> item in tileStates)
        {
            if (item.Value.Equals(tileState))
            {
                count++;
            }
        }

        return count;
    }

    public void SetTileState(Vector3 position, ITileState newTileState)
    {
        if (tileStates.ContainsKey(position))
        {
            if (!tileStates[position].Equals(newTileState))
            {
                gameBoard.SetTileState(position, newTileState);
                tileStates[position] = newTileState;
            }
        }
        else
        {
            Debug.LogError("Attempted to access a key that is not in the dictionary, Key:" + position);
            throw new KeyNotFoundException();
        }
    }
}