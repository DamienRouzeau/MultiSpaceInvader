using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public class BulletController : NetworkBehaviour
{

    public float _speed = 10f;
    public float timeOfLife = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (IsServer)
        {
            transform.position += transform.up * _speed * Time.deltaTime;
            timeOfLife -= Time.deltaTime;
            if (timeOfLife <= 0)
            {
                this.GetComponent<NetworkObject>().Despawn();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsServer)
        {
            if (collision.CompareTag("enemy"))
            {
                this.GetComponent<NetworkObject>().Despawn();
            }
        }
    }


}
