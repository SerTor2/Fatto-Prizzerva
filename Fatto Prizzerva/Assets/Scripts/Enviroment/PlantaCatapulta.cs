using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantaCatapulta : MonoBehaviour
{
    private PlayerScript player;
    private bool inSide = false;
    private Vector3 direction;
    private float currentTime = 0;
    [SerializeField] private KeyCode upKey = KeyCode.W;
    [SerializeField] private KeyCode downKey = KeyCode.S;
    [SerializeField] private KeyCode rightKey = KeyCode.D;
    [SerializeField] private KeyCode leftKey = KeyCode.A;
    // Start is called before the first frame update
    void Start()
    {
        direction = gameObject.transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        if(inSide)
        {
            if (currentTime < 0.5f)
                currentTime += Time.deltaTime;
            CheckKeys();
        }
    }

    private void CheckKeys()
    {
        Vector3 lastDirection = direction;
        direction = Vector3.zero;
        if (Input.GetKey(upKey))
            direction += Vector3.up;
        if (Input.GetKey(downKey))
            direction += Vector3.down;
        if (Input.GetKey(rightKey))
            direction += Vector3.right;
        if (Input.GetKey(leftKey))
            direction += Vector3.left;

        direction.Normalize();
        if (direction.magnitude == 0)
            direction = lastDirection;

        gameObject.transform.rotation = Quaternion.LookRotation(gameObject.transform.forward, direction);

        if ((Input.GetMouseButton(0) || Input.GetMouseButton(1)) && currentTime >= 0.5f)
            ExitPlant();

    }

    public void EnterInThePlant(PlayerScript _player)
    {
        player = _player;
        currentTime = 0;
        player.gameObject.GetComponent<CharacterController>().enabled = false;
        player.gameObject.SetActive(false);
        inSide = true;
    }

    public void ExitPlant(bool _killPlant = false)
    {
        if(_killPlant)
        {
            if (player != null)
            {
                player.gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, player.gameObject.transform.position.z);
                player.gameObject.GetComponent<CharacterController>().enabled = true;
                player.gameObject.SetActive(true);

            }
            inSide = false;
        }
        else
        {
            player.gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, player.gameObject.transform.position.z) + direction;
            player.gameObject.GetComponent<CharacterController>().enabled = true;
            player.StartFlyKick(direction);
            player.gameObject.SetActive(true);
            inSide = false;
        }
    }
}
