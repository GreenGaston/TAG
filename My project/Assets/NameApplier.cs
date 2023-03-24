using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
//for fixed string
using Unity.Collections;
public class NameApplier : NetworkBehaviour
{
    // Start is called before the first frame update

    public NetworkVariable<FixedString64Bytes> name=new NetworkVariable<FixedString64Bytes>();

    public override void OnNetworkSpawn()
    {
        name.OnValueChanged+= OnNameChanged;
    }

    public override void OnNetworkDespawn()
    {
        name.OnValueChanged-= OnNameChanged;
    }

    public void OnNameChanged(FixedString64Bytes oldValue,FixedString64Bytes newValue){
        Debug.Log("NameApplier: OnNameChanged");
        //set the name
        text.text=newValue.ToString();
    }
    //textmeshpro text object
    public TMP_Text text;
    void Start()
    {

        if(IsOwner){
            UpdateNameServerRpc(StringKeeper.name);
        }
        text.text=name.Value.ToString();
    }

    
    [ServerRpc]
    public void UpdateNameServerRpc(string name)
    {
        //set the name
        this.name.Value=name;
        
    }
}
