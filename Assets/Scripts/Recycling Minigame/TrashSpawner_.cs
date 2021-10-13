using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TrashSpawner_ : MonoBehaviour
{


    [SerializeField] GameObject[] trashType;
    [SerializeField] int[] trashTypeAmount;
    int[] initialTrashTypeAmount = new int[3];
    int[] missedTrashTypeAmount = new int[3];
    [SerializeField] float spawnRate = 1.5f;
    float spawnRateTimer;
    [SerializeField] float[] spawnPos;
    [SerializeField] float maxTrashInScene;
    int maxToSpawn;
    int initialMaxToSpawn;
    [SerializeField] float waitTimeGameOver = 5;
    private int missedTrash;
    private int currentTrashSpawn = 0;
    Queue<GameObject> trashQueue = new Queue<GameObject>();
    GameObject trashParent;
    const string TRASH_PARENT_NAME = "TrashParent";
    bool spawnTrash = true;
    bool isPlay = true;
    Trashcans_ trashCans;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine( Pause(true,2));
        initialTrashTypeAmount[0] = trashTypeAmount[0];
        initialTrashTypeAmount[1] = trashTypeAmount[1];
        initialTrashTypeAmount[2] = trashTypeAmount[2];
        for (int i = 0; i < trashTypeAmount.Length; i++)
        {
            maxToSpawn += trashTypeAmount[i];

        }

        initialMaxToSpawn = maxToSpawn;
        trashCans = FindObjectOfType<Trashcans_>();
        CreateTrashParent();
    }
    private void CreateTrashParent()
    {
        trashParent = GameObject.Find(TRASH_PARENT_NAME);
        if (!trashParent)
        {
            trashParent = new GameObject(TRASH_PARENT_NAME);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public bool IsPlay()
    {
        return isPlay;
    }
    public void AddMissedTrash()
    {
        missedTrash++;
    }
    public int GetMissedTrash()
    {
        return missedTrash;
    }
    public void AddMissedTrashTypeAmount(int type)
    {
        missedTrashTypeAmount[type]++;
    }
    private IEnumerator GameOver(float time)
    {
        yield return new WaitForSeconds(time);
        isPlay = false;
    }
    private void FixedUpdate()
    {
        if (isPlay)
        {

            spawnRateTimer += Time.deltaTime;
            if (spawnRateTimer >= spawnRate && spawnTrash)
            {
                currentTrashSpawn++;
                if (currentTrashSpawn >= maxToSpawn)
                {
                    StartCoroutine(GameOver(waitTimeGameOver));
                    spawnTrash = false;
                }

                GameObject newTrash;
                int trashTypeToSpawn = Random.Range(0, trashType.Length);
                if (trashTypeAmount[0] > 0 || trashTypeAmount[1] > 0 || trashTypeAmount[2] > 0)
                {

                    while (trashTypeAmount[trashTypeToSpawn] <= 0)
                    {
                        trashTypeToSpawn = Random.Range(0, trashType.Length);
                    }
                    trashTypeAmount[trashTypeToSpawn]--;
                    newTrash = Instantiate(trashType[trashTypeToSpawn], new Vector3(transform.position.x + spawnPos[Random.Range(0, spawnPos.Length)], transform.position.y, transform.position.y), transform.rotation) as GameObject;
                    Debug.Log("Spawn");
                    newTrash.transform.parent = trashParent.transform;
                    trashQueue.Enqueue(newTrash);
                }


                spawnRateTimer = 0;
                if (trashQueue.Count > maxTrashInScene) Destroy(trashQueue.Dequeue());
            }
        }
       // else Time.timeScale = 0;
    }

    public void RestartScene()
    {
        Time.timeScale = 1;
        spawnTrash = true;
        currentTrashSpawn = 0;
        isPlay = true;
        if (missedTrash > 0)
        {
            trashTypeAmount[0] = missedTrashTypeAmount[0];
            trashTypeAmount[1] = missedTrashTypeAmount[1];
            trashTypeAmount[2] = missedTrashTypeAmount[2];
            maxToSpawn = missedTrash;
            missedTrash = 0;
        }
        else
        {

            trashTypeAmount[0] = initialTrashTypeAmount[0];
            trashTypeAmount[1] = initialTrashTypeAmount[1];
            trashTypeAmount[2] = initialTrashTypeAmount[2];
            maxToSpawn = initialMaxToSpawn;
            trashCans.SetPoint(0, 0);
        }
        for (int i = 0; i < missedTrashTypeAmount.Length; i++)
        {
            missedTrashTypeAmount[i] = 0;
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        for (int i = 0; i < spawnPos.Length; i++)
        {

            Gizmos.DrawSphere(new Vector3(transform.position.x + spawnPos[i], transform.position.y, transform.position.y), 0.5f);
        }
    }
    
    private IEnumerator Pause(bool pause,float time )
    {
        yield return new WaitForSeconds(time);
        if (pause) Time.timeScale = 0;
        else Time.timeScale = 1;
    }

    public void Pause(bool pause)
    {
       
        if (pause) Time.timeScale = 0;
        else Time.timeScale = 1;
    }
        
}