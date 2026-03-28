using UnityEngine;

public class Narrator : MonoBehaviour
{
    public DangerZoneScanner dangerZoneScanner;
    public AudioSource narratorAudioSource;
    public AudioClip wallWarningClip;
    public AudioClip winClip;

    private bool isOff = false;
    private bool hasPlayedWarningClip;

    void Start()
    {
        GameObject narrator = GameObject.Find("Narrator");
        narratorAudioSource = narrator.GetComponent<AudioSource>();
        dangerZoneScanner = FindObjectOfType<DangerZoneScanner>();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.K))
        {
            if(!isOff)
            {
                narratorAudioSource.Stop();
                narratorAudioSource.volume = 0f;
                isOff = !isOff;
            }
            else
            {
                narratorAudioSource.Play();
                narratorAudioSource.volume = 1f;
                isOff = !isOff;
            }
           
        }

        if (dangerZoneScanner.dangerEmitters[0].volume >= 0.3f && !hasPlayedWarningClip)
        {
            narratorAudioSource.Stop();
            narratorAudioSource.PlayOneShot(wallWarningClip);
            hasPlayedWarningClip = true;
        }
    }
}
