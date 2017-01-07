using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public interface IPlayerController: IStateful<LogicControllerState>
{
    void StartMovePlayer(TumblingDirection direction, EventCallback callback);
    void TeleportPlayer(Vector3 position);
    void DestroyPlayer();
    Vector3 GetPlayerCurrentPosition();
    Vector3 GetPlayerTargetPosition();
}