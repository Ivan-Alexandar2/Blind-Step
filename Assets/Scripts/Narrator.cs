using UnityEngine;

public class Narrator : MonoBehaviour
{
    public static Narrator Instance;

    [Header("Components")]
    public AudioSource narratorAudioSource;
    public DangerZoneScanner dangerZoneScanner;

    [Header("In-Game Clips")]
    public AudioClip wallWarningClip;
    public AudioClip winClip;
    public AudioClip narratorToggleOnClip;
    public AudioClip narratorToggleOffClip;

    private bool isNarratorEnabled = true;
    private bool hasPlayedWarningClip;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Load the saved preference (1 = true, 0 = false)
        isNarratorEnabled = PlayerPrefs.GetInt("NarratorEnabled", 1) == 1;

        if (narratorAudioSource == null) narratorAudioSource = GetComponent<AudioSource>();
        if (dangerZoneScanner == null) dangerZoneScanner = FindObjectOfType<DangerZoneScanner>();
    }

    void Update()
    {
        // Consistent Narrator On/Off Toggle (Let's use 'N' for Narrator)
        if (Input.GetKeyDown(KeyCode.K))
        {
            ToggleNarrator();
        }

        // Wall Warning logic
        if (dangerZoneScanner != null && dangerZoneScanner.dangerEmitters.Length > 0)
        {
            if (dangerZoneScanner.dangerEmitters[0].volume >= 0.3f && !hasPlayedWarningClip)
            {
                Speak(wallWarningClip);
                hasPlayedWarningClip = true;
            }
        }
    }

    public void ToggleNarrator()
    {
        isNarratorEnabled = !isNarratorEnabled;

        PlayerPrefs.SetInt("NarratorEnabled", isNarratorEnabled ? 1 : 0);
        PlayerPrefs.Save();

        if (!isNarratorEnabled)
        {
            narratorAudioSource.Stop();
        }
        else
        {
            Speak(narratorToggleOnClip);
        }
    }

    public void Speak(AudioClip clip)
    {
        // Don't play if the narrator is turned off or the clip is missing
        if (!isNarratorEnabled || clip == null) return;

        narratorAudioSource.Stop();
        narratorAudioSource.PlayOneShot(clip);
    }
}
