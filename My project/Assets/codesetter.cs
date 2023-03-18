using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class codesetter : MonoBehaviour
{
   
    // Update is called once per frame

    //textmeshpro text
    [SerializeField] TextMeshProUGUI text;
    void Update()
    {
        //set text of textfield to stringkeeper.code
        text.text=StringKeeper.CodeToConnect;

    }
}
