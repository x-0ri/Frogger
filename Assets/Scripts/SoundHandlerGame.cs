using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandlerGame : MonoBehaviour
{
    public AudioSource Music;
    public AudioSource ButtonClick;
    public AudioSource Jump;
    public AudioSource DeathCar;
    public AudioSource DeathWater;
    public AudioSource Win;
    public AudioSource GameOver;

    public void PlayMusic()
    {
        Music.PlayOneShot(Music.clip, Settings.MusicVolume / 100);
    }

    public void StopMusic()
    {
        Music.Stop();
    }

    public void PlayJumpSound()
    {
        Jump.PlayOneShot(Jump.clip, Settings.EffectsVolume / 100);
    }

    public void PlayDeathCarSound()
    {
        StopMusic();
        DeathCar.PlayOneShot(DeathCar.clip, Settings.EffectsVolume / 100);
    }

    public void PlayDeathWaterSound()
    {
        StopMusic();
        DeathWater.PlayOneShot(DeathWater.clip, Settings.EffectsVolume / 100);
    }
    public void PlayButtonClick()
    {
        ButtonClick.PlayOneShot(ButtonClick.clip, Settings.EffectsVolume / 100);
    }

    public void PlayWinSound()
    {
        ButtonClick.PlayOneShot(Win.clip, Settings.EffectsVolume / 100);
    }

    public void PlayGameOverSound()
    {
        ButtonClick.PlayOneShot(GameOver.clip, Settings.EffectsVolume / 100);
    }
}
