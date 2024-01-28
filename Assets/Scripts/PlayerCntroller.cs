using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerCntroller : NetworkBehaviour
{
    public PlayerInputAction _PlayerInput;
    public Vector2 actualInput;
    public NetworkVariable<Vector3> _Position;
    public float _speed = 2f;
    public GameObject bullet;
    public GameObject enemy;
    public Vector3 enemiesSpawner = new Vector3(0,10,0);



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
        if (IsLocalPlayer)
        {
            SetPositionServerRpc(actualInput);
            float spawnRandom = Random.Range(0,30);
            if (spawnRandom == 0)
            {
                SetEnemyServerRpc();
            }
            Vector2 targetDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            SetRotationServerRpc(targetDir);
        }

        if (IsServer)
        {
            transform.position = _Position.Value;
        }

    }

    [ServerRpc]
    public void SetRotationServerRpc(Vector3 targetDir)
    {
        transform.up = targetDir;
    }

    [ServerRpc]
    public void SetEnemyServerRpc()
    {
       float numberRandom = Random.Range(0, 3);

        for (float i = numberRandom; i > 0; i--)
        {
            var spawnEnemies = Instantiate(enemy, enemiesSpawner, Quaternion.identity);
            spawnEnemies.GetComponent<NetworkObject>().Spawn();
        }
    }


    [ServerRpc]
    public void SetShotServerRpc()
    {
        var spawnBullet = Instantiate(bullet, transform.position, Quaternion.identity);
        spawnBullet.GetComponent<NetworkObject>().Spawn();
        spawnBullet.transform.up = transform.up;
    }


    [ServerRpc]
    public void SetPositionServerRpc(Vector2 actualInput)
    {
        var position = transform.position;
        position.x += actualInput.x * _speed * NetworkManager.ServerTime.FixedDeltaTime;
        position.y += actualInput.y * _speed * NetworkManager.ServerTime.FixedDeltaTime;
        _Position.Value = position;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsLocalPlayer)
        {
            _PlayerInput = new PlayerInputAction();
            _PlayerInput.Enable();
            _PlayerInput.Player.move.started += Oninputmove;
            _PlayerInput.Player.move.performed += Oninputmove;
            _PlayerInput.Player.move.canceled += Oninputmove;
            _PlayerInput.Player.shot.started += Oninputshot;
            _PlayerInput.Player.move.performed += Oninputmove;
        }
    }

    public void Oninputmove(InputAction.CallbackContext context)
    {
        actualInput = context.ReadValue<Vector2>();
    }

    public void Oninputshot(InputAction.CallbackContext context)
    {
        SetShotServerRpc();
    }

}
