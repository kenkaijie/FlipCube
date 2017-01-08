using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameBoard
{
    // given a list of coordinates, we will generate the board
    void GenerateBoard(List<Vector3> points);
    void GenerateBoard(List<Vector3> points, float maxSpawnDelay);

    // removes all tiles from the board
    void TearDownBoard();

    // Sets states of individual tiles
    void SetTileState(Vector3 position, ITileState newTileState);

    // properties
    int GetTileCount();
}