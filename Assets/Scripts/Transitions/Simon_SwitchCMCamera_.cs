using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

[RequireComponent(typeof(CinemachineBrain))]
public class Simon_SwitchCMCamera_ : MonoBehaviour
{
    public static event Action OnCameraBlendStarted;

    public static event Action OnCameraBlendFinished;

    private CinemachineBrain cineMachineBrain;

    private bool wasBlendingLastFrame;
    
    private void Awake()
    {
        cineMachineBrain = GetComponent<CinemachineBrain>();
    }
    private void Start()
    {
        wasBlendingLastFrame = false;
    }
    private void Update()
    {
        if(cineMachineBrain.IsBlending)
        {
            if(!wasBlendingLastFrame)
            {
                OnCameraBlendStarted?.Invoke();
            }
            wasBlendingLastFrame = true;
        }
        else
        {
            if(wasBlendingLastFrame)
            {
                OnCameraBlendFinished?.Invoke();
                wasBlendingLastFrame = false;
            }
        }
    }
}
