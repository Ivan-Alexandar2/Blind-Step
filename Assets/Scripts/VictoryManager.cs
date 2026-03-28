using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject victoryPanel;

    [Header("Audio")]
    public AudioSource victoryAudioSource;
    public Narrator narrator;

    public SceneFader sceneFader;
    public AudioSource buttonPressAudio;

    void Start()
    {
        victoryPanel = GameObject.Find("VictoryPanel");
        if (victoryPanel != null) victoryPanel.SetActive(false);

        sceneFader = FindObjectOfType<SceneFader>();

        GameObject pressBtnAudio = GameObject.Find("ButtonAudio");
        buttonPressAudio = pressBtnAudio.GetComponent<AudioSource>();
        narrator = FindObjectOfType<Narrator>();
    }

    private void Update()
    {
        if(victoryPanel.active)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                LoadNextLevel();
            if (Input.GetKeyDown(KeyCode.Alpha2))
                RestartLevel();
            if (Input.GetKeyDown(KeyCode.Alpha3))
                QuitGame();
        }     
    }

    public void TriggerVictory()
    {
        if (victoryPanel != null) victoryPanel.SetActive(true);

        victoryAudioSource.Play();
        narrator.narratorAudioSource.Stop();
        narrator.narratorAudioSource.PlayOneShot(narrator.winClip);

        Time.timeScale = 0f;
    }

    public void LoadNextLevel()
    {
        buttonPressAudio.Play();
        if (sceneFader != null) sceneFader.FadeToScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartLevel()
    {
        buttonPressAudio.Play();
        if (sceneFader != null) sceneFader.FadeToScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        buttonPressAudio.Play();
        if (sceneFader != null) sceneFader.FadeToQuit();
    }
}