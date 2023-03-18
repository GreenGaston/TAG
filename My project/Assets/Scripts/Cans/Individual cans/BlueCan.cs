using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BlueCan : NetworkBehaviour, Can
{
    public float Duration = 3f;
    public float SpeedBoost = 2f;
    public float AccelerationMultiplier = 2f;

    public float currentDuration = 0f;
    public float currentTimeOut = 0f;
    public bool usingCan = false;
    private float originalSpeed = 0f;
    private float originalAcceleration = 0f;

    public KindOfCan kindOfCan = KindOfCan.Blue;
    public float dosage= 0f;



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
        if(!IsOwner)
            return;
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
                normalMovement.SprintSpeed = originalSpeed;
                normalMovement.Acceleration = originalAcceleration;
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
            originalSpeed = normalMovement.SprintSpeed;
            originalAcceleration = normalMovement.Acceleration;
            normalMovement.SprintSpeed = originalSpeed * SpeedBoost;
            normalMovement.Acceleration = originalAcceleration * AccelerationMultiplier;
        }
    }

    public void OverDose(){
        if(usingCan){
            //undo all changes
            normalMovement.SprintSpeed = originalSpeed;
            normalMovement.Acceleration = originalAcceleration;
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
