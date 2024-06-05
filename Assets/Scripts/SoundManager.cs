using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public List<AudioClip> audioClipList;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    public void PlayOneShot(string clipName)
    {
        foreach (AudioClip clip in audioClipList)
        {
            if(clip.name == clipName)
            {
                AudioSource source = new GameObject("source").AddComponent<AudioSource>();
                source.clip = clip;
                source.Play();
                Destroy(source.gameObject, clip.length);
            }
        }
    }
}
