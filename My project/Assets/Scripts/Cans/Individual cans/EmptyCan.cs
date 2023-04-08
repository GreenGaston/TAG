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

    public void UseCanPermanently()
    {
        //throw new System.NotImplementedException();
    }

    public void UndoCan()
    {
        //throw new System.NotImplementedException();
    }

    public KindOfCan getKindOfCan()
    {
        return KindOfCan.None;
    }
}


