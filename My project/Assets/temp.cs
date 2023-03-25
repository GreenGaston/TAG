using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class temp : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(IsOwner){
            //disable mesh renderer
            GetComponent<MeshRenderer>().enabled=false;
        }
    }

  
}
