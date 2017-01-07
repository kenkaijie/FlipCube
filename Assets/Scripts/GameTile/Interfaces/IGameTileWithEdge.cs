using UnityEngine;

public interface IGameTileWithEdge: IGameTile
{
    void SetTileEdgeColor(Color color, float durationS);
    void SetTileCenterColor(Color color, float durationS);
    void SetFullTileColor(Color color, Color colorEdge, float durationS);
}
