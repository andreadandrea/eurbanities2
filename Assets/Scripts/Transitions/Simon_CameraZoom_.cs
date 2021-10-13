using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simon_CameraZoom_ : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float cameraSize = 6;
    
    [Header("Do not Change")]
    public bool zoomActive;
    
    public Vector3 target;
    private Simon_Door_ door;
    private CinemachineVirtualCamera cineCam;
    
    
    void Start()
    {
        cineCam = GetComponent<CinemachineVirtualCamera>();
        door = FindObjectOfType<Simon_Door_>();
        zoomActive = false;
        
    }

    void LateUpdate()
    {
        if(zoomActive)
        {
            
            
            cineCam.m_Lens.OrthographicSize = Mathf.Lerp(cineCam.m_Lens.OrthographicSize, 2, speed);
            //transform.position = Vector3.Lerp(transform.position, target, speed);
            //transform.position = Vector3.Lerp(transform.position, door.transform.position, speed);
            
        }
        else
        {
            cineCam.m_Lens.OrthographicSize = Mathf.Lerp(cineCam.m_Lens.OrthographicSize, cameraSize, speed);
        }
    }
}
