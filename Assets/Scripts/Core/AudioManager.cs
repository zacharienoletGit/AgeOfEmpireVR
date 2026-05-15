using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioClip buttonClip;
    public AudioClip buildClip;
    public AudioClip hitClip;
    public AudioClip deathClip;

    AudioSource source;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        source = GetComponent<AudioSource>();

        if (source == null)
            source = gameObject.AddComponent<AudioSource>();
    }

    public void PlayButton()
    {
        Play(buttonClip);
    }

    public void PlayBuild()
    {
        Play(buildClip);
    }

    public void PlayHit()
    {
        Play(hitClip);
    }

    public void PlayDeath()
    {
        Play(deathClip);
    }

    void Play(AudioClip clip)
    {
        // If no free sound is imported yet, just do nothing.
        if (clip == null || source == null)
            return;

        source.PlayOneShot(clip);
    }
}
