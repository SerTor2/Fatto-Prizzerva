using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemieBasic : MonoBehaviour
{
    public GameObject player;
    public float speed = 6;
    private float speedEmpuje = 0;
    private float currentTime = 0;
    private float maxTime = 0;
    private Vector3 direction;
    private CharacterController characterController;
    // Start is called before the first frame update
    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTime > 0)
        {
            characterController.Move(direction * speedEmpuje * Time.deltaTime);
            currentTime += Time.deltaTime;
            if(currentTime >= maxTime)
            {
                currentTime = 0;
            }
        }
        else
        {
            if(player.gameObject.activeSelf)
                characterController.Move((player.gameObject.transform.position - gameObject.transform.position).normalized * speed * Time.deltaTime);
        }
    }

    public bool MoveDirectionHit(Vector3 _direction, float _damage)
    {
        if (_damage <= 1)
            return false;
        else
        {
            direction = _direction;
            maxTime = 0.25f;
            currentTime += Time.deltaTime;
            speedEmpuje = _damage;
            return true;
        }

    }
}
