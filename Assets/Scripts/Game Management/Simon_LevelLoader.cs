using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Simon_LevelLoader : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float transitionTime = 1f;
    public static Simon_LevelLoader Instance { get; private set; }
    private GlobalStateManager_ GSM;
    private AmbMusicController_ soundController;

    private void Awake()
    {
        // Singleton implementation
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        } else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        GSM = GlobalStateManager_.Instance;
        soundController = FindObjectOfType<AmbMusicController_>();
    }

    private void Start()
    {
        soundController.LocationSoundSwitcher();
    }

    public IEnumerator LoadLevel(string sceneToLoad, string sceneToUnload)
    {
        animator.SetTrigger("End"); 
        yield return new WaitForSeconds(transitionTime);
        SceneManager.UnloadSceneAsync(sceneToUnload);
        Resources.UnloadUnusedAssets();
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
        animator.SetTrigger("Start");
        GSM.SetBool("canMove", true);
        
        int index = SceneManager.GetSceneByName(sceneToLoad).buildIndex;
        GSM.SetInt("currentScene", index);
        soundController.LocationSoundSwitcher();
    }
    
    public IEnumerator LoadLevelFromMap(string sceneToLoad)
    {
        int unload = GSM.GetInt("currentScene");
        
        if (unload > 1)
        {
            SceneManager.UnloadSceneAsync(unload);
            Resources.UnloadUnusedAssets();
        }

        if (sceneToLoad != "")
            SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
        
        int index = SceneManager.GetSceneByName(sceneToLoad).buildIndex;
        GSM.SetInt("currentScene", index);
        soundController.LocationSoundSwitcher();

        yield break;
    }
}
