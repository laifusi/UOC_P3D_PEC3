using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OptionsTextUI : MonoBehaviour
{
    [SerializeField] private bool musicText;
    [SerializeField] private bool sfxText;

    private TMP_Text text;

    private void Start()
    {
        text = GetComponent<TMP_Text>();

        if(musicText)
        {
            MusicManager.OnMusicOnChanged += ChangeText;
            ChangeText(MusicManager.Instance.IsMusicOn);
        }

        if(sfxText)
        {
            MusicManager.OnSFXOnChanged += ChangeText;
            ChangeText(MusicManager.Instance.AreSFXOn);
        }
    }

    private void ChangeText(bool isOn)
    {
        if (isOn)
        {
            if(musicText)
                text.SetText("Music: ON");
            if (sfxText)
                text.SetText("SFX: ON");
        }
        else
        {
            if (musicText)
                text.SetText("Music: OFF");
            if (sfxText)
                text.SetText("SFX: OFF");
        }
    }

    private void OnDestroy()
    {
        if (musicText)
        {
            MusicManager.OnMusicOnChanged -= ChangeText;
        }

        if (sfxText)
        {
            MusicManager.OnSFXOnChanged -= ChangeText;
        }
    }
}
