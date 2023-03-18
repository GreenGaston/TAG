using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour
{
    public float boostForce;
    public Vector3 boostDirection;
    public bool OverrideSpeed;
    public bool reorient;

    void Awake(){
        //convert the boost direction to global space
        if(reorient){
            boostDirection=transform.TransformDirection(boostDirection);
        }
       
        
    }

}
