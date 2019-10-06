using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectMananger_script : MonoBehaviour
{
    private AudioSource _sfxSource;
    private AudioSource _musicSource;
    private UIManager_script _uiManager;

    void Start()
    {
        _sfxSource = GetComponents<AudioSource>()[0];
        _musicSource = GetComponents<AudioSource>()[1];
        _uiManager = GetComponent<UIManager_script>();
        CheckMutes();
    }

    public void PlayEffect(AudioClip clip)
    {
        float p = Random.Range(0.8f, 1.2f);
        _sfxSource.pitch = p;
        _sfxSource.PlayOneShot(clip);
    }

    public void ToggleSfxVolume(bool b)
    {
        _sfxSource.mute = b;
        if (b)
        {
            PlayerPrefs.SetInt("sfx", 1);
        }
        else
        {
            PlayerPrefs.SetInt("sfx", 0);
        }
    }

    public void ToggleMusicVolume(bool b)
    {
        _musicSource.mute = b;
        if (b)
        {
            PlayerPrefs.SetInt("music", 1);
        }
        else
        {
            PlayerPrefs.SetInt("music", 0);
        }
    }

    void CheckMutes()
    {
        bool s = false;
        bool m = false;
        if (PlayerPrefs.GetInt("sfx") == 1)
        {
            _sfxSource.mute = true;
            s = true;
        }
        if (PlayerPrefs.GetInt("music") == 1)
        {
            _musicSource.mute = true;
            m = true;
        }
        _uiManager.SetToggles(s,m);
    }
}
