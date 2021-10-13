using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class DebateGatheringSounds_ : MonoBehaviour
{
    [EventRef] [SerializeField] private string barFillingSound = "event:/UI/Bar_Filling_Up";
    [EventRef] [SerializeField] private string rock = "event:/Debate/Rock";
    [EventRef] [SerializeField] private string scissor = "event:/Debate/Scissor";
    [EventRef] [SerializeField] private string paper = "event:/Debate/Paper";
    
    public void BarFillingUp()
    {
        RuntimeManager.PlayOneShot(barFillingSound, transform.position);
    }

    public void RockSound()
    {
        RuntimeManager.PlayOneShot(rock, transform.position);
    }

    public void ScissorkSound()
    {
        RuntimeManager.PlayOneShot(scissor, transform.position);
    }

    public void PaperSound()
    {
        RuntimeManager.PlayOneShot(paper, transform.position);
    }
}
