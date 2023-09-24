using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{

    // call "AudioManager.Instance.PlayMusic/PlaySFX("name"); to use it.

    public static AudioManager Instance;

    public AudioSource musicSource, sfxSource, ambienceSource;
    public Sound[] musicSounds, ambienceSounds, albumPickUp, creditsCamera, footstepsL, footsepsR;
    public Sound[] lookCamera, offCamera, photoAlbumPageTurnL, photoAlbumPageTurnR, shutterCamera, extraCamera;

    private void Awake() {
        if (Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else { Destroy(gameObject); }    
    }
    
    private void Start() {
        PlayMusic("MenuTheme");
        PlayAmbience("AmbientForestDay");
        /* MusicVolume(0.5f); */
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);
        if (s == null) Debug.Log("Sounds Not Found");
        else {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    public void PlayAmbience(string name)
    {
        Sound s = Array.Find(ambienceSounds, x => x.name == name);
        if (s == null) Debug.Log("Sounds Not Found");
        else {
            ambienceSource.clip = s.clip;
            ambienceSource.Play();
        }
    }
    public void PlayRandomAlbumPickup()
    {
        int randomIndex = UnityEngine.Random.Range(0, albumPickUp.Length);
        Sound s = albumPickUp[randomIndex];
        if (s == null) Debug.Log("Sounds Not Found");
        else {
            sfxSource.clip = s.clip;
            sfxSource.Play();
        }
    }
    public void PlayRandomAlbumArrowL()
    {
        int randomIndex = UnityEngine.Random.Range(0, photoAlbumPageTurnL.Length);
        Sound s = photoAlbumPageTurnL[randomIndex];
        if (s == null) Debug.Log("Sounds Not Found");
        else {
            sfxSource.clip = s.clip;
            sfxSource.Play();
        }
    }
    public void PlayRandomAlbumArrowR()
    {
        int randomIndex = UnityEngine.Random.Range(0, photoAlbumPageTurnR.Length);
        Sound s = photoAlbumPageTurnR[randomIndex];
        if (s == null) Debug.Log("Sounds Not Found");
        else {
            sfxSource.clip = s.clip;
            sfxSource.Play();
        }
    }
    public void PlayExtraCameraSFX(string name){
        Sound s = Array.Find(extraCamera, x => x.name == name);
        if (s == null) Debug.Log("SFX Not Found");
        else {
            sfxSource.PlayOneShot(s.clip);
        }
    }
    public void PlayCreditCameraSFX(string name){
        Sound s = Array.Find(creditsCamera, x => x.name == name);
        if (s == null) Debug.Log("SFX Not Found");
        else {
            sfxSource.PlayOneShot(s.clip);
        }
    }

    // int randomIndex = Random.Range(0, sounds.Length);
    // AudioClip randomSound = sounds[randomIndex];

    public void ToggleMusic() { musicSource.mute = !musicSource.mute; }
    public void ToggleSFX() { sfxSource.mute = !sfxSource.mute; }
    public void MusicVolume(float volume) { musicSource.volume = volume; }
    public void SfxVolume(float volume) { sfxSource.volume = volume; }
    public void AmbienceVolume(float volume) { ambienceSource.volume = volume; }
}