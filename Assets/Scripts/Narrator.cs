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

    internal bool isNarratorEnabled = true;
    private bool hasPlayedWarningClip;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        isNarratorEnabled = PlayerPrefs.GetInt("NarratorEnabled", 1) == 1;
        if (narratorAudioSource == null) narratorAudioSource = GetComponent<AudioSource>();
        if (dangerZoneScanner == null) dangerZoneScanner = FindObjectOfType<DangerZoneScanner>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            ToggleNarrator();
        }

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

        narratorAudioSource.Stop();

        if (!isNarratorEnabled)
        {
            // Play directly on the source so it ignores the isNarratorEnabled check!
            if (narratorToggleOffClip != null) narratorAudioSource.PlayOneShot(narratorToggleOffClip);
        }
        else
        {
            Speak(narratorToggleOnClip);
        }
    }

    public void Speak(AudioClip clip)
    {
        if (!isNarratorEnabled || clip == null) return;
        narratorAudioSource.Stop();
        narratorAudioSource.PlayOneShot(clip);
    }
}