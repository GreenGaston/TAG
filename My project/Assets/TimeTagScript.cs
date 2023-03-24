using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeTagScript : MonoBehaviour
{

    public bool started=false;
    public float time=0;
    public float timeToWait=0;
    // Start is called before the first frame update

    public void StartTimer(float timeToWait){
        started=true;
        time=0;
        this.timeToWait=timeToWait;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(started){
            time+=Time.deltaTime;
            if(time>=timeToWait){
                started=false;
                time=0;
                timeToWait=0;
                //destroy parent
                Destroy(gameObject.transform.parent.gameObject);
                
            }
            TextMeshProUGUI textmeshPro = GetComponent<TextMeshProUGUI>();
            string timeString=formatTime(time);
            textmeshPro.SetText(timeString);


        }
    }

    private string formatTime(float time){
        //seconds should be tranformed into the following format 00:00
        string timeString="";
        int minutes=(int)time/60;
        int seconds=(int)time%60;
        if(minutes<10){
            timeString+="0";
        }
        timeString+=minutes.ToString()+":";
        if(seconds<10){
            timeString+="0";
        }
        timeString+=seconds.ToString();
        return timeString;
    }
}
