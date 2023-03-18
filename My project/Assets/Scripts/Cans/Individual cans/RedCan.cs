using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class RedCan : NetworkBehaviour , Can
{

    public KindOfCan kindOfCan = KindOfCan.Red;
    public void useCan()
    {
        Debug.Log("Red Can");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OverDose()
    {
        //undo all changes
    }

    public float getDosage()
    {
        return 0f;
    }
    
    public KindOfCan getKindOfCan()
    {
        return KindOfCan.Red;
    }
}
