using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettings_ : MonoBehaviour
{
    FMOD.Studio.VCA Music;
    FMOD.Studio.VCA Sound;
	private bool musicChanged;
	private bool soundChanged;
    [SerializeField] float MusicVolume = 0.7f;
    [SerializeField] float SoundVolume = 0.7f;
	[SerializeField] Slider soundVolumeSlider; 
	[SerializeField] Slider musicVolumeSlider; 
	private GlobalStateManager_ GSM;

    void Awake()
    {
        GSM = GlobalStateManager_.Instance;
    
        Music = FMODUnity.RuntimeManager.GetVCA("vca:/MUSIC");
        Sound = FMODUnity.RuntimeManager.GetVCA("vca:/SOUND");
		soundVolumeSlider = transform.Find("SoundSlider").GetComponent<Slider>();
		musicVolumeSlider = transform.Find("MusicSlider").GetComponent<Slider>();
		
		if(GSM.GetBool("soundChanged") == true)
			SoundVolume = GSM.GetInt("soundVolume")/100f;
		
		if(GSM.GetBool("musicChanged") == true)
			MusicVolume = GSM.GetInt("musicVolume")/100f;

		soundVolumeSlider.value = SoundVolume;
		musicVolumeSlider.value = MusicVolume;
    }

    void Update()
    {
        Music.setVolume(MusicVolume);
        Sound.setVolume(SoundVolume);
    }

    /*public void MasterVolumeLevel(float newMasterVolume)
    {
        MasterVolume = newMasterVolume;
    }*/

    public void MusicVolumeLevel(float newMusicVolume)
    {
		GSM.SetInt("musicVolume", (int)(newMusicVolume * 100));
        MusicVolume = newMusicVolume;
		GSM.SetBool("musicChanged", true);
    }

    public void SoundVolumeLevel(float newSoundVolume)
    {
		GSM.SetInt("soundVolume", (int)(newSoundVolume * 100));
        SoundVolume = newSoundVolume;
		GSM.SetBool("soundChanged", true);
    }
}
