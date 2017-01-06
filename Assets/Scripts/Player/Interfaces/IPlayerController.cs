using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public interface IPlayerController: IStateful<LogicControllerState>
{
    void StartMovePlayer(TumblingDirection direction, EventCallback callback);
    Vector3 GetPlayerCurrentPosition();
    Vector3 GetPlayerTargetPosition();
}