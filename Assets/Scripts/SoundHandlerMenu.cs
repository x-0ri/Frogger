using UnityEngine;

public class SoundHandlerMenu : MonoBehaviour
{
    public AudioSource ButtonClick;

    public void PlayButtonClick()
    {
        ButtonClick.PlayOneShot(ButtonClick.clip, Settings.EffectsVolume / 100);
    }
}
