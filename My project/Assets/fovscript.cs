using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;
using UnityEngine.Serialization;
public class fovscript : MonoBehaviour
{
    //get cinemachine virtual camera
    private Cinemachine.CinemachineVirtualCamera vcam;
    void Start()
    {
        vcam=GetComponent<Cinemachine.CinemachineVirtualCamera>();
    
    }
       

    // Update is called once per frame
    void Update()
    {
        vcam.m_Lens.FieldOfView=SettingsFile.getVariableFloat("FOV");
    }
}
