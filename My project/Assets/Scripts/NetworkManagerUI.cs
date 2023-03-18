using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class NetworkManagerUI: MonoBehaviour
{
    // Start is called before the first frame update\
    [SerializeField] private Button _hostButton;
    [SerializeField] private Button _clientButton;
    [SerializeField] private Button _serverButton;
    void Awake(){
        _hostButton.onClick.AddListener(() => NetworkManager.Singleton.StartHost());
        _clientButton.onClick.AddListener(() => NetworkManager.Singleton.StartClient());
        _serverButton.onClick.AddListener(() => NetworkManager.Singleton.StartServer());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
