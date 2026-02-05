using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistantMusicManager : MonoBehaviour
{
    public static PersistantMusicManager Instance;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip mainMusic;

    private string[] noMusicScenes =
    {
        "SceneForBuild",
        "GoodEnd",
        "BadEnd"
    };

    private string lastSceneName = ""; // pour savoir d’où on vient

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        PlayMainMusic();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string currentScene = scene.name;

        // Si scène sans musique → stop
        if (IsNoMusicScene(currentScene))
        {
            StopMusic();
        }
        else
        {
            // Redémarre seulement si on revient au menu depuis une scène "no music"
            if (currentScene == "MainMenuScene" && IsNoMusicScene(lastSceneName))
            {
                RestartMusic();
            }
            else if (!musicSource.isPlaying)
            {
                // Sinon continue ou démarre si jamais ça ne joue pas
                PlayMainMusic();
            }
        }

        lastSceneName = currentScene;
    }

    private bool IsNoMusicScene(string sceneName)
    {
        foreach (string s in noMusicScenes)
        {
            if (sceneName == s)
                return true;
        }
        return false;
    }

    private void PlayMainMusic()
    {
        if (!musicSource.isPlaying)
        {
            musicSource.clip = mainMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    private void RestartMusic()
    {
        musicSource.Stop();
        PlayMainMusic();
    }

    private void StopMusic()
    {
        if (musicSource.isPlaying)
            musicSource.Stop();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
