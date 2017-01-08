using UnityEngine;

public class TileState: ITileState
{
    private string stateName;
    private Color stateColor;

    public TileState(string name)
    {
        stateName = name;
        stateColor = Color.clear;
    }

    public TileState(string name, Color color)
    {
        stateName = name;
        stateColor = color;
    }

    public Color GetColor()
    {
        return stateColor;
    }

    public string GetStateName()
    {
        return stateName;
    }

    public bool Equals(ITileState other)
    {
        if (other.GetColor().Equals(stateColor) && other.GetStateName().Equals(stateName))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
