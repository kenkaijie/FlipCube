using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public interface IGameTileController
{
    Vector3 GetPosition();
    bool IsActivated();
    void ActivateTile();
    void DeactivateTile();
    void DespawnTile();
    void SetTileColor(Color color);
}
