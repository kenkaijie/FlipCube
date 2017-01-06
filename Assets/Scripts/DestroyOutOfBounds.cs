using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour {

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // stub for now
        }
    }
}
