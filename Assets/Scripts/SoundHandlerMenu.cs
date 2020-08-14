using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandlerMenu : MonoBehaviour
{
    public AudioSource ButtonClick;

    public void PlayButtonClick()
    {
        ButtonClick.PlayOneShot(ButtonClick.clip, Settings.EffectsVolume / 100);
    }
}
