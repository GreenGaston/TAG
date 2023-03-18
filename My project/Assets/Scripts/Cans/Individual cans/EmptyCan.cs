using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyCan : MonoBehaviour,Can
{
    public void useCan()
    {
        //Debug.Log("Empty Can");
    }

    public void OverDose()
    {
        //Debug.Log("Empty Can Overdose");
    }

    public float getDosage()
    {
        return 0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public KindOfCan getKindOfCan()
    {
        return KindOfCan.None;
    }
}


