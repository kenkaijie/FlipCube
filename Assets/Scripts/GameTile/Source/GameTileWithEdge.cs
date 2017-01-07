using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTileWithEdge : MonoBehaviour, IGameTile, IGameTileWithEdge
{
    private Animator anim;
    private Renderer tileRenderer;
    private Renderer tileEdgeRenderer;
    // Use this for initialization

    public Color defaultColor;
    public Color defaultEdgeColor;
    
    //tile
    private Color currentColor;
    private Color targetColor;
    float lerpPercentage = 0.0f;
    float lerpStep = 0.1f;

    //tile
    private Color currentEdgeColor;
    private Color targetEdgeColor;
    float lerpEdgePercentage = 0.0f;
    float lerpEdgeStep = 0.1f;


    void Start()
    {
        tileRenderer = transform.Find("AnimatedGroup/Tile").gameObject.GetComponent<Renderer>();
        tileEdgeRenderer = transform.Find("AnimatedGroup/TileEdge").gameObject.GetComponent<Renderer>();
        anim = GetComponentInChildren<Animator>();
        currentColor = targetColor = tileRenderer.material.GetColor("_Color");
        currentEdgeColor = targetEdgeColor = tileEdgeRenderer.material.GetColor("_Color");
        SetTileColor(defaultColor, 1.0f);
        SetTileEdgeColor(defaultEdgeColor, 1.0f);
    }

    public void SetTileColor(Color color, float durationS)
    {
        SetFullTileColor(color, color, durationS);
    }

    private void FixedUpdate()
    {
        if (!currentColor.Equals(targetColor))
        {
            lerpPercentage = Mathf.Min(lerpStep + lerpPercentage, 1.0f);
            if (Mathf.Approximately(lerpPercentage, 1.0f))
            {
                currentColor = targetColor;
                return;
            }
            tileRenderer.material.SetColor("_Color", Color.Lerp(currentColor, targetColor, lerpPercentage));
        }
        if (!currentEdgeColor.Equals(targetEdgeColor))
        {
            lerpEdgePercentage = Mathf.Min(lerpEdgeStep + lerpEdgePercentage, 1.0f);
            if (Mathf.Approximately(lerpEdgePercentage, 1.0f))
            {
                currentEdgeColor = targetEdgeColor;
                return;
            }
            tileEdgeRenderer.material.SetColor("_Color", Color.Lerp(currentEdgeColor, targetEdgeColor, lerpEdgePercentage));
        }
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void DespawnTile()
    {
        anim.SetTrigger("Despawn");
        Destroy(gameObject, 1.0f);
    }

    public void SetTileEdgeColor(Color color, float durationS)
    {
        durationS = Mathf.Max(0.01f, durationS);
        targetEdgeColor = color;
        lerpEdgePercentage = 0.0f;
        lerpEdgeStep = Time.fixedDeltaTime / durationS;
    }

    public void SetTileCenterColor(Color color, float durationS)
    {
        // prevent too low values
        durationS = Mathf.Max(0.01f, durationS);
        targetColor = color;
        lerpPercentage = 0.0f;
        lerpStep = Time.fixedDeltaTime / durationS;
    }

    public void SetFullTileColor(Color color, Color colorEdge, float durationS)
    {
        SetTileCenterColor(color, durationS);
        SetTileEdgeColor(colorEdge, durationS);
    }
}
