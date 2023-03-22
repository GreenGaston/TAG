using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] 
    private Button backButton;

    void Awake(){
        //destroy this gameobject when the back button is clicked
        backButton.onClick.AddListener(()=>{
            Destroy(gameObject);
        });
    }
}
