using System.Collections.Generic;
using UnityEngine;

public class SoundManager : IService
{
    AudioSource BGMPlayer;
    List<AudioPlayer> audioPlayers = new List<AudioPlayer>();

    public void PlayBGM(string path)
    {
        if (BGMPlayer == null)
        {
            GameObject go = new GameObject() { name = "BGMPlayer" };
            BGMPlayer = go.AddComponent<AudioSource>();
            GameObject.DontDestroyOnLoad(go);
            BGMPlayer.playOnAwake = false;
            BGMPlayer.loop = true;
        }
        ResourceManager res = ServiceLocator.Get<ResourceManager>();
        AudioClip clip = res.Load<AudioClip>(path);
        BGMPlayer.clip = clip;
        BGMPlayer.Play();
    }
    public void PlaySFX(string path)
    {
        ResourceManager res = ServiceLocator.Get<ResourceManager>();
        AudioClip clip = res.Load<AudioClip>(path);
        foreach (AudioPlayer player in audioPlayers)
        {
            if (!player.IsPlaying)
            {
                player.gameObject.SetActive(true);
                player.Play(clip);
                return;
            }
        }
        
        GameObject go = res.Load<GameObject>("Prefab/AudioPlayer");
        AudioPlayer audioPlayer = go.GetComponent<AudioPlayer>();
        audioPlayer.Play(clip);
        audioPlayers.Add(audioPlayer);
    }
}
