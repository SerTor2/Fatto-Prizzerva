using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantaTierra : MonoBehaviour
{
    private PlayerScript player;
    private bool inSide = false;
    public float forceJump = 50f;
    public Vector3 direction = Vector3.forward;
    public bool planningPlant = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && !inSide)
        {
            player = other.gameObject.GetComponent<PlayerScript>();
            player.SetPlantaTierra(this);
            inSide = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && inSide)
        {
            player.SetPlantaTierra(null);
            player = null;
            inSide = false;
        }
    }
}
