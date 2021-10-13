using System;
using UnityEngine;

public class ProximityDoor_ : MonoBehaviour
{
    private Animator anim;
    
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.CompareTag("Player")) return;
        
        anim.SetBool("playerNear", true);
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if(!other.CompareTag("Player")) return;
        
        anim.SetBool("playerNear", false);
    }
}
