﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public interface IGameBoardController
{
    // given a list of coordinates, we will generate the board
    void GenerateBoard();

    // removes all tiles from the board
    void TearDownBoard();

    // counts the number of tiles with the same color
    int GetTileStateCount(ITileState tileState);

    //set tile state
    void SetTileState(Vector3 position, ITileState tileState);

    // check whether the co-ordinate is populated
    bool CheckPositionBounds(Vector3 posiiton);
}