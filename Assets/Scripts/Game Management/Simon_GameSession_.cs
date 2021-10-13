using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simon_GameSession_ : MonoBehaviour
{
    private static Simon_GameSession_ instance;
    public static Simon_GameSession_ Instance { get { return instance; } }

    [Header("Stops Player Movement If False")]
    public bool canMove = true;
    public bool inDialog = false;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        } else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}