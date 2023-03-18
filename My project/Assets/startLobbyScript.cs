using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class startLobbyScript : MonoBehaviour
{
    // whenever a player walks into the trigger
    //public GameObject PlayerPrefab;

    public float time = 0;
    public bool entered = false;
    public float waitTime = 5;
    private void OnTriggerEnter(Collider other)
    {
        // if it is the player
        if (other.gameObject.tag == "Player")
        {
            entered=true;

        }
    }
    private void OnTriggerExit(Collider other)
    {
        // if it is the player
        if (other.gameObject.tag == "Player")
        {
            entered=false;
            time=0;
        }
    }

    void Update()
    {
        if(entered){
            time+=Time.deltaTime;
            if(time>=waitTime){
                Loader.Load(Loader.Scene.Game);
            }
        }
    }
}
