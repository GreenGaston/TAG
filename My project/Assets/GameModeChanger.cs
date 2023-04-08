using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameModeChanger : MonoBehaviour
{

    private GameObject gameStarter;
    private Dropdown dropdown;
    void Start(){
        dropdown = GetComponent<Dropdown>();
        dropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(dropdown);
        });

    }
    void DropdownValueChanged(Dropdown change){
        
        string option = dropdown.options[change.value].text;

    }
}
