using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FootstepsSound_ : MonoBehaviour
{
    [EventRef] [SerializeField] string playerFootstepsSound = "event:/Global/Footsteps/Footstep_Asphalt";

    FMOD.Studio.EventInstance playerFootsteps;

    public void PlayerFootstepsSound()
    {
        playerFootsteps = RuntimeManager.CreateInstance(playerFootstepsSound);

        playerFootsteps.start();
    }
}

