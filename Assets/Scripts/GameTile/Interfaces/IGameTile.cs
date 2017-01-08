using UnityEngine;

// Interface for a basic game tile
public interface IGameTile
{
    Vector3 GetPosition();
    void SetTileColor(Color color, float durationS);
    void DespawnTile();
}
