using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    [SerializeField] private AudioSource backgroundAudioSource;
    public AudioClip[] bgmList;
    private int currentMusicIndex = 0;

    private const string BGM_VOLUME_KEY = "BGM_Volume_Save";
    private float currentBgmVolume = 1f;
    private float currentSfxVolume = 1f; 

    private GameObject sfxPrefab;

    private void Awake()
    {
        if (Instance == null) 
        { 
            Instance = this; 
            DontDestroyOnLoad(gameObject);

            sfxPrefab = new GameObject("SFX_Prefab");
            sfxPrefab.AddComponent<AudioSource>();
            sfxPrefab.SetActive(false);
            DontDestroyOnLoad(sfxPrefab);
        }
        else 
        {
            Destroy(gameObject);
        }

        LoadVolume();
    }

    private void OnEnable() { SceneManager.sceneLoaded += OnSceneChange; }
    private void OnDisable() { SceneManager.sceneLoaded -= OnSceneChange; }

    private void Start()
    {
        AudioListener.volume = 1f; 
        PlayMusic(currentMusicIndex);
    }

    private void OnSceneChange(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.ToLower() == "endingscene")
        {
            StopMusic();
        }
        else
        {
            if (!backgroundAudioSource.isPlaying)
            {
                PlayMusic(currentMusicIndex);
            }
        }
    }

    public void PlayMusic(int index)
    {
        if (index < 0 || index >= bgmList.Length) return;
        currentMusicIndex = index;
        backgroundAudioSource.clip = bgmList[index];
        backgroundAudioSource.loop = true;
        backgroundAudioSource.volume = currentBgmVolume;
        backgroundAudioSource.Play();
    }

    public void StopMusic()
    {
        backgroundAudioSource.Stop();
    }

    public void IncreaseVolume()
    {
        currentBgmVolume = Mathf.Clamp01(currentBgmVolume + 0.1f);
        UpdateSourceVolume();
    }

    public void DecreaseVolume()
    {
        currentBgmVolume = Mathf.Clamp01(currentBgmVolume - 0.1f);
        UpdateSourceVolume();
    }

    private void UpdateSourceVolume()
    {
        backgroundAudioSource.volume = currentBgmVolume;
        PlayerPrefs.SetFloat(BGM_VOLUME_KEY, currentBgmVolume);
        PlayerPrefs.Save();
    }

    private void LoadVolume()
    {
        currentBgmVolume = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, 1f);
        backgroundAudioSource.volume = currentBgmVolume;
    }

    public void NextMusic()
    {
        currentMusicIndex++;
        if (currentMusicIndex >= bgmList.Length) currentMusicIndex = 0;
        PlayMusic(currentMusicIndex);
    }

    public string GetCurrentMusicName()
    {
        if (bgmList.Length == 0) return "No Music";
        return bgmList[currentMusicIndex].name;
    }

    public void PlaySFX(AudioClip clip, float volume, float pitch)
    {
        if (clip == null) return;

        GameObject sfxObj = Instantiate(sfxPrefab, Vector3.zero, Quaternion.identity);
        sfxObj.name = $"SFX_{clip.name}";
        sfxObj.SetActive(true);

        AudioSource source = sfxObj.GetComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume * currentSfxVolume; 
        source.pitch = pitch;
        source.spatialBlend = 0f; 

        source.Play();

        Destroy(sfxObj, clip.length + 0.1f);
    }
}