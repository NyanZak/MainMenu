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
This behaviour allows you to store all your audio files in one gameobject, adjusting the files properties and being able to reference them.

### Properties

-   `Sounds` - List that contains all the sounds
-   `Name` - Name of the audio clip to refer to it
-   `Clip` - Location of the audio file
-   `Volume` - How loud the audio is
-   `Pitch` - Adjust the pitch of the audio
-   `Loop` - Decided if the audio clip should loop once it reaches the end
-   `Group` - The Audio group that this clip belongs to, in order to adjust audio levels as a group.

### Script
In order to use audio files we must reference Unity's Audio system.

```
using UnityEngine.Audio;
```
We create a list that stores all the audio files and instance it so that it will be present in all of our scenes, so we can use all the sounds in every single scene, for each audio clip that we add we add some properties that we can later adjust in the Inspector.

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
```
    
In order to call sounds within other scripts we create a public void with the string which will be our audioclips  `Name`
    
```
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

This behaviour allows you to load Twitter with preset messages to promote the game, you can also exit the game and switch to the next scene using buttons.

### Properties

-   `APP_PC_Link` - Link to the site where your game is located.
-   `Twittername Parameter` - Message included in the tweet.

### Script
In order to switch scenes we will need to use Unity's SceneManagement.

```
using UnityEngine.SceneManagement;
```
We create a few strings that will change what is tweeted for us, we are able to link the game as well as a brief description of it, in order to bring up twitters create tweet we link to its url, and you can even change the language if you or the game are not english.

```
[SerializeField] private string APP_PC_LINK = "INSERT LINK HERE";
[SerializeField] private string twitternameParameter = "Just played GAME TITLE, GAME DESCRIPTION";
private string twitterdescriptionParam = "";
private const string TWITTER_ADDRESS = "http://twitter.com/intent/tweet";
private const string TWITTER_LANGUAGE = "en";
```

A few voids are made which will be mapped onto Button's OnClick event, this allows us to quickly transition the scene to the next one in the build index list, and you can also quit the game at the same time. Using the strings we created we can reference them when loading up the url which will take players to a new window.

```
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

Stops all the sounds in the scene playing.

### Properties

-   `Audio Source` - References all the audio sources present in the scene.

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
