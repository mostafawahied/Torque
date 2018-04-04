using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OptionMenu : MonoBehaviour {

    public AudioMixer audiomixer;
        
    public void Option(float volume)
    {
        audiomixer.SetFloat("volume",volume);
    }
}
