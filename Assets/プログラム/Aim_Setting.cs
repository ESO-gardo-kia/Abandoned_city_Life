using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Aim_Setting : MonoBehaviour
{
    public Slider slider;
    [SerializeField] AudioMixer audioMixer;
    public void OnButtonClick()
    {
        audioMixer.SetFloat("Master", slider.value);
    }
}
