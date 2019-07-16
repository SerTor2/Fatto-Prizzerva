using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantaCatapulta : MonoBehaviour
{
    private PlayerScript player;
    private PunchFly punchFly;
    private bool inSide = false;
    private bool pulsed = false;
    private Vector3 direction;
    private float rotationSpeed = 60;
    public float minSpeed = 60;
    public float maxSpeed = 720;
    public float dividendo = 5;
    public float maxTime = 5;
    [SerializeField] private KeyCode upKey = KeyCode.W;
    [SerializeField] private KeyCode downKey = KeyCode.S;
    [SerializeField] private KeyCode rightKey = KeyCode.D;
    [SerializeField] private KeyCode leftKey = KeyCode.A;
    private GameObject myCamera;
    // Start is called before the first frame update
    void Start()
    {
        rotationSpeed = minSpeed;
        direction = gameObject.transform.forward;
        myCamera = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (inSide)
        {
            CheckKeys();

        }
    }

    private void CheckKeys()
    {
        player.currentTimeState += Time.deltaTime;
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        if (rotationSpeed < maxSpeed)
            rotationSpeed += Time.deltaTime * rotationSpeed / dividendo;
        else if (rotationSpeed > maxSpeed)
            rotationSpeed = maxSpeed;

        direction = gameObject.transform.rotation.eulerAngles;
        float diference = (direction.y / 90f) - Mathf.RoundToInt(direction.y / 90f);
        if (diference <= 0.01f && (pulsed || player.currentTimeState >= maxTime))
        {
            int forward = Mathf.RoundToInt(direction.y / 90f);
            switch (forward)
            {
                case 1: direction = new Vector3(1,0,0);
                    break;
                case 2:
                    direction = new Vector3(0, 0, -1);
                    break;
                case 3:
                    direction = new Vector3(-1, 0, 0);
                    break;
                case 4:
                case 0:
                    direction = new Vector3(0, 0, 1);
                    break;
            }
            ExitPlant();
        }

        if ((Input.GetMouseButton(0) || Input.GetMouseButton(1)) && rotationSpeed >= maxSpeed - maxSpeed / 10)
            pulsed = true;

    }

    public void EnterInThePlant(PlayerScript _player)
    {
        player = _player;
        punchFly = _player.punchFly.GetComponent<PunchFly>();
        player.gameObject.GetComponent<CharacterController>().enabled = false;
        player.gameObject.SetActive(false);
        rotationSpeed = minSpeed;
        player.currentTimeState = 0;
        inSide = true;
        pulsed = false;
    }

    public void ExitPlant(bool _killPlant = false)
    {
        if (_killPlant)
        {
            if (player == null) return;

            player.gameObject.transform.position = new Vector3(gameObject.transform.position.x, player.gameObject.transform.position.y, gameObject.transform.position.z);
            player.gameObject.GetComponent<CharacterController>().enabled = true;
            player.gameObject.SetActive(true);
        }
        else
        {
            player.gameObject.transform.position = new Vector3(gameObject.transform.position.x, player.gameObject.transform.position.y, gameObject.transform.position.z) + direction;
            player.gameObject.GetComponent<CharacterController>().enabled = true;
            punchFly.StartFlyKick(direction, player.normalSpeed * 3f);
            player.gameObject.SetActive(true);

        }
        inSide = false;


    }
}
