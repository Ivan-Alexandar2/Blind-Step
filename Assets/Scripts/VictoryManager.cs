using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject victoryPanel;

    [Header("Audio")]
    public AudioSource victoryAudioSource;
    public AudioClip victoryVoiceLine;
    public AudioClip victorySoundEffect;

    public SceneFader sceneFader;
    public AudioSource buttonPressAudio;

    void Start()
    {
        victoryPanel = GameObject.Find("VictoryPanel");
        if (victoryPanel != null) victoryPanel.SetActive(false);

        victoryAudioSource = GetComponent<AudioSource>();
        sceneFader = FindObjectOfType<SceneFader>();

        GameObject pressBtnAudio = GameObject.Find("ButtonAudio");
        buttonPressAudio = pressBtnAudio.GetComponent<AudioSource>();
    }

    public void TriggerVictory()
    {
        if (victoryPanel != null) victoryPanel.SetActive(true);

        if (victoryAudioSource != null)
        {
            victoryAudioSource.PlayOneShot(victorySoundEffect);
            victoryAudioSource.PlayOneShot(victoryVoiceLine);
        }

        Time.timeScale = 0f;
    }

    public void LoadNextLevel()
    {
        // Tell the Fader to load the next index in the Build Settings
        buttonPressAudio.Play();
        if (sceneFader != null) sceneFader.FadeToScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartLevel()
    {
        // Tell the Fader to load the CURRENT index
        buttonPressAudio.Play();
        if (sceneFader != null) sceneFader.FadeToScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        buttonPressAudio.Play();
        // Tell the Fader to trigger the Application.Quit callback
        if (sceneFader != null) sceneFader.FadeToQuit();
    }
}