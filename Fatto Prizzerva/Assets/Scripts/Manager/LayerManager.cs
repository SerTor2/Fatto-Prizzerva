using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerManager : MonoBehaviour
{
    public PlayerScript player;
    public List<Floors> floors;
    private float posYPlayer;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        posYPlayer = player.gameObject.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (((posYPlayer - player.gameObject.transform.position.y) >= 0.2f) || ((posYPlayer - player.gameObject.transform.position.y) <= -0.2f))
        {
            posYPlayer = player.gameObject.transform.position.y;
            int maxLayer = 0;
            for (int i = 0; i < floors.Count; i++)
            {
                if (floors[i].gameObject.transform.position.y < player.transform.position.y)
                {
                    if (floors[i].layer >= maxLayer)
                        maxLayer = floors[i].layer;
                }
            }

            if(player.GetLayerPlayer() != maxLayer)
                player.MoveCameraUpLayer();


            player.SetLayerPlayer(maxLayer);

            foreach (Floors f in floors)
            {
                if (f.layer != player.GetLayerPlayer())
                    f.gameObject.SetActive(false);
                else
                    f.gameObject.SetActive(true);
            }


        }

    }
}