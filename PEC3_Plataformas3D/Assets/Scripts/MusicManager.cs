using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    public static Action<bool> OnMusicOnChanged;
    public static Action<bool> OnSFXOnChanged;

    [SerializeField] AudioMixer mixer;
    [SerializeField] AudioMixerSnapshot[] musicOnSFXOn;
    [SerializeField] AudioMixerSnapshot[] musicOnSFXOff;
    [SerializeField] AudioMixerSnapshot[] musicOffSFXOn;
    [SerializeField] AudioMixerSnapshot[] musicOffSFXOff;

    private bool isMusicOn;
    private bool areSFXOn;
    private float[] weigth = new float[] { 1 };

    public bool IsMusicOn => isMusicOn;
    public bool AreSFXOn => areSFXOn;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        isMusicOn = true;
        areSFXOn = true;
    }

    public void SwitchAudio(bool changeMusic, bool changeSFX)
    {
        isMusicOn = changeMusic ? !isMusicOn : isMusicOn;
        areSFXOn = changeSFX ? !areSFXOn : areSFXOn;

        OnMusicOnChanged?.Invoke(isMusicOn);
        OnSFXOnChanged?.Invoke(areSFXOn);

        if (isMusicOn && areSFXOn)
        {
            mixer.TransitionToSnapshots(musicOnSFXOn, weigth, 0);
        }
        else if (isMusicOn && !areSFXOn)
        {
            mixer.TransitionToSnapshots(musicOnSFXOff, weigth, 0);
        }
        else if (!isMusicOn && areSFXOn)
        {
            mixer.TransitionToSnapshots(musicOffSFXOn, weigth, 0);
        }
        else if (!isMusicOn && !areSFXOn)
        {
            mixer.TransitionToSnapshots(musicOffSFXOff, weigth, 0);
        }
    }
}
