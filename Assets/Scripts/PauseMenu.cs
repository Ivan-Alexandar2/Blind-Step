using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private enum MenuState { Playing, Paused, Settings }
    private MenuState currentState = MenuState.Playing;

    [Header("UI Panels")]
    public GameObject pausePanel;
    public GameObject settingsPanel;

    [Header("Menu Voice Clips")]
    public AudioClip pauseIntroClip;    // "Paused. Press Escape to Resume, 2 for Settings, 3 to Quit."
    public AudioClip settingsIntroClip; // "Settings. Up and Down to select, Left and Right to adjust."
    public AudioClip ambienceSelectedClip; // "Ambience Volume"
    public AudioClip sfxSelectedClip;      // "SFX Volume"

    [Header("Settings Data")]
    private int currentSettingIndex = 0; // 0 = Ambience, 1 = SFX
    private float ambienceVolume;
    private float sfxVolume;

    void Start()
    {
        // Ensure menus are off when the game starts
        if (pausePanel != null) pausePanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);

        // Load saved audio volumes (Default is 1.0f / 100%)
        ambienceVolume = PlayerPrefs.GetFloat("AmbienceVol", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVol", 1f);
    }

    void Update()
    {
        // Direct input based on what screen we are currently looking at
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

    // --- MAIN PAUSE MENU ---

    private void OpenPauseMenu()
    {
        currentState = MenuState.Paused;
        Time.timeScale = 0f; // Freeze game

        if (pausePanel != null) pausePanel.SetActive(true);
        if (settingsPanel != null) settingsPanel.SetActive(false);

        Narrator.Instance.Speak(pauseIntroClip);
    }

    private void ResumeGame()
    {
        currentState = MenuState.Playing;
        Time.timeScale = 1f; // Unfreeze game

        if (pausePanel != null) pausePanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);

        // Tell the narrator to stop talking immediately when returning to game
        Narrator.Instance.narratorAudioSource.Stop();
    }

    private void HandlePauseInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) ResumeGame();
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) OpenSettings();
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)) QuitGame();
    }

    // --- SETTINGS MENU ---

    private void OpenSettings()
    {
        currentState = MenuState.Settings;
        currentSettingIndex = 0; // Always start at the top of the list (Ambience)

        if (pausePanel != null) pausePanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(true);

        Narrator.Instance.Speak(settingsIntroClip);
    }

    private void HandleSettingsInput()
    {
        // Go back to the Pause menu
        if (Input.GetKeyDown(KeyCode.Z))
        {
            OpenPauseMenu();
            return;
        }

        // Navigate UP and DOWN
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            // Toggle between index 0 (Ambience) and 1 (SFX)
            currentSettingIndex = (currentSettingIndex == 0) ? 1 : 0;

            // Read the name of the highlighted setting
            if (currentSettingIndex == 0) Narrator.Instance.Speak(ambienceSelectedClip);
            if (currentSettingIndex == 1) Narrator.Instance.Speak(sfxSelectedClip);
        }

        // Adjust LEFT and RIGHT
        if (Input.GetKeyDown(KeyCode.LeftArrow)) AdjustSetting(-0.1f); // Down 10%
        if (Input.GetKeyDown(KeyCode.RightArrow)) AdjustSetting(0.1f); // Up 10%
    }

    private void AdjustSetting(float changeAmount)
    {
        if (currentSettingIndex == 0)
        {
            ambienceVolume = Mathf.Clamp(ambienceVolume + changeAmount, 0f, 1f);
            PlayerPrefs.SetFloat("AmbienceVol", ambienceVolume);

            // TIP: If you have a clip of a generic "beep", play it here so they hear the adjustment!
        }
        else if (currentSettingIndex == 1)
        {
            sfxVolume = Mathf.Clamp(sfxVolume + changeAmount, 0f, 1f);
            PlayerPrefs.SetFloat("SFXVol", sfxVolume);
        }

        PlayerPrefs.Save();
        ApplyAudioSettings(); // Apply it instantly!
    }

    private void ApplyAudioSettings()
    {
        // Find your Ambience audio source and update it instantly
        GameObject gm = GameObject.Find("GameManager");
        if (gm != null)
        {
            AudioSource amb = gm.GetComponent<AudioSource>();
            if (amb != null) amb.volume = ambienceVolume;
        }

        // Find your footstep/echolocation script and apply the SFX volume there (you will need to write this specific line depending on where your footstep AudioSource is!)
    }

    private void QuitGame()
    {
        Time.timeScale = 1f;
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}
