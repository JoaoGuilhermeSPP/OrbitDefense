using UnityEngine;
using System.Collections;

public class SFXManager : MonoBehaviour
{
    [Header("Clips")]
    public AudioClip bulletPlayer;
    public AudioClip bulletEnemys;
    public AudioClip Laser;
    public AudioClip splosion;
    public AudioClip Life;
    public AudioClip Buff;
    public AudioClip BulletB;
    public AudioClip ExplosionB;

    [Header("Configurações")]
    [Range(0f, 1f)]
    public float volume = 1f;

    public static SFXManager current;

    private AudioSource audioSource;

    private void Awake()
    {
        // Singleton
        if (current != null && current != this)
        {
            Destroy(gameObject);
            return;
        }

        current = this;

        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.volume = PlayerPrefs.GetFloat("Volume", volume);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null)
            return;



        audioSource.PlayOneShot(clip);
    }

    public void StopMusic()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }

    public void SetVolume(float value)
    {
        volume = value;

        if (audioSource != null)
        {
            audioSource.volume = value;
        }

        PlayerPrefs.SetFloat("Volume", value);
        PlayerPrefs.Save();
    }

    private void OnDestroy()
    {
        if (current == this)
        {
            current = null;
        }
    }
}