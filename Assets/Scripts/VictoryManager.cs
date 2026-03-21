using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject victoryPanel;

    [Header("Audio")]
    public AudioSource victoryAudioSource;
    public AudioClip victoryVoiceLine; // e.g., "You have found the exit."
    public AudioClip victorySoundEffect; // e.g., Triumphant chime

    public SceneFader sceneFader;

    void Start()
    {
        victoryPanel = GameObject.Find("VictoryPanel");
        victoryPanel.SetActive(false);
        victoryAudioSource = GetComponent<AudioSource>();
        victoryPanel.SetActive(false);
        sceneFader = FindObjectOfType<SceneFader>();
    }

    public void TriggerVictory()
    {
        // 1. Show the Menu
        victoryPanel.SetActive(true);

        // 2. Play the Sounds
        if (victoryAudioSource != null)
        {
            victoryAudioSource.PlayOneShot(victorySoundEffect);
            victoryAudioSource.PlayOneShot(victoryVoiceLine);
        }

        // 3. Freeze the game so footsteps and echolocation stop
        Time.timeScale = 0f;
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1f; // CRITICAL: Unfreeze the game before loading!
        // Loads the next scene in the Build Settings index
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }

    private IEnumerator DelayAction()
    {
        //sceneFader.FadeToScene();
        yield return new WaitForSeconds(3);
    }
}
