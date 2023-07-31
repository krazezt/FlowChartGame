using System;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    [HideInInspector]
    public static AudioManager instance;

    [Range(0f, 1f)]
    public float BGMVolume;

    [Range(0f, 1f)]
    public float SFXVolume;

    public Sound[] BGMList;
    public Sound[] SFXList;

    private void Awake() {
        if (instance == null)
            instance = this;
        else {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (var bgm in BGMList) {
            bgm.audioSource = gameObject.AddComponent<AudioSource>();
            bgm.audioSource.clip = bgm.clip;
            bgm.audioSource.volume = BGMVolume * bgm.volumeMultiply;
            bgm.audioSource.pitch = bgm.pitch;
            bgm.audioSource.loop = bgm.loop;
        }

        foreach (var sfx in SFXList) {
            sfx.audioSource = gameObject.AddComponent<AudioSource>();
            sfx.audioSource.clip = sfx.clip;
            sfx.audioSource.volume = SFXVolume * sfx.volumeMultiply;
            sfx.audioSource.pitch = sfx.pitch;
            sfx.audioSource.loop = sfx.loop;
        }

        PlayBGM_MainTheme();
    }

    public void SetBGMVolume(float value) {
        BGMVolume = value;
        UpdateBGMVolume();
    }

    public void SetSFXVolume(float value) {
        SFXVolume = value;
        UpdateSFXVolume();
    }

    private void UpdateBGMVolume() {
        foreach (var bgm in BGMList) {
            bgm.audioSource.volume = BGMVolume * bgm.volumeMultiply;
        }
    }

    private void UpdateSFXVolume() {
        foreach (var sfx in SFXList) {
            sfx.audioSource.volume = SFXVolume * sfx.volumeMultiply;
        }
    }

    public void PlayBGM_MainTheme() {
        PlayBGM(AudioConfig.Track.MainTheme);
    }

    public void PlaySFX_ButtonClick() {
        PlaySFX(AudioConfig.Track.ButtonTouch);
    }

    public void PlayBGM(AudioConfig.Track track) {
        Sound bgm = Array.Find(BGMList, s => s.track == track);
        bgm?.audioSource.Play();
    }

    public void PlaySFX(AudioConfig.Track track) {
        Sound sfx = Array.Find(SFXList, s => s.track == track);
        sfx?.audioSource.PlayOneShot(sfx?.audioSource.clip);
    }
}