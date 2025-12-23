using UnityEngine;


public enum SoundType
{
    LAND,
    WALK,
    RAMPDOWN,
    PLATEDOWN,
    PICKUP,
    DROP,
    KEY,
    FLAME,
    SEESAW
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundList;
    private static SoundManager instance;
    public AudioSource audioSource;

    private void Awake()
    {
        instance = this;
    }
    
    public static void PlaySound(SoundType sound, float volume = 1)
    {
        instance.audioSource.PlayOneShot(instance.soundList[(int)sound], volume);
    }
}
