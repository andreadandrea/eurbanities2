using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner_ : MonoBehaviour
{
    [SerializeField] GameObject npc;
    [Range(0, 9)] [SerializeField] int npcAmount;
    [SerializeField] Vector3[] npcPositions = new Vector3[9];
    List<Vector3> npcPositionsTaken = new List<Vector3>();
    // Start is called before the first frame update

    private void Awake()
    {
       // npcAmount = Random.Range(0, npcPositions.Length);
        for (int i = 0; i < npcAmount; i++)
        {
           
            Vector3 positionToSpawn = npcPositions[Random.Range(0, npcPositions.Length)];
            while (npcPositionsTaken.Contains(positionToSpawn))
            {
                positionToSpawn = npcPositions[Random.Range(0, npcPositions.Length)];
            }
            npcPositionsTaken.Add ( positionToSpawn);
            GameObject newNpc =  Instantiate(npc, positionToSpawn, transform.rotation) as GameObject;
            newNpc.transform.parent = this.transform;
        }

    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 255, 0, 1f);
        for (int i = 0; i < npcPositions.Length; i++)
        {
            Gizmos.DrawSphere(npcPositions[i], 0.2f);
        }

    }
}
