using UnityEngine;

public interface ITileState
{
    Color GetColor();
    string GetStateName();

    bool Equals(ITileState other);
}