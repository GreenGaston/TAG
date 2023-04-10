using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class RedCan : NetworkBehaviour , Can
{

    public KindOfCan kindOfCan = KindOfCan.Red;

    //object containing all the walls that need to be disabled
    private GameObject walls;
    public void useCan()
    {
        Debug.Log("Red Can");
    }

    // Start is called before the first frame update
    void Start()
    {
        //find the walls by the tag WallDisabler
        walls=GameObject.FindGameObjectWithTag("WallDisabler");
    }


    public void OverDose()
    {
        //undo all changes
    }

    public float getDosage()
    {
        return 0f;
    }
    

    public void UseCanPermanently()
    {
        
        //disable all the walls
        walls.SetActive(false);
    }

    public void UndoCan()
    {
      
        //enable all the walls
        walls.SetActive(true);
    }
    public KindOfCan getKindOfCan()
    {
        return KindOfCan.Red;
    }
}
