using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public interface IGameTile
{
    Vector3 GetPosition();
    void SetTileColor(Color color, float durationS);
    void DespawnTile();
}
