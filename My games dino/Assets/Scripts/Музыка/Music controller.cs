using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MusicController : MonoBehaviour
{
    [SerializeField] private AudioClip[] backgroundMusics;
    [SerializeField] private int[] musicIndexPerScene;

    private static AudioSource primarySource;
    private static AudioSource secondarySource;
    [SerializeField] private AudioSource[] sounds;

    private float musicVolume;
    private float soundsVolume;

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundsSlider;

    private void Awake()
    {
        if (primarySource == null)
        {
            GameObject musicObject = new GameObject("BackgroundMusic");
            DontDestroyOnLoad(musicObject);

            primarySource = musicObject.AddComponent<AudioSource>();
            primarySource.loop = true;

            secondarySource = musicObject.AddComponent<AudioSource>();
            secondarySource.loop = true;
        }

        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        soundsVolume = PlayerPrefs.GetFloat("SoundsVolume", 1f);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        foreach (var source in sounds)
            source.volume = soundsVolume;

        if (musicSlider != null)
            musicSlider.value = musicVolume;
        if (soundsSlider != null)
            soundsSlider.value = soundsVolume;

        UpdateMusicForScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateMusicForScene(scene.buildIndex);
    }

    // Переименованный метод (бывший ForceCheckSceneMusic)
    public void UpdateMusicForScene(int sceneIndex)
    {
        if (sceneIndex < 0 || sceneIndex >= musicIndexPerScene.Length) return;

        int musicIndex = musicIndexPerScene[sceneIndex];

        if (musicIndex == -1)
        {
            if (primarySource.isPlaying) primarySource.Stop();
            if (secondarySource.isPlaying) secondarySource.Stop();
            return;
        }

        if (musicIndex >= 0 && musicIndex < backgroundMusics.Length)
        {
            if (primarySource.isPlaying && primarySource.clip == backgroundMusics[musicIndex])
                return;

            primarySource.Stop();
            primarySource.clip = backgroundMusics[musicIndex];
            primarySource.volume = musicVolume;
            primarySource.Play();
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);

        if (primarySource != null)
            primarySource.volume = musicVolume;

        if (secondarySource != null)
            secondarySource.volume = musicVolume;
    }

    public void SetSoundsVolume(float volume)
    {
        soundsVolume = volume;
        PlayerPrefs.SetFloat("SoundsVolume", soundsVolume);

        foreach (var source in sounds)
            if (source != null)
                source.volume = soundsVolume;
    }

    public void PlaySecondaryTrack(AudioClip clip)
    {
        if (clip == null) return;

        secondarySource.clip = clip;
        secondarySource.volume = musicVolume;
        secondarySource.Play();
    }

    public void StopSecondaryTrack()
    {
        if (secondarySource.isPlaying)
            secondarySource.Stop();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}