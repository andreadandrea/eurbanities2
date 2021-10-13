using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class MenuSelectionSound_ : MonoBehaviour
{
    [EventRef] [SerializeField] private string selectionSound = "event:/UI/Global_Selection";

    public void SelectionSound()
    {
        RuntimeManager.PlayOneShot(selectionSound, transform.position);
    }
}
