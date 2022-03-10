using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STOPSOUNDS : MonoBehaviour
{
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
