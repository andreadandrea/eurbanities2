using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class PhoneAudio_ : MonoBehaviour
{
    [SerializeField] [EventRef] private string select, back, click, open, close, locked;

    public void phoneClickSound()
    {
        RuntimeManager.PlayOneShot(click, transform.position);
    }

    public void phoneBackSound()
    {
        RuntimeManager.PlayOneShot(back, transform.position);
    }

    public void phoneSelectSound()
    {
        RuntimeManager.PlayOneShot(select, transform.position);
    }

    public void phoneOpenSound(bool phoneOpenOrClosed)
    {
        if (phoneOpenOrClosed == true)
        {
            RuntimeManager.PlayOneShot(open, transform.position);
        }
        else
        {
            RuntimeManager.PlayOneShot(close, transform.position);
        }
    }
    public void phoneLockedSound()
    {
        RuntimeManager.PlayOneShot(locked, transform.position);
    }
}
