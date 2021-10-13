using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class RecyclingUIManager_ : MonoBehaviour
{
    [SerializeField] Text[] trashTypeText;
    Trashcans_ trashCans;
    [SerializeField] GameObject scoreScreenUI, playScreenUI;
    TrashSpawner_ trashSpawner;
    [SerializeField] Text totalScoreText;
    [SerializeField] Text missedTrashText;
    // Start is called before the first frame update
    void Start()
    {
        trashSpawner = FindObjectOfType<TrashSpawner_>();
        trashCans = FindObjectOfType<Trashcans_>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        missedTrashText.text = "Missed trash: " + trashSpawner.GetMissedTrash().ToString("F0");
        totalScoreText.text = "Trash collected " + trashCans.GetTotalPoints().ToString("F0");
       // if (!trashSpawner.IsPlay()) Time.timeScale = 0;
        scoreScreenUI.SetActive(!trashSpawner.IsPlay());
        playScreenUI.SetActive(trashSpawner.IsPlay());
        missedTrashText.enabled = !trashSpawner.IsPlay() && trashSpawner.GetMissedTrash() > 0;
        for (int i = 0; i < trashTypeText.Length; i++)
        {
            trashTypeText[i].text = trashCans.GetTrashPoints(i).ToString();
            
        }
    }
    public void RestartGame()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
    }
}
