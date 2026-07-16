using UnityEngine;
using UnityEngine.UI;

public class Volume : MonoBehaviour
{
    public AudioSource audioSource;
    public Slider volumeSlider;


    public void Volumes()
    {
        float vol = volumeSlider.value;


        audioSource.volume = vol;

        PlayerPrefs.SetFloat("Volume", vol);
    }
   

  
}
