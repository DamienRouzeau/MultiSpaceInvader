using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkSetup : MonoBehaviour
{
    public GameObject boutons;
    public NetworkManager _NetworkManager;
    // Start is called before the first frame update
    void Start()
    {
        _NetworkManager= GetComponent<NetworkManager>();
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void CallStartHost()
    {
        _NetworkManager.StartHost();
        boutons.SetActive(false);
    }

    public void CallStartClient()
    {
        _NetworkManager.StartClient();
        boutons.SetActive(false);

    }
}
