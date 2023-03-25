using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;

public class QuitButton : MonoBehaviour
{
    public Button quitButton;
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
    }
    
}
