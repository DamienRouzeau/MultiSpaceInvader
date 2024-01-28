using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEditor;

public class EnemiesControlleur : NetworkBehaviour
{
    public float previousTargetPosition = float.PositiveInfinity;
    public NetworkObject playerTarget;
    public float distanceWithTarget;
    public float _speed = 5f;
    public int life = 3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (IsServer)
        {
            foreach (NetworkClient players in NetworkManager.Singleton.ConnectedClientsList)
            {
                distanceWithTarget = (players.PlayerObject.transform.position - transform.position).magnitude;
                if (distanceWithTarget < previousTargetPosition)
                {
                    playerTarget = players.PlayerObject;
                    previousTargetPosition = distanceWithTarget;
                }
            }
            transform.up = (transform.position - playerTarget.transform.position).normalized;
            transform.position += -transform.up * _speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsServer)
        {
            if (collision.CompareTag("bullet"))
            {
                life--;
            }
            if (life <= 0)
            {
                this.GetComponent<NetworkObject>().Despawn();
            }
        }
    }
}
