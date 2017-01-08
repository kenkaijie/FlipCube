using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTile : MonoBehaviour, IGameTile
{
    private Animator anim;
    private Renderer tileRenderer;
    // Use this for initialization

    private Color currentColor;
    private Color targetColor;

    float lerpPercentage = 0.0f;
    float lerpStep = 0.1f;

    CallbackHandler onGeneratedCallback;

    void Start()
    {
        tileRenderer = transform.Find("AnimatedGroup/Tile").gameObject.GetComponent<Renderer>();
        anim = GetComponentInChildren<Animator>();
        currentColor = targetColor = tileRenderer.material.GetColor("_Color");
    }

    public void SetTileColor(Color target, float durationS)
    {
        // prevent too low values
        durationS = Mathf.Max(0.01f, durationS);
        targetColor = target;
        lerpPercentage = 0.0f;
        lerpStep = Time.fixedDeltaTime / durationS;

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
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void DespawnTile()
    {
        anim.SetTrigger("Despawn");
        float delay = anim.GetCurrentAnimatorStateInfo(0).length + 1f;
        Destroy(gameObject, delay);
    }

    public void SetOnCreationCompleteHandler(CallbackHandler callback)
    {
        onGeneratedCallback = callback;
    }
}
