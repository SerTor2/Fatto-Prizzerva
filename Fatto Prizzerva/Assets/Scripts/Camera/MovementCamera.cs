using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementCamera : MonoBehaviour
{
    public Transform posIzqueirdaArriba;
    public Transform posIzquierdaAbajo;
    public Transform posDerechaArriba;
    public Transform posDerechaAbajo;
    private PlayerScript player;
    private Vector3 distanceToPlayer;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        distanceToPlayer = player.gameObject.transform.position - gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector3(player.gameObject.transform.position.x - distanceToPlayer.x, gameObject.transform.position.y, player.gameObject.transform.position.z - distanceToPlayer.z);

        if (gameObject.transform.position.x < posIzqueirdaArriba.position.x)
            gameObject.transform.position = new Vector3(posIzqueirdaArriba.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        else if (gameObject.transform.position.x > posDerechaArriba.position.x)
            gameObject.transform.position = new Vector3(posDerechaArriba.position.x, gameObject.transform.position.y, gameObject.transform.position.z);

        if (gameObject.transform.position.z > posIzqueirdaArriba.position.z)
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, posIzqueirdaArriba.position.z);
        else if (gameObject.transform.position.z < posDerechaAbajo.position.z)
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, posDerechaAbajo.position.z);

    }
}
