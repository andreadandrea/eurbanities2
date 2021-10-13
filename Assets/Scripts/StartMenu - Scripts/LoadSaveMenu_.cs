using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadSaveMenu_ : MonoBehaviour
{
    [SerializeField] Animator animator;
    GlobalStateManager_ gsm_;

    [SerializeField] string sceneToLoad;
    [SerializeField] Text inputField;

    private void Start()
    {
        gsm_ = GlobalStateManager_.Instance;
        gsm_.load = false;
        //CheckForSaveFile();
        Debug.Log(gsm_.CheckIfSaveExist(gsm_.currentSave));
    }

    //private void CheckForSaveFile()
    //{
    //    if (!gsm_.CheckIfSaveExist(gsm_.currentSave))
    //    {
    //        animator.SetBool("FirstStart", true);
    //    }
    //    else animator.SetBool("FirstStart", false);
    //}

    public void StartNewgame()
    {
        if (inputField.text == "") return;
        Settings_.settings.PlayerName = inputField.text;
        StartCoroutine(Simon_LevelLoader.Instance.LoadLevel(sceneToLoad, "StartMenu"));
        int index = SceneManager.GetSceneByName(sceneToLoad).buildIndex;
        gsm_.SetInt("currentScene", index);
    }


    public void LoadLastSave()
    {
        gsm_.load = true; // Load savedgame
    }
}