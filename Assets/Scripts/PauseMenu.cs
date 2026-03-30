using TMPro;
using UnityEngine;
using UnityEngine.UI; // Change to TMPro if using TextMeshPro!

// This creates a neat list in your Inspector where you can set up as many settings as you want!
[System.Serializable]
public class AudioSetting
{
    public string settingName;       // e.g., "Ambience"
    public string prefsKey;          // e.g., "AmbienceVol"
    public float currentVolume;      // The actual 0.0f to 1.0f value
    public AudioClip spokenNameClip; // "Ambience Volume" voice line
    public TextMeshProUGUI uiNumberText;        // The UI text that displays 0 through 10
    public GameObject highlightBox;  // The visual outline/background to show it is selected
}

public class PauseMenu : MonoBehaviour
{
    private enum MenuState { Playing, Paused, Settings }
    private MenuState currentState = MenuState.Playing;

    [Header("UI Panels")]
    public GameObject pausePanel; // The main parent panel
    public GameObject settingsPanel; // The child panel

    [Header("Menu Voice Clips")]
    public AudioClip pauseIntroClip;
    public AudioClip pauseOutroClip;
    public AudioClip settingsIntroClip;

    [Header("Settings List")]
    // You will set the size of this array in the Inspector and drag your UI elements in!
    public AudioSetting[] settingsList;
    private int currentIndex = 0;

    void Start()
    {
        if (pausePanel != null) pausePanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);

        // Load all saved volumes and update their UI text to 0-10
        for (int i = 0; i < settingsList.Length; i++)
        {
            settingsList[i].currentVolume = PlayerPrefs.GetFloat(settingsList[i].prefsKey, 1f);
            UpdateUIText(i);
        }
    }

    void Update()
    {
        switch (currentState)
        {
            case MenuState.Playing:
                if (Input.GetKeyDown(KeyCode.Escape)) OpenPauseMenu();
                break;
            case MenuState.Paused:
                HandlePauseInput();
                break;
            case MenuState.Settings:
                HandleSettingsInput();
                break;
        }
    }

    // --- PAUSE MENU ---
    private void OpenPauseMenu()
    {
        currentState = MenuState.Paused;
        Time.timeScale = 0f;

        // Pause panel STAYS ON because settings is a child of it!
        if (pausePanel != null) pausePanel.SetActive(true);
        if (settingsPanel != null) settingsPanel.SetActive(false);

        Narrator.Instance.Speak(pauseIntroClip);
    }

    public void ResumeGame()
    {
        currentState = MenuState.Playing;
        Time.timeScale = 1f;

        if (pausePanel != null) pausePanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);

        Narrator.Instance.narratorAudioSource.Stop();
        if( Narrator.Instance.isNarratorEnabled) Narrator.Instance.narratorAudioSource.PlayOneShot(pauseOutroClip);
    }

    private void HandlePauseInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) ResumeGame();
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) OpenSettings();
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)) QuitGame();
    }

    // --- SETTINGS MENU ---
    public void OpenSettings()
    {
        currentState = MenuState.Settings;

        // Keep pause panel ON, turn settings panel ON over it
        if (pausePanel != null) pausePanel.SetActive(true);
        if (settingsPanel != null) settingsPanel.SetActive(true);

        Narrator.Instance.Speak(settingsIntroClip);

        currentIndex = 0;
        UpdateVisualHighlights();
    }

    private void HandleSettingsInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Close settings, go back to pause menu!
            OpenPauseMenu();
            return;
        }

        // Navigate UP and DOWN (With Wrap-around logic!)
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentIndex++;
            if (currentIndex >= settingsList.Length) currentIndex = 0; // Wrap to top
            ChangeSelection();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentIndex--;
            if (currentIndex < 0) currentIndex = settingsList.Length - 1; // Wrap to bottom
            ChangeSelection();
        }

        // Adjust LEFT and RIGHT
        if (Input.GetKeyDown(KeyCode.LeftArrow)) AdjustSetting(-0.1f);
        if (Input.GetKeyDown(KeyCode.RightArrow)) AdjustSetting(0.1f);
    }

    private void ChangeSelection()
    {
        UpdateVisualHighlights();
        Narrator.Instance.Speak(settingsList[currentIndex].spokenNameClip);
    }

    private void AdjustSetting(float changeAmount)
    {
        AudioSetting currentSetting = settingsList[currentIndex];

        // Clamp between 0.0 and 1.0
        currentSetting.currentVolume = Mathf.Clamp(currentSetting.currentVolume + changeAmount, 0f, 1f);

        PlayerPrefs.SetFloat(currentSetting.prefsKey, currentSetting.currentVolume);
        PlayerPrefs.Save();

        UpdateUIText(currentIndex);
        ApplyAudioSettings();
    }

    private void UpdateUIText(int index)
    {
        if (settingsList[index].uiNumberText != null)
        {
            // Converts 1.0f to "10", 0.5f to "5", etc.
            int displayValue = Mathf.RoundToInt(settingsList[index].currentVolume * 10f);
            settingsList[index].uiNumberText.text = displayValue.ToString();
        }
    }

    private void UpdateVisualHighlights()
    {
        // Turn off all highlights, then turn on ONLY the one we have selected
        for (int i = 0; i < settingsList.Length; i++)
        {
            if (settingsList[i].highlightBox != null)
            {
                settingsList[i].highlightBox.SetActive(i == currentIndex);
            }
        }
    }

    private void ApplyAudioSettings()
    {
        // Example: Apply Ambience Volume (assuming Ambience is index 0)
        if (settingsList.Length > 0 && settingsList[0].settingName == "Ambience")
        {
            GameObject gm = GameObject.Find("GameManager");
            if (gm != null)
            {
                AudioSource amb = gm.GetComponent<AudioSource>();
                if (amb != null) amb.volume = settingsList[0].currentVolume;
            }
        }
        // Apply other settings here!
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}