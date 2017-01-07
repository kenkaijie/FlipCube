using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameBoard
{
    // given a list of coordinates, we will generate the board
    IEnumerator GenerateBoard(List<Vector3> points);
    IEnumerator GenerateBoard(List<Vector3> points, float maxSpawnDelay);

    // removes all tiles from the board
    void TearDownBoard();
}