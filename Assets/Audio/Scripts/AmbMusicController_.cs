using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AmbMusicController_ : MonoBehaviour
{
    private GlobalStateManager_ GSM;
	[SerializeField] private int peopleJoined;
    public StudioEventEmitter music, amb;

    void Awake()
    {
        GSM = GlobalStateManager_.Instance;
    }

    public void LocationSoundSwitcher()
    {
		//MainMenu
		if (GSM.GetInt("currentScene") == 16)
        {
            music.SetParameter("Location", 11f);
            amb.SetParameter("Location", 2f);
        }
		
        //Park
        else if (GSM.GetInt("currentScene") == 4 || GSM.GetInt("currentScene") == 5)
        {
            music.SetParameter("Location", 0f);
            amb.SetParameter("Location", 0f);
            Debug.Log("Music: Park");
        }

        //TownHall
        else if (GSM.GetInt("currentScene") == 7)
        {
            music.SetParameter("Location", 1f);
            amb.SetParameter("Location", 1f);
        }
		
		//Recycling
		else if (GSM.GetInt("currentScene") == 17)
        {
            music.SetParameter("Location", 8f);
            amb.SetParameter("Location", 2f);
        }
		
        //CommunityCenter
        else if (GSM.GetInt("currentScene") == 10)
        {
            music.SetParameter("Location", 3f);
			if (peopleJoined >= 3 && peopleJoined < 6)
			{
				music.SetParameter("PeopleJoined", 0.01f);
				Debug.Log("Community Center Music Stage 1");
			}
			else if (peopleJoined >= 6 && peopleJoined < 9)
			{
				music.SetParameter("PeopleJoined", 0.2f);
				Debug.Log("Community Center Music Stage 2");
			}
			else if (peopleJoined >= 9 && peopleJoined < 12)
			{
				music.SetParameter("PeopleJoined", 0.4f);
				Debug.Log("Community Center Music Stage 3");
			}
			else if (peopleJoined >= 12 && peopleJoined < 14)
			{
				music.SetParameter("PeopleJoined", 0.6f);
				Debug.Log("Community Center Music Stage 4");
			}
			else if (peopleJoined >= 14)
			{
				music.SetParameter("PeopleJoined", 0.8f);
				Debug.Log("Community Center Music Stage 5");
			}
			else
			{
				music.SetParameter("PeopleJoined", 0.0f);
				Debug.Log("Community Center Music Stage 0");
			}
            amb.SetParameter("Location", 3f);
        }
		
		//Gym
		else if (GSM.GetInt("currentScene") == 14)
        {
            music.SetParameter("Location", 3f);
            amb.SetParameter("Location", 3f);
        }
		
		//Yoga
		else if (GSM.GetInt("currentScene") == 18)
        {
            music.SetParameter("Location", 9f);
            amb.SetParameter("Location", 3f);
        }
		
		//Gathering
		else if (GSM.GetInt("currentScene") == 19)
        {
            music.SetParameter("Location", 10f);
            amb.SetParameter("Location", 8f);
        }
		
        //FlowerShop
        else if (GSM.GetInt("currentScene") == 3)
        {
            music.SetParameter("Location", 4f);
            amb.SetParameter("Location", 4f);
        }

        //BikeShop
		else if (GSM.GetInt("currentScene") == 15)
        {
            music.SetParameter("Location", 5f);
            amb.SetParameter("Location", 5f);
        }
		
        //Bar
		else if (GSM.GetInt("currentScene") == 13)
        {
            music.SetParameter("Location", 6f);
            amb.SetParameter("Location", 6f);
        }
		
        //Garden
        else if (GSM.GetInt("currentScene") == 8)
        {
            music.SetParameter("Location", 7f);
            amb.SetParameter("Location", 7f);
        }

        //Defaults to street
        else
        {
            music.SetParameter("Location", 2f);
            amb.SetParameter("Location", 2f);
            Debug.Log("Music: Street");
        }
    }
}
