using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;

public class TagManager : NetworkBehaviour
{
    //store reference to player who is it as a network object
    public NetworkVariable<ChaseState> TagState=new NetworkVariable<ChaseState>(ChaseState.Runner);

    [SerializeField]
    private GameObject chaserModel;

    [SerializeField]
    private GameObject runnerModel;

    [SerializeField]
    private ChaseHandler _ChaseHandler;

    

    public override void OnNetworkSpawn()
    {
        TagState.OnValueChanged+= OnIsChaserChanged;
    }

    public override void OnNetworkDespawn()
    {
        TagState.OnValueChanged-= OnIsChaserChanged;
    }

    public void OnIsChaserChanged(ChaseState oldValue,ChaseState newValue){
        //Debug.Log("TagManager: OnIsChaserChanged    oldValue: "+oldValue+"    newValue: "+newValue+"");
        if(oldValue!=ChaseState.Chaser&&newValue==ChaseState.Chaser){
            //Debug.Log("hurray");
            chaserModel.SetActive(true);
            runnerModel.SetActive(false);
        }
        else if(oldValue!=ChaseState.Runner&&newValue==ChaseState.Runner){
            //Debug.Log("hurray2");
            chaserModel.SetActive(false);
            runnerModel.SetActive(true);
        }
        if(!IsOwner){
            return;
        }

        if(oldValue!=ChaseState.Chaser&&newValue==ChaseState.Chaser){
            _ChaseHandler.BecomeChaser();
        }
        else if(oldValue!=ChaseState.Runner&&newValue==ChaseState.Runner){
            _ChaseHandler.BecomeRunner();
        }
        else{
            //throw error this should never happen
            Debug.LogError("TagManager: OnIsChaserChanged: newValue and oldValue are the same");
        }
        
    }
    


    [ServerRpc(RequireOwnership = false)]
    public void ChaserServerRpc(ChaseState chaseState){
        TagState.Value=chaseState;
    }

}
