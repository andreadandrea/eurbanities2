using System.Collections;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class CarSpawnerScript_ : MonoBehaviour
{
    public GameObject[] carPrefab;

    public float yLevel;
    
    [Range(1, 500)]
    [SerializeField] private float minCarSpeed = 4f;
    [Range(1, 500)]
    [SerializeField] private float maxCarSpeed = 8f;
    [Range(1, 500)]
    [SerializeField] private float spawnMinSeconds = 5f;
    [Range(1, 500)]
    [SerializeField] private float spawnMaxSeconds = 30f;

    #if UNITY_EDITOR
        public bool displaySpawnLevel;
    #endif

    private Vector2 leftSpawnPosition = Vector2.zero;
    private Vector2 rightSpawnPosition = Vector2.zero;
    
    private Transform carPool;    
    private Bounds bounds;
    private Camera cam;

    
    private Vector2 carSize;
    private int selectedCar;
    

    [SerializeField] private Color[] carColors;

    private void Start()
    {
        cam = Camera.main;
        carPool = transform;

        CalculateBounds();
        CalculateYLevel();
        SpawnPositions();
        
        StartCoroutine(SpawnCars());
    }

    public void OnDrawGizmos()
    {
        #if UNITY_EDITOR
        if (displaySpawnLevel)
        {
            
            Handles.color = Color.red;
            Handles.DrawLine(leftSpawnPosition, rightSpawnPosition, 10);
            
        }
        #endif

        RestrainVariables();
        CalculateYLevel();
    }

    private IEnumerator SpawnCars()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(spawnMinSeconds, spawnMaxSeconds));
            
            CalculateBounds();
            SpawnPositions();
            RestrainVariables();
            
            float carSpeed = Random.Range(minCarSpeed, maxCarSpeed);
            Vector3 spawnPosition;
            Vector3 moveDirection;
            var flip = false;
            
            if(Random.Range(0f, 1f) >= 0.5f)
            {
                spawnPosition = leftSpawnPosition;
                moveDirection = (rightSpawnPosition - leftSpawnPosition).normalized * carSpeed;
                flip = true;
            }
            else
            {
                spawnPosition = rightSpawnPosition + new Vector2(0, .8f);
                moveDirection = (leftSpawnPosition - rightSpawnPosition).normalized * carSpeed;
            }
            
            foreach (Transform t in carPool)
            {
                
            
                if (!bounds.Contains(t.position))
                {
                    Destroy(t.gameObject);
                }
            }
            
            selectedCar = Random.Range(0, carPrefab.Length);
            GameObject go = Instantiate(carPrefab[selectedCar], spawnPosition, carPrefab[selectedCar].transform.rotation, carPool);
            
            

            go.GetComponent<Rigidbody2D>().velocity = moveDirection;
            go.transform.rotation *= Quaternion.Euler(0f, flip ? 180f : 0,0);
            var localScale = flip ? 1f : 0.8f;
            go.transform.localScale *= localScale;
            go.GetComponent<SpriteRenderer>().color = carColors[Random.Range(0, carColors.Length)];

            foreach (var spr in go.GetComponentsInChildren<SpriteRenderer>())
            {
                if (spr.gameObject == go)
                    spr.sortingOrder = flip ? 8 : 5;
                else if (spr.gameObject.name == "Window")
                    spr.sortingOrder = flip ? 7 : 4;
                else
                    spr.sortingOrder = flip ? 9 : 6;
            }
        }
    }

    private void SpawnPositions()
    {
        leftSpawnPosition.x = bounds.min.x * 1f - carSize.x;
        rightSpawnPosition.x = bounds.max.x * 1f + carSize.x;
    }

    private void CalculateBounds()
    {
        carSize = carPrefab[selectedCar].GetComponent<SpriteRenderer>().bounds.size;
        
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;

        Vector2 position = cam.transform.position;
        bounds = new Bounds(new Vector2(position.x, position.y), new Vector2(width + carSize.x, height + carSize.y));
    }

    private void CalculateYLevel()
    {
        leftSpawnPosition.y = yLevel;
        rightSpawnPosition.y = yLevel;
    }

    private void RestrainVariables()
    {
        if (minCarSpeed > maxCarSpeed)
            maxCarSpeed = minCarSpeed;

        if (spawnMinSeconds > spawnMaxSeconds)
            spawnMaxSeconds = spawnMinSeconds;
    }
}
