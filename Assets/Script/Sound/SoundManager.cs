using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioClip backgroundMusic;
    public AudioClip buttonClickSound;
    
    [Range(0f, 1f)]
    public float musicVolume = 0.7f;
    [Range(0f, 1f)]
    public float sfxVolume = 1f;
    
    public bool playMusicOnStart = true;
    public bool loopMusic = true;
    
    public static SoundManager Instance;
    
    private const string MUSIC_VOLUME_KEY = "MusicVolume";
    private const string SFX_VOLUME_KEY = "SFXVolume";
    private const string MUSIC_ENABLED_KEY = "MusicEnabled";
    private const string SFX_ENABLED_KEY = "SFXEnabled";
    
    public bool IsMusicEnabled { get; private set; } = true;
    public bool IsSFXEnabled { get; private set; } = true;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeSoundManager();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        if (playMusicOnStart && backgroundMusic != null)
        {
            PlayBackgroundMusic();
        }
    }
    
    private void InitializeSoundManager()
    {
        if (musicSource == null)
        {
            GameObject musicObj = new GameObject("MusicSource");
            musicObj.transform.SetParent(transform);
            musicSource = musicObj.AddComponent<AudioSource>();
        }
        
        if (sfxSource == null)
        {
            GameObject sfxObj = new GameObject("SFXSource");
            sfxObj.transform.SetParent(transform);
            sfxSource = sfxObj.AddComponent<AudioSource>();
        }
        
        LoadVolumeSettings();
        SetupAudioSources();
    }
    
    private void SetupAudioSources()
    {
        musicSource.clip = backgroundMusic;
        musicSource.loop = loopMusic;
        musicSource.playOnAwake = false;
        musicSource.volume = musicVolume;
        
        sfxSource.playOnAwake = false;
        sfxSource.volume = sfxVolume;
    }
    
    private void LoadVolumeSettings()
    {
        musicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 0.7f);
        sfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1f);
        IsMusicEnabled = PlayerPrefs.GetInt(MUSIC_ENABLED_KEY, 1) == 1;
        IsSFXEnabled = PlayerPrefs.GetInt(SFX_ENABLED_KEY, 1) == 1;
    }
    
    public void PlayBackgroundMusic()
    {
        if (backgroundMusic != null && musicSource != null && IsMusicEnabled)
        {
            musicSource.clip = backgroundMusic;
            musicSource.Play();
        }
    }
    
    public void PlayBackgroundMusic(AudioClip newMusic)
    {
        if (newMusic != null && musicSource != null && IsMusicEnabled)
        {
            backgroundMusic = newMusic;
            musicSource.clip = newMusic;
            musicSource.Play();
        }
    }
    
    public void StopBackgroundMusic()
    {
        if (musicSource != null && musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }
    
    public void PauseBackgroundMusic()
    {
        if (musicSource != null && musicSource.isPlaying)
        {
            musicSource.Pause();
        }
    }
    
    public void ResumeBackgroundMusic()
    {
        if (musicSource != null && IsMusicEnabled)
        {
            musicSource.UnPause();
        }
    }
    
    public void PlayButtonClick()
    {
        PlaySFX(buttonClickSound);
    }
    
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null && IsSFXEnabled)
        {
            sfxSource.PlayOneShot(clip, sfxVolume);
        }
    }
    
    public void PlaySFX(AudioClip clip, float volume)
    {
        if (clip != null && sfxSource != null && IsSFXEnabled)
        {
            sfxSource.PlayOneShot(clip, volume * sfxVolume);
        }
    }
    
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        if (musicSource != null)
        {
            musicSource.volume = IsMusicEnabled ? musicVolume : 0f;
        }
        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, musicVolume);
    }
    
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        if (sfxSource != null)
        {
            sfxSource.volume = IsSFXEnabled ? sfxVolume : 0f;
        }
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, sfxVolume);
    }
    
    public void ToggleMusic()
    {
        IsMusicEnabled = !IsMusicEnabled;
        
        if (musicSource != null)
        {
            if (IsMusicEnabled)
            {
                musicSource.volume = musicVolume;
                if (!musicSource.isPlaying && backgroundMusic != null)
                {
                    PlayBackgroundMusic();
                }
            }
            else
            {
                musicSource.volume = 0f;
            }
        }
        
        PlayerPrefs.SetInt(MUSIC_ENABLED_KEY, IsMusicEnabled ? 1 : 0);
    }
    
    public void ToggleSFX()
    {
        IsSFXEnabled = !IsSFXEnabled;
        
        if (sfxSource != null)
        {
            sfxSource.volume = IsSFXEnabled ? sfxVolume : 0f;
        }
        
        PlayerPrefs.SetInt(SFX_ENABLED_KEY, IsSFXEnabled ? 1 : 0);
    }
    
    public void SaveSettings()
    {
        PlayerPrefs.Save();
    }
    
    public bool IsMusicPlaying()
    {
        return musicSource != null && musicSource.isPlaying;
    }
}