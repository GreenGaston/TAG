using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
//import unitytransport
using TMPro;
 



public class LobbyUI : MonoBehaviour
{
    [SerializeField]
    private Button createRoomButton;
    [SerializeField]
    private Button joinRoomButton;

    //input field
    [SerializeField]
    private TMP_InputField text;
    async void Awake(){
        
        createRoomButton.onClick.AddListener(async ()=>{
            Debug.Log("create room button clicked");
            try{
                Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);
                Debug.Log("allocation id: "+allocation.AllocationId);
                string joinCode = await Unity.Services.Relay.RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
                StringKeeper.CodeToConnect=joinCode;
                //change scene to lobby
                Debug.Log("code to connect: "+StringKeeper.CodeToConnect);
                //Loader.Load(Loader.Scene.Lobby);
                NetworkManager.Singleton.GetComponent<Unity.Netcode.Transports.UTP.UnityTransport>().SetHostRelayData(
                    allocation.RelayServer.IpV4,
                    (ushort)allocation.RelayServer.Port,
                    allocation.AllocationIdBytes,
                    allocation.Key,
                    allocation.ConnectionData
                );
                //destroy parent gameobject
                Destroy(transform.parent.gameObject);
                NetworkManager.Singleton.StartHost();
            } catch (System.Exception e){
                Debug.Log(e);
            }
        });
        joinRoomButton.onClick.AddListener(async ()=>{
            Debug.Log("join room button clicked");
            //get text from textfield gameobject
            
            string code = text.text.ToString();
            
            try{
                JoinAllocation joinallocation= await RelayService.Instance.JoinAllocationAsync(code);


                NetworkManager.Singleton.GetComponent<Unity.Netcode.Transports.UTP.UnityTransport>().SetClientRelayData(
                    joinallocation.RelayServer.IpV4,
                    (ushort)joinallocation.RelayServer.Port,
                    joinallocation.AllocationIdBytes,
                    joinallocation.Key,
                    joinallocation.ConnectionData,
                    joinallocation.HostConnectionData
                );
                //destroy parent gameobject
                Destroy(transform.parent.gameObject);
                NetworkManager.Singleton.StartClient();
            } catch (System.Exception e){
                Debug.Log(e);
            }
            
        });
    }

    async void Start(){
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }
    



}
