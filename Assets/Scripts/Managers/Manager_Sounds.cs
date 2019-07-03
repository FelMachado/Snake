using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Sounds : MonoBehaviour
{
    [SerializeField]
    List<AudioClip> audioClips = new List<AudioClip>();

    AudioSource audioSource;

    private void Start()
    {
        this.audioSource = this.GetComponent<AudioSource>();
    }

    public void PlaySound(int sound, float volume, float pitch)
    {
        this.audioSource.pitch = pitch;
        this.audioSource.volume = pitch;
        this.audioSource.PlayOneShot(this.audioClips[sound], volume);
    }
}
