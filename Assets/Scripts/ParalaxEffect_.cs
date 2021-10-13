using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxEffect_ : MonoBehaviour
{
    [SerializeField] Transform mainCamera => Camera.main.transform;
    [Header("")]
    [SerializeField] Transform forground, midground, backgroundLayer1, backgroundLayer2, backgroundLayer3;
    [SerializeField] [Range(-1, 1)] float forgroundStrength, backgroundStrength1, backgroundStrength2, backgroundStrength3;
    private void FixedUpdate()
    {
        float offset = mainCamera.position.x - midground.position.x;
        forground.position = new Vector3(midground.position.x + (offset * forgroundStrength), forground.position.y);
        if(backgroundLayer1 !=null)
            backgroundLayer1.position = new Vector3(midground.position.x + (offset * backgroundStrength1), backgroundLayer1.position.y);
        if (backgroundLayer2 != null)
            backgroundLayer2.position = new Vector3(midground.position.x + (offset * backgroundStrength2), backgroundLayer2.position.y);
        if (backgroundLayer3 != null)
            backgroundLayer3.position = new Vector3(midground.position.x + (offset * backgroundStrength3), backgroundLayer3.position.y);
    }
}
