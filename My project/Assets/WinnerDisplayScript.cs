using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinnerDisplayScript : MonoBehaviour
{
    // Start is called before the first frame update
    public string winner="";
    public float timeToDisplay=5;
    public float time=0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //text
        TextMeshProUGUI textmeshPro = GetComponent<TextMeshProUGUI>();
        //set text
        textmeshPro.SetText(winner);
        //if time is less than time to display
        if(time<timeToDisplay){
            //add time
            time+=Time.deltaTime;
        }else{
            //destroy parent
            Destroy(gameObject.transform.parent.gameObject);
        }
    }
}
