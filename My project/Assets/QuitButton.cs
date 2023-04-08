using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;

public class QuitButton : MonoBehaviour
{
    public Button quitButton;

    public Button unpauseButton;
    public GameObject mainMenu;
    

    void Start()
    {
        quitButton.onClick.AddListener(() => {
            //disconnect from server
            
            NetworkManager.Singleton.Shutdown();
            //switch to main menu
            Instantiate(mainMenu);
            //destroy  parent object
            Destroy(transform.parent.gameObject);

        });
        unpauseButton.onClick.AddListener(() => {
            //find the player that you own
            GameObject[] players=GameObject.FindGameObjectsWithTag("Player");

            foreach(GameObject player in players){
                if(player.GetComponent<NetworkObject>().IsOwner){
                    //set the pause variable to false
                    player.GetComponentInChildren<StarterAssetsInputs>().PauseInput(true);
                    return;
                }
            }

        });
    }
    
}
