using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private PlayableDirector timelineDirector;
    [SerializeField] private int nextSceneIndex = 4;
    [SerializeField] private GameObject skipText;
    private bool canSkip = true;

    void Start()
    {
        GameStateManager.Instance.SetState(GameState.Cutscene);

        if (PlayerPrefs.GetInt("IntroCutsceneWatched", 0) == 1)
        {
            LoadNextScene();
            return;
        }

        if (timelineDirector != null)
        {
            timelineDirector.Play();
            timelineDirector.stopped += OnTimelineStopped;
        }

        if (skipText != null) skipText.SetActive(true);
    }

    void Update()
    {
        if (canSkip && Input.GetKeyDown(KeyCode.Return))
        {
            SkipCutscene();
        }
    }

    private void OnTimelineStopped(PlayableDirector director)
    {
        PlayerPrefs.SetInt("IntroCutsceneWatched", 1);
        PlayerPrefs.Save();
        LoadNextScene();
        GameStateManager.Instance.SetState(GameState.Gameplay);
    }

    private void SkipCutscene()
    {
        canSkip = false;
        if (timelineDirector != null) timelineDirector.Stop();
        if (skipText != null) skipText.SetActive(false);
        OnTimelineStopped(timelineDirector);
    }

    private void LoadNextScene()
    {
        PlayerPrefs.Save();

        MusicController musicController = FindObjectOfType<MusicController>();
        if (musicController != null)
        {
            musicController.UpdateMusicForScene(nextSceneIndex);
        }

        SceneManager.LoadScene(nextSceneIndex);
    }

    public void ResetCutsceneWatched()
    {
        PlayerPrefs.SetInt("IntroCutsceneWatched", 0);
        PlayerPrefs.Save();
    }
}