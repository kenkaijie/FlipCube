using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTile : MonoBehaviour, IGameTile
{
    private Animator anim;

    // RED E1212180
    // GREEN 86F48C80
    // BLUE 21B9E180

    // Use this for initialization
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void ActivateTile()
    {
        anim.SetBool("Activated",true);
    }

    public void DeactivateTile()
    {
        anim.SetBool("Activated", false);
    }
    
    public Vector3 GetPosition()
    {
        return transform.position;
    }

}
