using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum TumblingDirection
{
    UP, DOWN, LEFT, RIGHT, NONE
}

public enum PlayerState
{
    UNINITIALISED, // uninitialised object, default state
    CREATED, // initialised an near ready to take input
    IDLE, // no input is needed, hold position
    PREP, // an input was sent, setup vectors to actuate
    BUSY // current in the middle of tumbling
}

// Interface for player objects, players can be spawned, moved and created.
public interface IPlayer : IStateful<PlayerState>
{
    void StartTumbling(TumblingDirection direction, EventCallback callback);
    void Teleport(Vector3 position, Quaternion rotation);
    Quaternion GetCurrentRotation();
    Vector3 GetCurrentPosition();
    void Destroy();
}
