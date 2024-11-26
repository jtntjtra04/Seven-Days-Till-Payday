using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public Sound[] music_sound, bgm_sound, sfx_sound, hybrid_sound, train_sound;
    public AudioSource music_source, bgm_source, sfx_source, hybrid_source, train_source;

    private float max_music_volume = 0.8f;

    private void Awake()
    {
        if (instance == null) // to make things easier to access
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            //  Destroy(gameObject);
        }
    }
    private void Start()
    {
        string current_scene = SceneManager.GetActiveScene().name;
        if(current_scene == "GameScene")
        {
            PlayMusic("Theme"); // When game start play theme
        }
        if(current_scene == "MainMenu")
        {
            PlayMusic("MainMenu");
        }
        else
        {
            Debug.Log("No music will played at start");
        }
        
    }
    public void PlayMusic(string name) //Call this function from any script u want to add music
    {
        Sound sound = Array.Find(music_sound, x => x.name == name); //Search audio from array
        if (sound != null)
        {
            music_source.clip = sound.clip;
            music_source.Play();
        }
        else
        {
            Debug.Log("Music Not Found");
        }
    }
    public void PlayBGM(string name) //Call this function from any script u want to add music
    {
        Sound sound = Array.Find(bgm_sound, x => x.name == name); //Search audio from array
        if (sound != null)
        {
            bgm_source.clip = sound.clip;
            bgm_source.Play();
        }
        else
        {
            Debug.Log("BGM Not Found");
        }
    }
    public void PlaySFX(string name) //Call this function from any script u want to add SFX
    {
        Sound sound = Array.Find(sfx_sound, x => x.name == name); //Search audio from array
        if (sound != null)
        {
            sfx_source.PlayOneShot(sound.clip, 1f);
        }
        else
        {
            Debug.Log("SFX Not Found");
        }
    }
    public void PlayHybrid(string name)
    {
        Sound sound = Array.Find(hybrid_sound, x => x.name == name); //Search audio from array
        if (sound != null)
        {
            hybrid_source.clip = sound.clip;
            hybrid_source.Play();
        }
        else
        {
            Debug.Log("Hybrid Not Found");
        }
    }
    public void PlayTrain(string name) //Call this function from any script u want to add SFX
    {
        Sound sound = Array.Find(sfx_sound, x => x.name == name); //Search audio from array
        if (sound != null)
        {
            train_source.PlayOneShot(sound.clip, 1f);
        }
        else
        {
            Debug.Log("Train SFX Not Found");
        }
    }
    public void ToggleMusic()
    {
        music_source.mute = !music_source.mute;
    }
    public void ToggleSFX()
    {
        sfx_source.mute = !sfx_source.mute;
    }
    public void MusicVolume(float volume)
    {
        music_source.volume = Mathf.Min(volume, max_music_volume);
    }
    public void SFXVolume(float volume)
    {
        sfx_source.volume = volume;
    }
    public void DecreaseMusicVolume()
    {
        music_source.volume = 0.2f;
        //max_music_volume = Mathf.Max(0, max_music_volume - 0.2f);
    }
    public void IncreaseVolumeMusic()
    {
        music_source.volume = 0.4f;
    }
    public void ChangeMusic(string name)
    {
        Sound sound = Array.Find(music_sound, x => x.name == name);
        if (sound != null)
        {
            if (music_source.clip == null || music_source.clip == sound.clip)
            {
                Debug.Log("Same Song Played");
                return;
            }
            music_source.Stop();
            music_source.clip = sound.clip;
            music_source.clip.LoadAudioData();
            music_source.Play();
        }
        else
        {
            Debug.Log("Music Not Found");
        }
    }
}
