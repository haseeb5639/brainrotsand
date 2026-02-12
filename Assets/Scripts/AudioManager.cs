
//using UnityEngine;

//public class AudioManager : MonoBehaviour
//{
//    [Header("🎵 Audio Sources")]
//    public AudioSource backgroundMusic;
//    public AudioSource sfxSource;

//    [Header("🔊 Sound Clips")]
//    public AudioClip pickClip;
//    public AudioClip dropClip;
//    public AudioClip buttonClip;

//    private static AudioManager instance;

//    public static bool MusicEnabled { get; private set; }
//    public static bool SoundEnabled { get; private set; }

//    void Awake()
//    {
//        if (instance == null)
//        {
//            instance = this;
//            DontDestroyOnLoad(gameObject);
//            LoadSettings();
//        }
//        else
//        {
//            Destroy(gameObject);
//            return;
//        }
//    }

//    void Start()
//    {
//        // Auto-start background music when MainMenu loads
//        if (backgroundMusic != null)
//        {
//            if (MusicEnabled)
//            {
//                backgroundMusic.loop = true;
//                backgroundMusic.Play();
//                Debug.Log("🎶 Background music started automatically.");
//            }
//            else
//            {
//                backgroundMusic.Stop();
//            }
//        }
//    }

//    public static void PlayPickSound()
//    {
//        if (instance && SoundEnabled && instance.pickClip)
//            instance.sfxSource.PlayOneShot(instance.pickClip);
//    }

//    public static void PlayDropSound()
//    {
//        if (instance && SoundEnabled && instance.dropClip)
//            instance.sfxSource.PlayOneShot(instance.dropClip);
//    }

//    public static void PlayButtonSound()
//    {
//        if (instance && SoundEnabled && instance.buttonClip)
//            instance.sfxSource.PlayOneShot(instance.buttonClip);
//    }

//    public static void ToggleMusic(bool state)
//    {
//        MusicEnabled = state;
//        if (instance && instance.backgroundMusic)
//        {
//            if (MusicEnabled)
//                instance.backgroundMusic.Play();
//            else
//                instance.backgroundMusic.Stop();
//        }
//        PlayerPrefs.SetInt("Music", MusicEnabled ? 1 : 0);
//    }

//    public static void ToggleSound(bool state)
//    {
//        SoundEnabled = state;
//        PlayerPrefs.SetInt("Sound", SoundEnabled ? 1 : 0);
//    }

//    public static void LoadSettings()
//    {
//        MusicEnabled = PlayerPrefs.GetInt("Music", 1) == 1;
//        SoundEnabled = PlayerPrefs.GetInt("Sound", 1) == 1;
//    }

//    public static void ApplySettings()
//    {
//        ToggleMusic(MusicEnabled);
//        ToggleSound(SoundEnabled);
//    }
//}


////
///

using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("🎵 Audio Sources")]
    public AudioSource backgroundMusic;
    public AudioSource sfxSource;

    [Header("🔊 Sound Clips")]
    public AudioClip pickClip;
    public AudioClip dropClip;
    public AudioClip buttonClip;
    public AudioClip AIClip;
    public AudioClip damageClip;   // 💥 NEW — damage sound

    private static AudioManager instance;

    public static bool MusicEnabled { get; private set; }
    public static bool SoundEnabled { get; private set; }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadSettings();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        if (backgroundMusic != null)
        {
            if (MusicEnabled)
            {
                backgroundMusic.loop = true;
                backgroundMusic.Play();
                Debug.Log("🎶 Background music started automatically.");
            }
            else
            {
                backgroundMusic.Stop();
            }
        }
    }

    // =================== SOUND METHODS ===================

    public static void PlayPickSound()
    {
        if (instance && SoundEnabled && instance.pickClip)
            instance.sfxSource.PlayOneShot(instance.pickClip);
    }

    public static void PlayDropSound()
    {
        if (instance && SoundEnabled && instance.dropClip)
            instance.sfxSource.PlayOneShot(instance.dropClip);
    }

    //public static void PlayButtonSound()
    //{
    //    if (instance && SoundEnabled && instance.buttonClip)
    //        instance.sfxSource.PlayOneShot(instance.buttonClip);
    //}

    public static void PlayAISound()
    {
        if (instance && SoundEnabled && instance.AIClip)
            instance.sfxSource.PlayOneShot(instance.AIClip);
    }

    // 💥 NEW DAMAGE SOUND
    public static void PlayDamageSound()
    {
        if (instance && SoundEnabled && instance.damageClip)
            instance.sfxSource.PlayOneShot(instance.damageClip);
    }

    // =====================================================

    public static void ToggleMusic(bool state)
    {
        MusicEnabled = state;
        if (instance && instance.backgroundMusic)
        {
            if (MusicEnabled)
                instance.backgroundMusic.Play();
            else
                instance.backgroundMusic.Stop();
        }
        PlayerPrefs.SetInt("Music", MusicEnabled ? 1 : 0);
    }

    public static void ToggleSound(bool state)
    {
        SoundEnabled = state;
        PlayerPrefs.SetInt("Sound", SoundEnabled ? 1 : 0);
    }

    public static void LoadSettings()
    {
        MusicEnabled = PlayerPrefs.GetInt("Music", 1) == 1;
        SoundEnabled = PlayerPrefs.GetInt("Sound", 1) == 1;
    }

    public static void ApplySettings()
    {
        ToggleMusic(MusicEnabled);
        ToggleSound(SoundEnabled);
    }
}
