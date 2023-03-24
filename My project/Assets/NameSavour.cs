using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NameSavour : MonoBehaviour
{
    public TMP_InputField inputField;
    void Update(){
        StringKeeper.name=inputField.text;
    }
}
