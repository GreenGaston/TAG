using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class TempCharacterController : NetworkBehaviour
{
    // Start is called before the first frame update
    StarterAssetsInputs _input;
    public float speed=6f;
    void Start()
    {
        if(!IsOwner)
            return;
        Debug.Log("I am the owner");
        _input = GetComponent<StarterAssetsInputs>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsOwner)
            return;
        Debug.Log("I am the owner");
        Vector2 move=_input.move.normalized*speed*Time.deltaTime;
        Debug.Log(move);
        transform.Translate(move.x,0,move.y);


    }
}
