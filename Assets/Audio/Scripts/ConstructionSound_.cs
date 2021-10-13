using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class ConstructionSound_ : MonoBehaviour
{
    [EventRef] [SerializeField] private string constructionSound = "event:/Global/Effect/Construction";

    public void ConstructionSound()
    {
        RuntimeManager.PlayOneShot(constructionSound, transform.position);
    }
}

