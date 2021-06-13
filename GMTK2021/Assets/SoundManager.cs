using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public GameObject SoundPlayerPrefab;
    public AudioClip Larve;
    public AudioClip TakeChunk;
    public AudioClip DropChunk;

    private void _play(AudioClip clip)
    {
        StartCoroutine(_playCorout(clip));
    }
    private IEnumerator _playCorout(AudioClip clip)
    {
        var soundInstance = GameObject.Instantiate(SoundPlayerPrefab);
        var audioSource = soundInstance.GetComponent<AudioSource>();

        audioSource.clip = clip;
        audioSource.Play();
        yield return new WaitForSeconds(clip.length+0.2f);
        Destroy(soundInstance);
    }

    public void PlayLarve()
    {
        _play(Larve);
    }
    public void PlayTake()
    {
        _play(TakeChunk);
    }

    public void PlayDrop()
    {
        _play(DropChunk);
    }
}
