using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource winSource;
    [SerializeField] AudioSource loseSource;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    public void PlayWinSound()
    {
        winSource.Play();
    }

    public void PlayLoseSound()
    {
        loseSource.Play();
    }

    public void SwapMusic(AudioClip musicClip)
    {
        if (musicSource.clip != musicClip) 
        {
            musicSource.Stop();
            musicSource.clip = musicClip;
            musicSource.Play();
        }
    }
}
