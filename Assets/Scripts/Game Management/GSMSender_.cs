using System;
using UnityEngine;

public class GSMSender_ : MonoBehaviour
{
    private GlobalStateManager_ GSM;

    [Serializable]
    public struct Variables
    {
        public string key;
        public int value;
    }
    
    public Variables[] vars; 

    private void Start()
    {
        GSM = GlobalStateManager_.Instance;
    }

    public void Execute()
    {
        foreach (var var in vars)
        {
            GSM.SetInt(var.key, var.value);
        }
    }
}
