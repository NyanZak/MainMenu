Main Menu System Guide
==================

This documentation describes how to use the `SettingsMenu` component in your project.

Behaviours
----------

-   \[`AudioManager`\]
-   \[`MainMenu`\]
-   \[`SettingsMenu`\]
-   \[`StopSounds`\]
  
AudioManager
------------------------
Description

### Properties

-   `Sounds` 
-   `Name`
-   `Clip`
-   `Volume`
-   `Pitch`
-   `Loop`
-   `Group`

### Script

```
using UnityEngine.Audio;
```

```
public Sound[] sounds;
    public static AudioManager instance;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.group;

            s.source.loop = s.loop;
        }
    }
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.volume = s.volume;
        s.source.pitch = s.pitch;
        s.source.Play();
    }
}
```
   
MainMenu
------------------------

Description

### Properties

-   `APP_PC_Link` 
-   `Twittername Parameter`

### Script
```
using UnityEngine.SceneManagement;
```

```
[SerializeField] private string APP_PC_LINK = "INSERT LINK HERE";
   [SerializeField] private string twitternameParameter = "Just played GAME TITLE, GAME DESCRIPTION";
    private string twitterdescriptionParam = "";
    private const string TWITTER_ADDRESS = "http://twitter.com/intent/tweet";
    private const string TWITTER_LANGUAGE = "en";
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void Promote()
    {
        Application.OpenURL(TWITTER_ADDRESS + "?text=" + WWW.EscapeURL(twitternameParameter + "\n" + twitterdescriptionParam + "\n" + APP_PC_LINK));
    }
    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}
```

SettingsMenu
-------

Description

### Properties

-   `AudioMixer` 
-   `MusicMixer`
-   `SFXMixer`
-   `ResolutionDropdown`

### Script
```
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;
```

```
public AudioMixer AudioMixer;
    public AudioMixerGroup MusicMixer;
    public AudioMixerGroup SFXMixer;
    public TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;
    private void Start()
    {
        resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToArray();
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.width &&
                resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }
    public void LoadScene(int level)
    {
        SceneManager.LoadScene(level);
    }
    public void SetResolution(int ResolutionIndex)
    {
        Resolution resolution = resolutions[ResolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void SetVolume(float volume)
    {
        AudioMixer.SetFloat("volume", volume);
    }
    public void SetMusicVolume(float MusicVolume)
    {
        AudioMixer.SetFloat("musicvolume", MusicVolume);
    }
   public void SetSFXVolume(float SFXVolume)
    {
        AudioMixer.SetFloat("sfxvolume", SFXVolume);
    }
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
```

StopSounds
----------------------------------

Description

### Properties

-   `Audio Source` - Animation for the door opening

### Script
```
    private AudioSource[] allAudioSources;
    public void StopAllAudio()
    {
        allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach (AudioSource audioS in allAudioSources)
        {
            Debug.Log("MUSIC STOPPED");
            audioS.Stop();
        }
    }
}
```
