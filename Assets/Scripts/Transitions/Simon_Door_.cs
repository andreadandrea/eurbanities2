using System;
using UnityEngine;
using FMODUnity;

public class Simon_Door_ : MonoBehaviour
{
    public float waitTime = 0.5f;
    [SerializeField] private string sceneToLoad;
    [Range(-10f, 10f)][SerializeField] float xSpawnOffset = 0;
    Simon_CameraZoom_ cameraZoom;
    private Simon_LevelLoader levelLoader;

    [SerializeField] [EventRef] private string openDoor = "event:/Global/Door";

    private GlobalStateManager_ GSM;

    public static event Action<GameObject> DoorClicked;
    public static event Action DoorTouched;

    private bool clickedOn;
    private bool transitioning;
    void Start()
    {
        cameraZoom = FindObjectOfType<Simon_CameraZoom_>();
        levelLoader = Simon_LevelLoader.Instance;
        GSM = GlobalStateManager_.Instance;

        clickedOn = false;
        transitioning = false;
    }
    
    private void OnMouseUp()
    {
        clickedOn = true;
        DoorClicked?.Invoke(gameObject);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player")) {
            if(other.gameObject.GetComponent<Simon_FollowTarget_>().insideArea)
            {
                clickedOn = true;
            }
        }
        if (!other.gameObject.CompareTag("Player") || !clickedOn || transitioning) return;
        GSM.SetBool("canMove", false);
        transitioning = true;
        
        var transformPosition = transform.position;
        cameraZoom.target = transformPosition;
        RuntimeManager.PlayOneShot(openDoor, transformPosition);

        var playerPosition = other.transform.position;
        GSM.SetPositionOfPlayer(gameObject.scene.name,
            new Vector3(playerPosition.x + xSpawnOffset, playerPosition.y, playerPosition.z));

        StartCoroutine(levelLoader.LoadLevel(sceneToLoad, gameObject.scene.name));
        cameraZoom.zoomActive = true;
    }
}
