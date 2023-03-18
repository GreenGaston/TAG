using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GreenCan : NetworkBehaviour  , Can
{

    public KindOfCan kindOfCan = KindOfCan.Green;
    public float dosage= 0f;


    public float Duration = 3f;
    public float JumpBoost = 2f;
    

    public float currentDuration = 0f;
    public float currentTimeOut = 0f;
    public bool usingCan = false;
    private float originalJumpForce = 0f;


    Movement.NormalMovement normalMovement;
    // Start is called before the first frame update
    void Start()
    {
        //get normal movement script from player tag object child
        normalMovement = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Movement.NormalMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(usingCan){
            currentDuration += Time.deltaTime;
            //Debug.Log("currentDuration:"+currentDuration);
            if(currentDuration>=currentTimeOut){
                //stop using can
                usingCan = false;
                currentDuration = 0f;
                currentTimeOut = 0f;
                //deactivate can
                //gameObject.SetActive(false);
                normalMovement.JumpHeight = originalJumpForce;
            }
        }
    }

    public void useCan(){
        if(usingCan){
            currentTimeOut += Duration;

        }
        else{
            currentTimeOut = Duration;
            currentDuration = 0f;
            usingCan = true;
            originalJumpForce = normalMovement.JumpHeight;
            normalMovement.JumpHeight = originalJumpForce * JumpBoost;

        }
    }

    public void OverDose(){
        if(usingCan){
            
            //undo all changes
            normalMovement.JumpHeight = originalJumpForce;
            usingCan = false;
            currentDuration = 0f;
            currentTimeOut = 0f;
        }
    }

    public float getDosage(){
        return dosage;
    }

    public KindOfCan getKindOfCan(){
        return kindOfCan;
    }
  
}
