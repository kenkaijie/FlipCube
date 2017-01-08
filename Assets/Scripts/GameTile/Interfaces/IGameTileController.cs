using UnityEngine;

// Game Tile Controller            
public interface IGameTileController
{
    Vector3 GetPosition();

    void DespawnTile();

    void SetTileState(ITileState newTileState);
    ITileState GetTileState();

    float GetSpawnTime();
    float GetDespawnTime();
}
