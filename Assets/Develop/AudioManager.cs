using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager I;
    public AudioClip KnockClip;
    private List<AudioSource> _audioSource = new List<AudioSource>();

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        I = this;
    }

    public void PlayKnock()
    {
        playAudio(KnockClip);
    }

    private void playAudio(AudioClip audioClip)
    {
        var audioSource = getAudioSource();
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    private AudioSource getAudioSource()
    {
        AudioSource audioSource = null;
        foreach (var item in _audioSource)
        {
            if(!item.isPlaying)
            {
                audioSource = item;
                break;
            }
        }
        if(audioSource==null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.loop = false;
        }

        return audioSource;
    }

}
