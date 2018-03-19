using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    //array för att spara bildupplösningsinställningar.
    Resolution[] resolutions;

    public Dropdown resolutionDropdown;

    //hämtar grafikkortets bildupplösningsinställningar. För att göra det behöver vi rensa alternativen så listan först är tom
    //sedan skapar vi en lista av strängar. Vi loopar igenom vår array med bildupplösningsinställningar och skapar en sträng av och en.
    //Dessa läggs sedan till i rullistans alternativ. Vi kollar också nuvarande bildupplösning och skriver in den i dropdown-menyn överst.
    private void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && 
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetMasterVol(float volume)
    {
        audioMixer.SetFloat("MasterVol", volume);
    }

    public void SetMusicVol(float volume)
    {
        audioMixer.SetFloat("MusicVol", volume);
    }

    public void SetSFXVol(float volume)
    {
        audioMixer.SetFloat("SFXVol", volume);
    }

    public void SetFullscreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
    }



}
