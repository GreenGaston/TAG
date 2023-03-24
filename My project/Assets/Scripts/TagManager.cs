using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TagManager : NetworkBehaviour
{
    //store reference to player who is it as a network object
    public NetworkVariable<bool> isChaser=new NetworkVariable<bool>();

    [SerializeField]
    private GameObject chaserModel;

    [SerializeField]
    private GameObject runnerModel;

    [SerializeField]
    private ChaseHandler _ChaseHandler;

    

    public override void OnNetworkSpawn()
    {
        isChaser.OnValueChanged+= OnIsChaserChanged;
    }

    public override void OnNetworkDespawn()
    {
        isChaser.OnValueChanged-= OnIsChaserChanged;
    }

    public void OnIsChaserChanged(bool oldValue,bool newValue){
        Debug.Log("TagManager: OnIsChaserChanged");
        if(newValue&&!oldValue){
            chaserModel.SetActive(true);
            runnerModel.SetActive(false);
        }
        else if(!newValue&&oldValue){
            chaserModel.SetActive(false);
            runnerModel.SetActive(true);
        }
        if(!IsOwner){
            return;
        }

        if(newValue&&!oldValue){
            _ChaseHandler.BecomeChaser();
        }
        else if(!newValue&&oldValue){
            _ChaseHandler.BecomeRunner();
        }
        else{
            //throw error this should never happen
            Debug.LogError("TagManager: OnIsChaserChanged: newValue and oldValue are the same");
        }
        
    }
    


    [ServerRpc(RequireOwnership = false)]
    public void ChaserServerRpc(){
        isChaser.Value=!isChaser.Value;
    }

}
